using Naxxum.WeeCare.UserManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.UserManagement.Application.Repositories
{
    public interface IUserDetailsRepository
    {
         List<UsersDetails> GetAllUsers();
        UsersDetails UpdateUser(UsersDetails user);
         UsersDetails GetUserById(int Id);
        bool DeleteUser(int Id);
        UsersDetails CreateUser(UsersDetails user);
        public UsersDetails GetUserByEmail(string email);

    }
}
