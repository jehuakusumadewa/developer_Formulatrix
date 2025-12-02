namespace SimpleTodoList.Interfaces
{
    public interface ITodoItem
    {
        int Id { get; set; }
        string Title { get; set; }
        bool IsCompleted { get; set; }
    }
}