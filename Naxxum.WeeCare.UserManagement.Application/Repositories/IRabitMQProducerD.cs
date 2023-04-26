using Naxxum.WeeCare.UserManagement.Infrastructrue.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.UserManagement.Application.Repositories
{
   public interface IRabitMQProducerD
    {
        public void SendProductMessage<T>(T message);
        public void SendTokenDetails(UserWithRoleAndNameDto userWithRoleAndName);
    }
}
