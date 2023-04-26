using Naxxum.WeeCare.UserManagement.Application.Repositories;
using Naxxum.WeeCare.UserManagement.Domain.Entites;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Naxxum.WeeCare.UserManagement.Infrastructrue.Repositories
{
    public class TokenService  : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TokenService> _logger;
        private readonly IRabitMQProducerD _rabitMQProducer;
        public TokenService(IServiceProvider serviceProvider, ILogger<TokenService> logger, IRabitMQProducerD rabitMQProducer)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            // create the RabbitMQ connection
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();

            // create the channel
            _channel = _connection.CreateModel();

            // declare the queue
            _channel.QueueDeclare("UserId", exclusive: false);
            // set up a consumer to listen for messages on the queue
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnMessageReceived;
            _channel.BasicConsume(queue: "UserId", autoAck: true, consumer: consumer);
            _rabitMQProducer = rabitMQProducer;
        }

        private void OnMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray(); // Use ToArray() to convert ReadOnlyMemory<byte> to byte[]
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Received user id: {message}");
            var userId = JsonConvert.DeserializeObject<int>(message);
            _logger.LogInformation($"Received user id: {userId}");
            using (var scope = _serviceProvider.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetService<IUserDetailsRepository>();
                var user = userRepository.GetUserById(userId);

                if (user is not null)
                {
                    var userWithRoleAndName = new UserWithRoleAndNameDto
                    {
                        Role = user.Role,
                        FullName = user.fullName
                    };
                    _rabitMQProducer.SendTokenDetails(userWithRoleAndName);
                }
                else
                {
                    _logger.LogError($"User with id {userId} not found");
                }
            }
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
