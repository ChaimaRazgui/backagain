using Microsoft.EntityFrameworkCore;
using Naxxum.WeeCare.UserManagement.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.UserManagement.Infrastructrue.Data
{
    public class UserManagementDbContext : DbContext
    {
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
            : base(options)
        {
        }
        public DbSet<UsersDetails> UserDetails { get; set; }
    }
}
