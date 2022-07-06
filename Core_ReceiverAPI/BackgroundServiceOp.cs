using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Core_ReceiverAPI.Models;
using System.Diagnostics;

namespace Core_ReceiverAPI
{
    public class BackgroundServiceOp : BackgroundService
    {
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;
        

        public BackgroundServiceOp(ILoggerFactory loggerFactory, IServiceProvider service)
        {
            this._logger = loggerFactory.CreateLogger<BackgroundServiceOp>();

            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            // create a factory
            var factory = new ConnectionFactory()
            {
                 
                 Uri = new Uri("amqp://guest:guest@localhost:5672") 
            };

            // create a connection
            _connection = factory.CreateConnection();

            // create channel
            _channel = _connection.CreateModel();

            // channel exchange declare
            _channel.ExchangeDeclare("message.exchange", ExchangeType.Topic);
            _channel.QueueDeclare("message.queue",
                durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind("message.queue", "message.exchange", "message.queue.*", null);
            // prefetch size, prefetch count
            _channel.BasicQos(0, 1, false);

            _connection.ConnectionShutdown += _connection_ConnectionShutdown;

        }

        private void _connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, message) =>
            {
                // received body
                var content = Encoding.UTF8.GetString(message.Body.ToArray());
                // handled the received Message
                HandleReceivedMessage(content);
                // acknowledge the message
                _channel.BasicAck(message.DeliveryTag, false);
            };

            consumer.Shutdown += Consumer_Shutdown;
            consumer.Registered += Consumer_Registered;
            consumer.Unregistered += Consumer_Unregistered;
            consumer.ConsumerCancelled += Consumer_ConsumerCancelled;

            _channel.BasicConsume("message.queue", false, consumer);

            return Task.CompletedTask;
        }


        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }

        private void Consumer_ConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Consumer_Unregistered(object sender, ConsumerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Consumer_Registered(object sender, ConsumerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Consumer_Shutdown(object sender, ShutdownEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void HandleReceivedMessage(string content)
        {

            var MessageObject = new Message()
            {
                Order = new Order(),
                TotalAmount = 0,
                Advance = 0
            };

            var message = System.Text.Json.JsonSerializer.Deserialize<Message>(content);
            Debug.WriteLine($"Received Messsage {content}");
        }
    }
}
