using SimpleTodoList.Interfaces;
using SimpleTodoList.Models;
using SimpleTodoList.Repositories;
using System;
using System.Linq;

namespace SimpleTodoList.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public void AddTask(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Judul tidak boleh kosong!");
                return;
            }

            var task = new TodoItem(0, title);
            _repository.Add(task);
            Console.WriteLine($"Task '{title}' ditambahkan.");
        }

        public void CompleteTask(int id)
        {
            var task = _repository.GetAll().FirstOrDefault(t => t.Id == id);
            
            if (task == null)
            {
                Console.WriteLine("Task tidak ditemukan!");
                return;
            }

            if (task.IsCompleted)
            {
                Console.WriteLine("Task sudah selesai!");
                return;
            }

            task.IsCompleted = true;
            _repository.Update(task);
            Console.WriteLine($"Task '{task.Title}' selesai!");
        }

        public void DeleteTask(int id)
        {
            var task = _repository.GetAll().FirstOrDefault(t => t.Id == id);
            
            if (task == null)
            {
                Console.WriteLine("Task tidak ditemukan!");
                return;
            }

            _repository.Delete(id);
            Console.WriteLine($"Task '{task.Title}' dihapus.");
        }

        public void ShowAllTasks()
        {
            var tasks = _repository.GetAll();
            
            if (!tasks.Any())
            {
                Console.WriteLine("Tidak ada task.");
                return;
            }

            Console.WriteLine("\nDaftar Task:");
            Console.WriteLine("ID | Status | Title");
            Console.WriteLine("---|--------|------");
            
            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "[✓]" : "[ ]";
                Console.WriteLine($"{task.Id,-2} | {status,-6} | {task.Title}");
            }
        }
    }
}