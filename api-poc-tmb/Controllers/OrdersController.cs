using api_poc_tmb.Data;
using api_poc_tmb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace api_poc_tmb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly DatabaseContext _dbContext;

        public OrdersController(
            ILogger<OrdersController> logger,
            DatabaseContext dbContext
            )
        {
            _logger = logger;
            _dbContext = dbContext;
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
                return BadRequest("Pedido inv�lido");
            }

            newOrder.Status = EOrderStatus.Pendente;
            newOrder.Data_criacao = DateTime.UtcNow;


            _dbContext.orders.Add(newOrder);
            await _dbContext.SaveChangesAsync();

            // Enviar mensagem para a fila

            _logger.LogInformation("Pedido {OrderId} criado.", newOrder.Id);

            return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.Id }, newOrder);
        }

        /// <summary>
        /// Retorna todos os pedidos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var pedidos = await _dbContext.orders.ToListAsync();
            return Ok(pedidos);
        }

        /// <summary>
        /// Retorna o pedido especificado pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrderById(int id)
        {
            var order = _dbContext.orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }
    }
}
