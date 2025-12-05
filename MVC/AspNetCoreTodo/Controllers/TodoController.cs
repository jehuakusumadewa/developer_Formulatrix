// Controllers/TodoController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;

[Authorize]
[Route("Todo")] // Base route
public class TodoController : Controller
{
    private readonly ITodoItemService _todoItemService;
    private readonly UserManager<IdentityUser> _userManager;
    
    public TodoController(
        ITodoItemService todoItemService,
        UserManager<IdentityUser> userManager)
    {
        _todoItemService = todoItemService;
        _userManager = userManager;
    }
    
    // GET: Todo/ atau Todo/Index
    [HttpGet("")] // Bisa juga [HttpGet] atau [HttpGet("Index")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();
        
        var items = await _todoItemService
            .GetIncompleteItemsAsync(currentUser);
            
        var model = new TodoViewModel()
        {
            Items = items
        };
        
        return View(model);
    }

    // GET: Todo/Create - Form untuk create baru (opsional)
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(); // Akan render form kosong
    }
    
    
// [HttpPost]
// [ValidateAntiForgeryToken]
// public async Task<IActionResult> AddItem(TodoItem newItem)
// {
//     // Debug: lihat apa yang diterima
//     Console.WriteLine($"Title received: '{newItem?.Title}'");
    
//     // Validasi manual jika perlu
//     if (string.IsNullOrWhiteSpace(newItem?.Title))
//     {
//         ModelState.AddModelError("Title", "Title cannot be empty");
//     }
//     ModelState.Remove("UserId");
    
//     if (!ModelState.IsValid)
//     {
//         // Debug: lihat error di ModelState
//         foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
//         {
//             Console.WriteLine($"Validation error: {error.ErrorMessage}");
//         }
        
//         // Kembali ke Index dengan data yang ada
//         var currentUserNow = await _userManager.GetUserAsync(User);
//         var items = await _todoItemService.GetIncompleteItemsAsync(currentUserNow);
//         var model = new TodoViewModel 
//         { 
//             Items = items 
//         };
        
//         // Simpan error di ViewData
//         ViewData["ErrorMessage"] = "Please enter a title";
//         return View("Index", model);
//     }
    
//     var currentUser = await _userManager.GetUserAsync(User);
//     if (currentUser == null) return Challenge();
    
//     var successful = await _todoItemService.AddItemAsync(newItem, currentUser);
            
//     if (!successful)
//     {
//         return BadRequest("Could not add item.");
//     }
    
//     return RedirectToAction("Index");
// }


    // POST: Todo/ - Create operation
    [HttpPost("Create")] 
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoItem newItem) // Ganti dari AddItem
    {

        // Debug: lihat apa yang diterima
         Console.WriteLine($"Title received: '{newItem?.Title}'");
         ModelState.Remove("UserId");
        if (!ModelState.IsValid)
        {
            // Debug: lihat error di ModelState
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Validation error: {error.ErrorMessage}");
            }
            
            // Kembali ke Index dengan data yang ada
            var currentUserNow = await _userManager.GetUserAsync(User);
            var items = await _todoItemService.GetIncompleteItemsAsync(currentUserNow);
            var model = new TodoViewModel 
            { 
                Items = items 
            };
            
            // Simpan error di ViewData
            ViewData["ErrorMessage"] = "Please enter a title";
            return View("Index", model);
        }
        
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();
        
        var successful = await _todoItemService.AddItemAsync(newItem, currentUser);
        
        if (!successful)
        {
            ModelState.AddModelError("", "Could not add item.");
            return View(newItem);
        }
        
        return RedirectToAction("Index");
    }
    
        // GET: Todo/Edit/{id} - Form untuk edit
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();
        
        var item = await _todoItemService.GetItemByIdAsync(id, currentUser);
        if (item == null) return NotFound();
        
        return View(item);
    }
    
    // POST: Todo/Edit/{id} - Update operation
    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, TodoItem updatedItem)
    {
        if (id != updatedItem.Id) return BadRequest();
        
        ModelState.Remove("UserId");
        if (!ModelState.IsValid)
        {
            return View(updatedItem);
        }
        
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();
        
        var successful = await _todoItemService.UpdateItemAsync(updatedItem, currentUser);
        Console.WriteLine($"Title Update: '{updatedItem?.Title}'");
        if (!successful)
        {
            ModelState.AddModelError("", "Could not update item.");
            return View(updatedItem);
        }
        
        return RedirectToAction("Index");
    }


        // POST: Todo/Delete/{id} - Delete operation
    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();
        
        var successful = await _todoItemService.DeleteItemAsync(id, currentUser);
        
        if (!successful)
        {
            return BadRequest("Could not delete item.");
        }
        
        return RedirectToAction("Index");
    }
    
    // POST: Todo/MarkDone/{id} - Special update operation
    [HttpPost("MarkDone/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkDone(Guid id)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();
        
        var successful = await _todoItemService.MarkDoneAsync(id, currentUser);
        
        if (!successful)
        {
            return BadRequest("Could not mark item as done.");
        }
        
        return RedirectToAction("Index");
    }




    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> MarkDone(Guid id)
    // {
    //     var currentUser = await _userManager.GetUserAsync(User);
    //     if (currentUser == null) return Challenge();
        
    //     var successful = await _todoItemService
    //         .MarkDoneAsync(id, currentUser);
            
    //     if (!successful)
    //     {
    //         return BadRequest("Could not mark item as done.");
    //     }
        
    //     return RedirectToAction("Index");
    // }
}