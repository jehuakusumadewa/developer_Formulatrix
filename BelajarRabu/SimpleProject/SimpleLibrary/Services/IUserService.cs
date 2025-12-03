using SimpleLibrary.Models;
using System.Collections.Generic;
namespace SimpleLibrary.Services
{
    public interface IUserService
    {
        User GetActiveUserById(int id);
        List<User> GetActiveUsers();
        bool RegisterUser(string name, string email);
        bool DeleteUser(int id);
        string GetUserStatus(int id);
    }
}