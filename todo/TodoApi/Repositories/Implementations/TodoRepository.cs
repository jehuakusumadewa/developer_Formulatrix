using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Helpers;
using TodoApi.Models;
using TodoApi.Repositories.Interfaces;

namespace TodoApi.Repositories.Implementations
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDbContext _context;
        
        public TodoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<IEnumerable<TodoItem>>> GetAllByUserIdAsync(string userId)
        {
            var items = await _context.TodoItems
                .Where(t => t.UserId == userId)
                .ToListAsync();
            return Result<IEnumerable<TodoItem>>.Ok(items);
        }
        
        public async Task<Result<TodoItem>> GetByIdAsync(int id, string userId)
        {
            var item = await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            
            if (item == null)
                return Result<TodoItem>.Failed("Todo item not found");
            
            return Result<TodoItem>.Ok(item);
        }
        
        public async Task<Result<TodoItem>> CreateAsync(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return Result<TodoItem>.Ok(todoItem);
        }
        
        public async Task<Result<TodoItem>> UpdateAsync(TodoItem todoItem)
        {
            _context.TodoItems.Update(todoItem);
            await _context.SaveChangesAsync();
            return Result<TodoItem>.Ok(todoItem);
        }
        
        public async Task<Result<bool>> DeleteAsync(int id, string userId)
        {
            var item = await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            
            if (item == null)
                return Result<bool>.Failed("Todo item not found");
            
            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            return Result<bool>.Ok(true);
        }
    }
}