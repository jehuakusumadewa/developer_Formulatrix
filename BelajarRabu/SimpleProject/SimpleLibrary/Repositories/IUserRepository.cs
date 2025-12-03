using SimpleLibrary.Models;
using System.Collections.Generic;
namespace SimpleLibrary.Repositories
{
    public interface IUserRepository
    {
        User GetUserById(int id);
        List<User> GetAllUsers();
        void AddUser(User user);
        bool DeleteUser(int id);
        User GetUserByEmail(string email);
    }
}