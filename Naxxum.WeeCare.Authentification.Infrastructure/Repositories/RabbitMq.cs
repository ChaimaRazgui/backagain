using Naxxum.WeeCare.Authentification.Application.Abstractions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.Authentification.Infrastructure.Repositories
{
    public class RabbitMq : IRabitMQProducer
    {
        public void SendProductMessage<T>(T message)
        {
            //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            //Create the RabbitMQ connection 
            var connection = factory.CreateConnection();
           //create channel with session and model
            using
            var channel = connection.CreateModel();
            //declare the queue
            channel.QueueDeclare("user", exclusive: false);
            //Serialize the message
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            //put the data on to the product queue
            channel.BasicPublish(exchange:"", routingKey: "user", body: body);
        }

        public void SendUserIdMessage(int UserId)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("UserId", exclusive: false);

            var json = JsonConvert.SerializeObject(UserId);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "UserId", body: body);
        }
    }
}
