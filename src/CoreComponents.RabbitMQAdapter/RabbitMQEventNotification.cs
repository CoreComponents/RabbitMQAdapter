using MediatR;

namespace CoreComponents.RabbitMQAdapter
{
	public class RabbitMQEventNotification<T> : INotification
    {
        public T EventData { get; set; }
        public RabbitMQEventNotification()
		{
		}
	}
}