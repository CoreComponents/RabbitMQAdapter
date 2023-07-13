using System;
namespace CoreComponents.RabbitMQAdapter.Sample.Events
{
	public class OrderEvent
	{
		public Guid Id { get; set; }
		public string ClientName { get; set; }
		public decimal TotalValue { get; set; }
		
        public OrderEvent()
		{
		}
	}
}

