using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.Services;
using Microsoft.EntityFrameworkCore;

namespace Oline_Ride_Share_idb_project.Server.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseDbContext _context; 

        public UserService(DatabaseDbContext context) 
        {
            _context = context;
        }
        public UserModel GetUserByPhoneNumber(string phoneNumber)
        {
            return _context.UserModels
                           .FirstOrDefault(user => user.PhoneNumber == phoneNumber);
        }
        public void AddUser(UserModel user)
        {
            _context.UserModels.Add(user); 
            _context.SaveChanges();
        }
    }
}
