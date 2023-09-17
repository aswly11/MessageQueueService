using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Producer.API.Services
{

    public class MessageProducer : IMessageProducer
    {
        public Task SendMesage<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "mypass",
                VirtualHost = "/"

            };
            var connetion = factory.CreateConnection();

            using (var channel = connetion.CreateModel())
            {
                channel.QueueDeclare("QueueName",durable:true , exclusive:true);
                var jsonMessage = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(jsonMessage);
                channel.BasicPublish("", "QueueName", body: body);
            }
            return Task.CompletedTask;

        }
    }
}
