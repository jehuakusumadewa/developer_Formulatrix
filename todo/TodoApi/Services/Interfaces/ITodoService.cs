using TodoApi.DTOs.Requests;
using TodoApi.DTOs.Responses;
using TodoApi.Helpers;

namespace TodoApi.Services.Interfaces
{
    public interface ITodoService
    {
        Task<Result<IEnumerable<TodoResponse>>> GetUserTodosAsync(string userId);
        Task<Result<TodoResponse>> GetTodoByIdAsync(int id, string userId);
        Task<Result<TodoResponse>> CreateTodoAsync(TodoRequest request, string userId);
        Task<Result<TodoResponse>> UpdateTodoAsync(int id, TodoRequest request, string userId);
        Task<Result<bool>> DeleteTodoAsync(int id, string userId);
        Task<Result<bool>> MarkCompletionAsync(int id, string userId);
    }
}