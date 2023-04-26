using MediatR;
using Naxxum.WeeCare.UserManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.UserManagement.Application.Commands
{
    public class UpdateUserCommand : IRequest<UsersDetails>
    {
        public int UserId { get; set; }
        public string fullName { get; set; }
        public string Role { get; set; }
    }
}
