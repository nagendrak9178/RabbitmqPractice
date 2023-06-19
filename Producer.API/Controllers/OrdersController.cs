using Microsoft.AspNetCore.Mvc;
using Producer.API.DTOs;
using Producer.API.RabbitMQ;
using Producer.API.Repository;
using Producer.API.Repository.Entities;

namespace Producer.API.Controllers
{
    // Follow this article for reference: https://code-maze.com/aspnetcore-rabbitmq/
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderRepositoryContext _orderRepositoryContext;
        private readonly IMessageProducer _messageProducer;
        public OrdersController(OrderRepositoryContext orderRepositoryContext, IMessageProducer messageProducer) 
        {
            _orderRepositoryContext = orderRepositoryContext;
            _messageProducer = messageProducer;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrdersDTO orders)
        {
            Order orderentity = new Order()
            {
                Price = orders.Price,
                ProductName = orders.ProductName,
                Quantity = orders.Quantity
            };
            await _orderRepositoryContext.AddAsync(orderentity);
            await _orderRepositoryContext.SaveChangesAsync();
            
            //Publish the message to the RabbitMQ Producer.
            _messageProducer.SendMessage(orderentity);
            return Ok(new { Id = orderentity.Id });
        }
    }
}
