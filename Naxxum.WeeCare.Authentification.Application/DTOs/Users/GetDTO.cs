using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.Authentification.Application.DTOs.Users
{
    public class GetDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }

    }
}
