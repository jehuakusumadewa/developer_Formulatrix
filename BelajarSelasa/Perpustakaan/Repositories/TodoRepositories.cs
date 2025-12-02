using SimpleTodoList.Interfaces;
using SimpleTodoList.Models;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTodoList.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private List<ITodoItem> _items = new List<ITodoItem>();
        private int _nextId = 1;

        public List<ITodoItem> GetAll() => _items;

        public void Add(ITodoItem item)
        {
            item.Id = _nextId++;
            _items.Add(item);
        }

        public void Update(ITodoItem item)
        {
            var existing = GetById(item.Id);
            if (existing != null)
            {
                existing.Title = item.Title;
                existing.IsCompleted = item.IsCompleted;
            }
        }

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
                _items.Remove(item);
        }

        private ITodoItem GetById(int id)
        {
            return _items.FirstOrDefault(i => i.Id == id);
        }
    }
}