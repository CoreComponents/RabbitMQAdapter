using CoreComponents.RabbitMQAdapter.Interfaces;
using CoreComponents.RabbitMQAdapter.Sample.Events;

namespace CoreComponents.RabbitMQAdapter.Sample.Services
{
	public class RabbitMQService : BackgroundService
    {
        private readonly IRabbitMQConsumer<OrderEvent> rabbitMQConsumer;

        public RabbitMQService(IRabbitMQConsumer<OrderEvent> rabbitMQConsumer)
		{
            this.rabbitMQConsumer = rabbitMQConsumer;
		}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this.rabbitMQConsumer.Subscribe();
        }
    }
}

