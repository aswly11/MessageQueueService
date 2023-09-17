using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Producer.API.Services
{

    public class MessageProducer : IMessageProducer
    {
        private readonly ConnectionFactory _factory;
        public MessageProducer()
        {
            _factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "user",
                Password = "mypass",
                VirtualHost = "/"

            };
        }
        public Task SendMesage<T>(T message)
        {
            var connetion = _factory.CreateConnection();

            using (var channel = connetion.CreateModel())
            {
                channel.QueueDeclare("QueueName",durable:true , exclusive:false);
                var jsonMessage = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(jsonMessage);
                channel.BasicPublish("", "QueueName", body: body);
            }
            return Task.CompletedTask;

        }
    }
}
