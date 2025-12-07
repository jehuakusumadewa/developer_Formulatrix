using AutoMapper;
using TodoApi.DTOs.Requests;
using TodoApi.DTOs.Responses;
using TodoApi.Helpers;
using TodoApi.Models;
using TodoApi.Repositories.Interfaces;
using TodoApi.Services.Interfaces;

namespace TodoApi.Services.Implementations
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;
        
        public TodoService(ITodoRepository todoRepository, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
        }
        
        public async Task<Result<IEnumerable<TodoResponse>>> GetUserTodosAsync(string userId)
        {
            var result = await _todoRepository.GetAllByUserIdAsync(userId);
            
            if (!result.Success)
                return Result<IEnumerable<TodoResponse>>.Failed(result.Error);
            
            var responses = _mapper.Map<IEnumerable<TodoResponse>>(result.Value);
            return Result<IEnumerable<TodoResponse>>.Ok(responses);
        }
        
        public async Task<Result<TodoResponse>> GetTodoByIdAsync(int id, string userId)
        {
            var result = await _todoRepository.GetByIdAsync(id, userId);
            
            if (!result.Success)
                return Result<TodoResponse>.Failed(result.Error);
            
            var response = _mapper.Map<TodoResponse>(result.Value);
            return Result<TodoResponse>.Ok(response);
        }
        
        public async Task<Result<TodoResponse>> CreateTodoAsync(TodoRequest request, string userId)
        {
            var todoItem = _mapper.Map<TodoItem>(request);
            todoItem.UserId = userId;
            
            var result = await _todoRepository.CreateAsync(todoItem);
            
            if (!result.Success)
                return Result<TodoResponse>.Failed(result.Error);
            
            var response = _mapper.Map<TodoResponse>(result.Value);
            return Result<TodoResponse>.Ok(response);
        }
        
        public async Task<Result<TodoResponse>> UpdateTodoAsync(int id, TodoRequest request, string userId)
        {
            var existingResult = await _todoRepository.GetByIdAsync(id, userId);
            
            if (!existingResult.Success)
                return Result<TodoResponse>.Failed(existingResult.Error);
            
            var todoItem = existingResult.Value;
            _mapper.Map(request, todoItem);
            
            var updateResult = await _todoRepository.UpdateAsync(todoItem);
            
            if (!updateResult.Success)
                return Result<TodoResponse>.Failed(updateResult.Error);
            
            var response = _mapper.Map<TodoResponse>(updateResult.Value);
            return Result<TodoResponse>.Ok(response);
        }
        
        public async Task<Result<bool>> DeleteTodoAsync(int id, string userId)
        {
            return await _todoRepository.DeleteAsync(id, userId);
        }
        
        public async Task<Result<bool>> MarkCompletionAsync(int id, string userId)
        {
            var result = await _todoRepository.GetByIdAsync(id, userId);
            
            if (!result.Success)
                return Result<bool>.Failed(result.Error);
            
            var todoItem = result.Value;
            todoItem.IsCompleted = !todoItem.IsCompleted;
            
            var updateResult = await _todoRepository.UpdateAsync(todoItem);
            return updateResult.Success 
                ? Result<bool>.Ok(true) 
                : Result<bool>.Failed(updateResult.Error);
        }
    }
}