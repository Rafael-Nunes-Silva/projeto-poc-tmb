using api_poc_tmb.Data;
using api_poc_tmb.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;

namespace api_poc_tmb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly DatabaseContext _dbContext;
        private readonly ServiceBusClient _busClient;
        private readonly string _queueName = "orders-queue";

        public OrdersController(
            ILogger<OrdersController> logger,
            DatabaseContext dbContext,
            ServiceBusClient busClient
            )
        {
            _logger = logger;
            _dbContext = dbContext;
            _busClient = busClient;
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
    }
}
