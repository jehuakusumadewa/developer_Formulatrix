namespace SimpleTodoList.Services
{
    public interface ITodoService
    {
        void AddTask(string title);
        void CompleteTask(int id);
        void DeleteTask(int id);
        void ShowAllTasks();
    }
}