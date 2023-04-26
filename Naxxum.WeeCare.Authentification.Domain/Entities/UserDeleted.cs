using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.Authentification.Domain.Entities
{
    public class UserDeleted
    {
        [Key]
        public int UserId { get; set; }
    }
}
