// Program.cs
using System;
using System.Collections.Generic;

namespace TodoApp
{
    class Program
    {
        static List<TodoItem> todos = new List<TodoItem>();
        
        static void Main(string[] args)
        {
            Console.WriteLine("Todo App");
            
            while (true)
            {
                Console.WriteLine("\n1. Add Todo");
                Console.WriteLine("2. Complete Todo");
                Console.WriteLine("3. List Todos");
                Console.WriteLine("4. Exit");
                
                var choice = Console.ReadLine();
                
                if (choice == "1")
                {
                    Console.Write("Title: ");
                    var title = Console.ReadLine();
                    Console.Write("Description: ");
                    var desc = Console.ReadLine();
                    
                    todos.Add(new TodoItem 
                    { 
                        Id = todos.Count + 1, 
                        Title = title, 
                        Description = desc 
                    });
                }
                else if (choice == "2")
                {
                    Console.Write("Todo ID to complete: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var todo = todos.Find(t => t.Id == id);
                        if (todo != null)
                        {
                            todo.IsCompleted = true;
                            Console.WriteLine($"Todo {id} completed!");
                        }
                    }
                }
                else if (choice == "3")
                {
                    foreach (var todo in todos)
                    {
                        Console.WriteLine($"[{todo.Id}] {todo.Title} - {(todo.IsCompleted ? "✓" : "✗")}");
                    }
                }
                else if (choice == "4")
                {
                    break;
                }
            }
        }
    }
    
    class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}