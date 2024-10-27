using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductOrderApi.Data.Entities;
using ProductOrderApi.Data.Models;
using ProductOrderApi.Services;

namespace ProductOrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var results = await _orderService.GetOrders();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var result = await _orderService.GetOrder(id);

            if(result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        
        [HttpPost]
        public async Task<ActionResult<Order>> AddOrder(CreateOrderModel model)
        {
            return Ok(await _orderService.CreateOrder(model));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
             if(order.Id != id)
             {
                return BadRequest();
             }
            
            var result = await _orderService.UpdateOrder(order);

            if (result is null)
            {
                return NotFound();
            }

             return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrder(id);
            
            return NoContent();
        }
    }
}
