using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.Authentification.Application.DTOs.Users
{
    public class UserCreated
    {
        [Key]
        public int UserId { get; set; }
        public string Email { get; set; }
        public string fullName { get; set; }
    }
}
