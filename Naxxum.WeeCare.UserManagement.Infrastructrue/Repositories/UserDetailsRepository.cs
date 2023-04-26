using Naxxum.WeeCare.UserManagement.Application.Repositories;
using Naxxum.WeeCare.UserManagement.Domain.Entites;
using Naxxum.WeeCare.UserManagement.Infrastructrue.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naxxum.WeeCare.UserManagement.Infrastructrue.Repositories
{
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly UserManagementDbContext _userManagementDb;
        public UserDetailsRepository(UserManagementDbContext userManagementDb) 
        
        {
            _userManagementDb = userManagementDb;
        }
        public UsersDetails CreateUser(UsersDetails user)
        {
            var userDetails = new UsersDetails
            {
                UserId = user.UserId,
                Email = user.Email,
                fullName = user.fullName, 
                Role = "" 
            };

            _userManagementDb.UserDetails.Add(userDetails);
            _userManagementDb.SaveChanges();

            return userDetails;
        }
        public List<UsersDetails> GetAllUsers()
        {
            return _userManagementDb.UserDetails.ToList();
        }
        public UsersDetails UpdateUser(UsersDetails user)
        {
            var result = _userManagementDb.Update(user);
            _userManagementDb.SaveChanges();
            return result.Entity;
        }
        public UsersDetails GetUserById(int Id)
        {
            return _userManagementDb.UserDetails.Where(x => x.UserId == Id).FirstOrDefault();
        }
        public bool DeleteUser(int Id)
        {
            var filteredData = _userManagementDb.UserDetails.Where(x => x.UserId == Id).FirstOrDefault();
            var result = _userManagementDb.Remove(filteredData);
            _userManagementDb.SaveChanges();
            return result != null ? true : false;
        }
        public UsersDetails GetUserByEmail(string email)
        {
            var user =  _userManagementDb.UserDetails.FirstOrDefault(u => u.Email == email);
            return user;
        }

    }
}











