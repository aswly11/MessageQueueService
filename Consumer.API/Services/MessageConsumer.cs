using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;
using System.Data.Common;
using System.Threading.Channels;
using Microsoft.AspNetCore.Connections;
using Consumer.API.Data;
using Consumer.API.Models;

namespace Consumer.API.Services
{
    public class MessageConsumer : IHostedService
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private ConsumerDBContext _consumerDBContext;
        private readonly IServiceProvider serviceProvider;

        public MessageConsumer(IServiceProvider serviceProvider)
        {
            _factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "user",
                Password = "mypass",
                VirtualHost = "/"
            };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            this.serviceProvider = serviceProvider;
            var scope = serviceProvider.CreateScope();
            _consumerDBContext = scope.ServiceProvider.GetRequiredService<ConsumerDBContext>();

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await StartConsuming();
        }

        public Task StartConsuming()
        {
            _channel.QueueDeclare(queue: "QueueName", durable: true, exclusive: false);

            // Set up a consumer
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Received message: " + message);
                var MessageObject = JsonSerializer.Deserialize<Message>(message);
                await _consumerDBContext.Messages.AddAsync(MessageObject);
                await _consumerDBContext.SaveChangesAsync();
                // Process the received message
            };

            // Start consuming messages from the queue
            _channel.BasicConsume("QueueName", true, consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Close();
            _connection.Close();
            return Task.CompletedTask;

        }
       
    }
}
