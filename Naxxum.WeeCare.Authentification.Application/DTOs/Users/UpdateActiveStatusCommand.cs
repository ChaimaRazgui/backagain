using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.Authentification.Application.DTOs.Users
{
    public class UpdateActiveStatusCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public bool Active { get; set; }
    }

}
