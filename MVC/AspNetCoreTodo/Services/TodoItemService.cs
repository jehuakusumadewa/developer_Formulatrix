using	System;
using	System.Collections.Generic;
using	System.Linq;
using	System.Threading.Tasks;
using	AspNetCoreTodo.Data;
using	AspNetCoreTodo.Models;
using	Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Services
{
    public class TodoItemService: ITodoItemService
    {
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoItem>> GetIncompleteItemsAsync(IdentityUser user)
        {
            return await _context.Items
                .Where(x => x.IsDone == false && x.UserId == user.Id)
                .ToArrayAsync();
        }

        public async Task<bool> AddItemAsync(TodoItem newItem, IdentityUser user)
        {
            newItem.Id = Guid.NewGuid();
            newItem.IsDone = false;
            newItem.UserId = user.Id;
            newItem.DueAt = DateTime.Now.AddDays(3);

            Console.WriteLine($"Adding todo: {newItem.Title} for user: {user.Id}");
            _context.Items.Add(newItem);

            
            var saveResult = await _context.SaveChangesAsync();
            Console.WriteLine($"Save result: {saveResult}");
            
            return saveResult == 1;
        }

        public async Task<bool> MarkDoneAsync(Guid id, IdentityUser user)
        {
            var item = await _context.Items
                .Where(x => x.Id == id && x.UserId == user.Id) // Filter by user
                .SingleOrDefaultAsync();
                
            if (item == null) return false;
            
            item.IsDone = true;
            
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1; // One entity should have been updated
        }

        public async Task<TodoItem> GetItemByIdAsync(Guid id, IdentityUser user)
        {
            return await _context.Items
                .Where(x => x.Id == id && x.UserId == user.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateItemAsync(TodoItem updatedItem, IdentityUser user)
        {
            var item = await _context.Items
                .Where(x => x.Id == updatedItem.Id && x.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (item == null) return false;

            item.Title = updatedItem.Title;
            item.DueAt = updatedItem.DueAt;
            // Update field lainnya sesuai kebutuhan

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> DeleteItemAsync(Guid id, IdentityUser user)
        {
            var item = await _context.Items
                .Where(x => x.Id == id && x.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (item == null) return false;

            _context.Items.Remove(item);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }



    }


}