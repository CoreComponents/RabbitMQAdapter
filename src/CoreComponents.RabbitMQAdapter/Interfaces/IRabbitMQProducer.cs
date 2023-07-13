
using System;
using CoreComponents.RabbitMQAdapter.Enums;

namespace CoreComponents.RabbitMQAdapter.Interfaces
{
	public interface IRabbitMQProducer<T>: IDisposable
    {
        void Publish(string queueName, T data, string exchangeName = "", ExchangeType exchangeType = ExchangeType.Direct);
        void Publish(T data, string exchangeName = "", ExchangeType exchangeType = ExchangeType.Direct);
    }
}

