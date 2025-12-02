using SimpleTodoList.Repositories;
using SimpleTodoList.Services;

namespace SimpleTodoList
{
    class Program
    {
        static void Main()
        {
            // Setup dependencies
            var repository = new TodoRepository();
            var todoService = new TodoService(repository);
            
            bool exit = false;
            
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== TODO LIST ===");
                Console.WriteLine("1. Lihat semua task");
                Console.WriteLine("2. Tambah task");
                Console.WriteLine("3. Tandai selesai");
                Console.WriteLine("4. Hapus task");
                Console.WriteLine("5. Keluar");
                Console.Write("Pilih: ");
                
                var choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        todoService.ShowAllTasks();
                        break;
                    case "2":
                        Console.Write("Judul task: ");
                        var title = Console.ReadLine();
                        todoService.AddTask(title);
                        break;
                    case "3":
                        todoService.ShowAllTasks();
                        Console.Write("ID task yang selesai: ");
                        if (int.TryParse(Console.ReadLine(), out int completeId))
                            todoService.CompleteTask(completeId);
                        break;
                    case "4":
                        todoService.ShowAllTasks();
                        Console.Write("ID task yang dihapus: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteId))
                            todoService.DeleteTask(deleteId);
                        break;
                    case "5":
                        exit = true;
                        Console.WriteLine("Terima kasih!");
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak valid!");
                        break;
                }
                
                if (!exit)
                {
                    Console.Write("\nTekan Enter untuk melanjutkan...");
                    Console.ReadLine();
                }
            }
        }
    }
}