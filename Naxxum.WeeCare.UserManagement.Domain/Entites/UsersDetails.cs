using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.UserManagement.Domain.Entites
{
    public class UsersDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public string Email { get; set; }  
        public string fullName { get; set; }
        public string Role { get; set; }

    }
}
