using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naxxum.WeeCare.UserManagement.Application.Repositories;
using Naxxum.WeeCare.UserManagement.Domain.Entites;
using Newtonsoft.Json;
using System.Threading.Channels;

namespace Naxxum.WeeCare.UserManagement.Infrastructrue.Repositories
{
    public class RabbitMQService : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMQService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            // create the RabbitMQ connection
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();

            // create the channel
            _channel = _connection.CreateModel();

            // declare the queue
            _channel.QueueDeclare("user", exclusive: false);
            // set up a consumer to listen for messages on the queue
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnMessageReceived;
            _channel.BasicConsume(queue: "user", autoAck: true, consumer: consumer);
        }

        private void OnMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);


            var userCreated = JsonConvert.DeserializeObject<UserCreated>(message);
            Console.WriteLine($"Received message: UserId={userCreated.UserId}, Email={userCreated.Email}, fullName={userCreated.fullName}");
            var userDetails = new UsersDetails
            {
                UserId = userCreated.UserId,
                Email = userCreated.Email,
                fullName = userCreated.fullName,
                Role = ""
            };
            using (var scope = _serviceProvider.CreateScope())
            {
                var userReposiotry = scope.ServiceProvider.GetService<IUserDetailsRepository>();
                userReposiotry.CreateUser(userDetails);
            }
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}

