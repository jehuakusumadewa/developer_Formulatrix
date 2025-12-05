// Models/TodoItem.cs
using System;
using System.ComponentModel.DataAnnotations;

public class TodoItem
{
    public Guid Id { get; set; }
    
    [Required]
    public string UserId { get; set; }
    
    public bool IsDone { get; set; }
    
    [Required(ErrorMessage = "Title is required")] // Optional: tambahkan pesan error
    public string Title { get; set; }
    
    public DateTime? DueAt { get; set; }
}