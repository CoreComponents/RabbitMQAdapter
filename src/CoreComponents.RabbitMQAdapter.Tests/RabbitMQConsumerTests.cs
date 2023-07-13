using MediatR;
using Moq;
using RabbitMQ.Client;

namespace CoreComponents.RabbitMQAdapter.Tests
{
    public class RabbitMQConsumerTests
	{
        private readonly string connectionString;
        private readonly Mock<IMediator> mediatorMock;

        public RabbitMQConsumerTests()
        {
            this.connectionString = "amqp://myuser:mypassword@localhost:5672";
            this.mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public void RabbitMQConsumer_Initialize_ConnectionAndChannelCreated()
        {
            

            // Act
            var consumer = new RabbitMQConsumer<string>(connectionString, mediatorMock.Object);

            // Assert
            Assert.NotNull(consumer);
            Assert.NotNull(consumer.Connection);
            Assert.NotNull(consumer.Channel);
            Assert.True(consumer.Connection.IsOpen);
            Assert.True(consumer.Channel.IsOpen);
        }

        
        [Fact]
        public async Task RabbitMQConsumer_Subscribe_ValidQueue_SubscriptionStarted()
        {
            // Arrange
            var queueName = "myQueue";
            var mediatorMock = new Mock<IMediator>();
            var channelMock = new Mock<IModel>();
            var connectionMock = new Mock<IConnection>();
            var consumerMock = new Mock<RabbitMQConsumer<string>>(connectionString, mediatorMock.Object); //new RabbitMQConsumer<string>(connectionString, mediatorMock.Object);

            var rabbitMQProducer = new RabbitMQProducer<string>(connectionString);
            var message = "Hello World";
            rabbitMQProducer.Publish(queueName, message,"");

            
            // Act
            await consumerMock.Object.Subscribe(queueName);

            // Assert
            channelMock.Verify(c => c.QueueDeclare(queueName, false, false, false, null), Times.Never);
            mediatorMock.Verify(m => m.Publish(It.Is<RabbitMQEventNotification<string>>(s => s.EventData == message), It.IsAny<CancellationToken>()), Times.Once);
        }

        
    }
}

