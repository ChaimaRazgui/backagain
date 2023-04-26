using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naxxum.WeeCare.Authentification.Domain.Entities;

namespace Naxxum.WeeCare.Authentification.Application.Services
{
    public class TokenConsumer : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TokenConsumer> _logger;

        public TokenConsumer(IServiceProvider serviceProvider, ILogger<TokenConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger; ;
            // create the RabbitMQ connection
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();

            // create the channel
            _channel = _connection.CreateModel();

            // declare the queue
            _channel.QueueDeclare("TokenDetails", exclusive: false);
            // set up a consumer to listen for messages on the queue
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnMessageReceived;
            _channel.BasicConsume(queue: "TokenDetails", autoAck: true, consumer: consumer);
        }

        public event EventHandler<UserWithRoleAndNameDto> MessageReceived;

        private void OnMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Received data : {message}");
            var userdata = JsonConvert.DeserializeObject<UserWithRoleAndNameDto>(message);
            Console.WriteLine($"Received message: Role={userdata.Role}, fullName={userdata.FullName}");
            MessageReceived?.Invoke(this, userdata);
        }


        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
