using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreComponents.RabbitMQAdapter.Interfaces;
using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CoreComponents.RabbitMQAdapter
{
	public class RabbitMQConsumer<T>: IRabbitMQConsumer<T> 
    {
        private readonly IModel channel;
        private readonly IConnection connection;
        private readonly IMediator mediator;

        public IConnection Connection => this.connection;
        public IModel Channel => this.channel;

        public RabbitMQConsumer(string connectionString, IMediator mediator)
		{
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            this.mediator = mediator;

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(connectionString)
            };

            this.connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        public async Task Subscribe(string queueName, CancellationToken cancellationToken = default)
        {
            await Task.Run(() => StartConsume(queueName));
        }

        public async Task Subscribe(CancellationToken cancellationToken = default)
        {
            await Task.Run(() => StartConsume(typeof(T).Name));
        }

        private void StartConsume(string queueName)
        {
            channel.QueueDeclare(queue: queueName,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                HandleMessage(content);
                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queueName, false, consumer);
        }

        private void HandleMessage(string content)
        {
            this.mediator.Publish(new RabbitMQEventNotification<T> { EventData = JsonConvert.DeserializeObject<T>(content) });

        }

        public void Dispose()
        {
            channel.Close();
            connection.Close();
        }

        
    }
}

