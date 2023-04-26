using Naxxum.WeeCare.UserManagement.Application.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.UserManagement.Infrastructrue.Repositories
{
    public class RabbitMqProducerD : IRabitMQProducerD
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
            channel.QueueDeclare("userdeleted", exclusive: false);
            //Serialize the message
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            //put the data on to the product queue
            channel.BasicPublish(exchange: "", routingKey: "userdeleted", body: body);
        }
        public void SendTokenDetails(UserWithRoleAndNameDto userWithRoleAndName)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

             var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();

            channel.QueueDeclare("TokenDetails", exclusive: false);

            var json = JsonConvert.SerializeObject(userWithRoleAndName);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "TokenDetails", body: body);
        }
    }
}
