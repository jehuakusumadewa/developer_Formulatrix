// Services/ITodoItemService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AspNetCoreTodo.Models;
public interface ITodoItemService
{
    Task<IEnumerable<TodoItem>> GetIncompleteItemsAsync(IdentityUser user);
    Task<bool> AddItemAsync(TodoItem newItem, IdentityUser user);
    Task<bool> MarkDoneAsync(Guid id, IdentityUser user);
    Task<TodoItem> GetItemByIdAsync(Guid id, IdentityUser user); // Baru
    Task<bool> UpdateItemAsync(TodoItem updatedItem, IdentityUser user); // Baru
    Task<bool> DeleteItemAsync(Guid id, IdentityUser user); // Baru
}