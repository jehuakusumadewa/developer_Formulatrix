using SimpleTodoList.Interfaces;

namespace SimpleTodoList.Models
{
    public class TodoItem : ITodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        
        public TodoItem(int id, string title, bool iscompleted)
        {
            Id = id;
            Title = title;
            IsCompleted = false;
        }
    }
}