using System;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace CoreComponents.RabbitMQAdapter.Interfaces
{
	public interface IRabbitMQConsumer<T>: IDisposable
	{
        public IConnection Connection { get;}
        public IModel Channel { get; }
        Task Subscribe(string queueName, CancellationToken cancellationToken = default);
        Task Subscribe(CancellationToken cancellationToken = default);
    }
}

