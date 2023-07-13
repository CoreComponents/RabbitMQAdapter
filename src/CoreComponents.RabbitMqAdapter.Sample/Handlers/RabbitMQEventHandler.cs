using System.Diagnostics;
using CoreComponents.RabbitMQAdapter.Sample.Events;
using MediatR;
using Newtonsoft.Json;

namespace CoreComponents.RabbitMQAdapter.Sample.Handlers
{
	public class RabbitMQEventHandler : INotificationHandler<RabbitMQEventNotification<OrderEvent>>
    {
		public RabbitMQEventHandler()
		{
		}

        public Task Handle(RabbitMQEventNotification<OrderEvent> notification, CancellationToken cancellationToken)
        {
            Debug.Print(JsonConvert.SerializeObject(notification.EventData));
            return Task.CompletedTask;
        }
    }
}

