using CoreComponents.RabbitMQAdapter.Enums;
using CoreComponents.RabbitMQAdapter.Interfaces;
using CoreComponents.RabbitMQAdapter.Sample.Events;
using Microsoft.AspNetCore.Mvc;

namespace AppVerse.RabbitMqAdapter.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IRabbitMQProducer<OrderEvent> rabbitMQProducer;
        
        public OrderController(IRabbitMQProducer<OrderEvent> rabbitMQProducer)
        {
            this.rabbitMQProducer = rabbitMQProducer;
        }

        [HttpPost("registerOrder")]
        public async Task AddOrder(string clientName, decimal totalValue)
        {
            var order = new OrderEvent
            {
                Id = Guid.NewGuid(),
                ClientName = clientName,
                TotalValue = totalValue
            };

            rabbitMQProducer.Publish(order,"teste",ExchangeType.Fanout);
        }
    }
}

