using Microsoft.EntityFrameworkCore;

namespace SOPBackend.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _context;

        public UserService(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList(); 
        }

        public User? GetUserById(Guid id)
        {
            return _context.Users.Find(id);
        }

        public User? CreateUser(User newUser)
        {
            var existingUser = _context.Users.Find(newUser.Id);
            if (existingUser != null)
            {
                Console.WriteLine("User already exists!");
                return null;
            }

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }

        public User UpdateUser(Guid id, User updatedUser)
        {
            var existingUser = _context.Users.Find(id);
            if (existingUser == null)
            {
                throw new Exception("User not found!"); 
            }
            
            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;
            existingUser.DestinationAddress = updatedUser.DestinationAddress;

            _context.Users.Update(existingUser);
            _context.SaveChanges();
            return existingUser;
        }

        public bool DeleteUser(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}


