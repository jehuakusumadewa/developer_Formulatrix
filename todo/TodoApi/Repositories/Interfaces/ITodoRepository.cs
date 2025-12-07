using TodoApi.Helpers;
using TodoApi.Models;

namespace TodoApi.Repositories.Interfaces
{
    public interface ITodoRepository
    {
        Task<Result<IEnumerable<TodoItem>>> GetAllByUserIdAsync(string userId);
        Task<Result<TodoItem>> GetByIdAsync(int id, string userId);
        Task<Result<TodoItem>> CreateAsync(TodoItem todoItem);
        Task<Result<TodoItem>> UpdateAsync(TodoItem todoItem);
        Task<Result<bool>> DeleteAsync(int id, string userId);
    }
}