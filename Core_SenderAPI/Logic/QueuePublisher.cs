using Core_SenderAPI.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Core_SenderAPI.Logic
{
    /// <summary>
    /// This class contains logic to write approved message in the queue
    /// </summary>
    public class QueuePublisher
    {
        public void PublishMessage(Order order, double totalAmount, double advance)
        {
            // creating a connection factory
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            // create a connection (we are using the default one)
            var connection = factory.CreateConnection();
            // create a channel
            var channel = connection.CreateModel();

            // time to live message 
            var ttl = new Dictionary<string, object>
            {
                { "x-message-ttl", 60000} // message will live for 60 seconds
            };


            //                      Exchange name           ,   Exchange type   , optional argument
            channel.ExchangeDeclare("message.exchange",
                ExchangeType.Topic, arguments: ttl);
            // The message to serialize
            var MessageObject = new Message()
            {
               Order = order,
               TotalAmount = totalAmount,
               Advance = advance
            };

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(MessageObject));
            // since the routing key is message.queue.*, that matched with the pattern the messages will
            // received by queue
            channel.BasicPublish("message.exchange", "message.queue.*", null, body);


        }
    }
}
