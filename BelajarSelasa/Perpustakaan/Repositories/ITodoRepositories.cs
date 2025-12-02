using SimpleTodoList.Interfaces;
using System.Collections.Generic;

namespace SimpleTodoList.Repositories
{
    public interface ITodoRepository
    {
        List<ITodoItem> GetAll();
        void Add(ITodoItem item);
        void Update(ITodoItem item);
        void Delete(int id);
    }
}