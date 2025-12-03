using SimpleLibrary.Models;
using SimpleLibrary.Repositories;
using System.Collections.Generic;
using System.Linq;
namespace SimpleLibrary.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetActiveUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            return user?.IsActive == true ? user : null;
        }

        public List<User> GetActiveUsers()
        {
            return _userRepository.GetAllUsers()
                .Where(u => u.IsActive)
                .ToList();
        }

        public bool RegisterUser(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
                return false;

            var existingUser = _userRepository.GetUserByEmail(email);
            if (existingUser != null)
                return false;

            var newUser = new User
            {
                Id = _userRepository.GetAllUsers().Count + 1,
                Name = name,
                Email = email,
                IsActive = true
            };

            _userRepository.AddUser(newUser);
            return true;
        }

        public bool DeleteUser(int id)
        {
            return _userRepository.DeleteUser(id);
        }

        public string GetUserStatus(int id)
        {
            var user = _userRepository.GetUserById(id);
            
            if (user == null)
                return "User not found";
            
            return user.IsActive ? "Active" : "Inactive";
        }
    }
}