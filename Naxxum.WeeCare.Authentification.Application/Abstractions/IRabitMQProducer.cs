using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.Authentification.Application.Abstractions
{
    public interface IRabitMQProducer
    {
        public void SendProductMessage<T>(T message);
        void SendUserIdMessage(int userId);
    }
}
