using SimpleLibrary.Models;
using System.Collections.Generic;
using System.Linq;
namespace SimpleLibrary.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new()
        {
            new User { Id = 1, Name = "John Doe", Email = "john@example.com", IsActive = true },
            new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com", IsActive = false }
        };

        public User GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public void AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _users.Add(user);
        }

        public bool DeleteUser(int id)
        {
            var user = GetUserById(id);
            if (user == null) return false;

            return _users.Remove(user);
        }

        public User GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }
    }
}