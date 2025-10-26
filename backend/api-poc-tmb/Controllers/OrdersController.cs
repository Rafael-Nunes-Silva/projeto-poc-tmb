using api_poc_tmb.Data;
using api_poc_tmb.Models;
using api_poc_tmb.Services;
using api_poc_tmb.Services.Interfaces;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace api_poc_tmb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly DatabaseContext _dbContext;
        private readonly ServiceBusClient _busClient;
        private readonly IOpenAIService _openaiService;
        private readonly ILLMSqlService _llmSqlService;
        private readonly string _queueName = "orders-queue";

        public OrdersController(
            ILogger<OrdersController> logger,
            DatabaseContext dbContext,
            ServiceBusClient busClient,
            IOpenAIService openaiService,
            IConfiguration configuration,
            ILLMSqlService llmSqlService
            )
        {
            _logger = logger;
            _dbContext = dbContext;
            _busClient = busClient;
            _openaiService = openaiService;
            _llmSqlService = llmSqlService;

            var queueName = configuration.GetSection("AzureServiceBus").GetValue<string>("QueueName");
            if (queueName != null)
                _queueName = queueName;
        }

        /// <summary>
        /// Cria um novo pedido
        /// </summary>
        /// <param name="newOrder"></param>
        /// <returns>Order</returns>
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order newOrder)
        {
            if (newOrder == null)
            {
                return BadRequest("Pedido invalido");
            }

            newOrder.Status = EOrderStatus.Pendente;
            newOrder.Data_criacao = DateTime.UtcNow;

            _dbContext.orders.Add(newOrder);
            await _dbContext.SaveChangesAsync();

            var sender = _busClient.CreateSender(_queueName);

            var messageBody = JsonSerializer.Serialize(new
            {
                OrderId = newOrder.Id,
                Cliente = newOrder.Cliente,
                Produto = newOrder.Produto,
                Valor = newOrder.Valor,
                Status = newOrder.Status.ToString(),
                Data_criacao = newOrder.Data_criacao
            });

            var message = new ServiceBusMessage(messageBody)
            {
                CorrelationId = newOrder.Id.ToString(),
                ApplicationProperties =
                {
                    { "EventType", "OrderCreated" }
                }
            };

            await sender.SendMessageAsync(message);

            _logger.LogInformation("Pedido {OrderId} criado e mensagem enviada à fila.", newOrder.Id);

            return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.Id }, newOrder);
        }

        /// <summary>
        /// Retorna todos os pedidos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _dbContext.orders
                .Include(o => o.HistoricoStatus)
                .OrderByDescending((order) => order.Data_criacao).ToListAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Retorna o pedido especificado pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrderById(int id)
        {
            var order = _dbContext.orders
                .Include(o => o.HistoricoStatus)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound($"Pedido {id} não encontrado");

            return Ok(order);
        }

        /// <summary>
        /// Retorna o pedido especificado pelo Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("pergunta/{texto}")]
        public async Task<ActionResult<string>> Pergunta(string texto)
        {
            var querySql = _openaiService.GenerateSQLQuery(texto);

            var queryResult = await _llmSqlService.ExecuteDynamicSqlAsync(querySql);

            var respostaAmigavel = _openaiService.GenerateFriendlyAnswer(texto, queryResult);

            return Ok(respostaAmigavel);
        }
    }
}
