using System;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using CoreComponents.RabbitMQAdapter.Interfaces;

namespace CoreComponents.RabbitMQAdapter
{
	public class RabbitMQProducer<T>: IRabbitMQProducer<T>
    {
        
        private readonly IModel channel;
        private readonly IConnection connection;

        public RabbitMQProducer(string connectionString)
		{
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                    throw new ArgumentNullException("connectionString");

                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(connectionString)
                };
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
            }
            catch (RabbitMQ.Client.Exceptions.ConnectFailureException ex)
            {
                throw new Exception("Unable to access the specified RabbitMQ server in the connection string.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while accessing the RabbitMQ server.", ex);
            }   
        }


        public void Publish(string queueName,T data, string exchangeName = "", Enums.ExchangeType exchangeType = Enums.ExchangeType.Direct)
        {
            SendMessage(queueName, data, exchangeName, exchangeType.GetDescriptionAttribute());
        }

        public void Publish(T data, string exchangeName = "", Enums.ExchangeType exchangeType = Enums.ExchangeType.Direct)
        {
            SendMessage(typeof(T).Name, data, exchangeName, exchangeType.GetDescriptionAttribute());
        }

        private void SendMessage(string queueName, T data, string exchangeName, string exchangeType)
        {
            try
            {
                channel.QueueDeclare(queue: queueName,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);


                channel.BasicQos(0, 1, false);

                if (!string.IsNullOrEmpty(exchangeName))
                {
                    channel.ExchangeDeclare(exchangeName, exchangeType);
                    channel.QueueBind(queueName, exchangeName, "");
                }

                var channelProperties = channel.CreateBasicProperties();
                channelProperties.Persistent = true;

                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: exchangeName == "" ? queueName : "",
                                         basicProperties: exchangeName == "" ? null : channelProperties,
                                         body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to send your message to RabbitMQ.", ex);
            }
            finally
            {
                channel.Close();
                connection.Close();
            }
        }

        public void Dispose()
        {
            channel.Close();
            connection.Close();
        }
    }
}