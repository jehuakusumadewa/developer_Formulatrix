using NUnit.Framework;
using SimpleTodoList.Interfaces;
using SimpleTodoList.Models;
using SimpleTodoList.Services;
using System;
using System.Collections.Generic;

namespace SimpleTodoList.Tests.Services
{
    [TestFixture]
    public class TodoServiceTests
    {
        private ITodoService _todoService;
        private FakeTodoRepository _repository;

        [SetUp]
        public void Setup()
        {
            _repository = new FakeTodoRepository();
            _todoService = new TodoService(_repository);
        }

        [Test]
        public void AddTask_ValidTitle_ShouldAddTask()
        {
            // Arrange
            var title = "New Task";

            // Act
            _todoService.AddTask(title);

            // Assert
            Assert.That(_repository.Tasks, Has.Count.EqualTo(1));
            Assert.That(_repository.Tasks[0].Title, Is.EqualTo(title));
            Assert.That(_repository.Tasks[0].IsCompleted, Is.False);
        }

        [Test]
        public void AddTask_EmptyTitle_ShouldThrowException()
        {
            // Arrange
            var title = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _todoService.AddTask(title));
        }

        [Test]
        public void CompleteTask_ValidId_ShouldMarkAsCompleted()
        {
            // Arrange
            var task = new TodoItem(1, "Task 1", false);
            _repository.Tasks.Add(task);

            // Act
            _todoService.CompleteTask(1);

            // Assert
            Assert.That(task.IsCompleted, Is.True);
        }

        [Test]
        public void CompleteTask_NonExistingId_ShouldThrowException()
        {
            // Act & Assert
            var ex = Assert.Throws<KeyNotFoundException>(() => _todoService.CompleteTask(999));
            Assert.That(ex.Message, Contains.Substring("not found"));
        }

        [Test]
        public void CompleteTask_AlreadyCompleted_ShouldThrowException()
        {
            // Arrange
            var task = new TodoItem(1, "Task 1", true);
            _repository.Tasks.Add(task);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _todoService.CompleteTask(1));
            Assert.That(ex.Message, Contains.Substring("already completed"));
        }

        [Test]
        public void DeleteTask_ValidId_ShouldRemoveTask()
        {
            // Arrange
            var task = new TodoItem(1, "Task to delete", false);
            _repository.Tasks.Add(task);

            // Act
            _todoService.DeleteTask(1);

            // Assert
            Assert.That(_repository.Tasks, Is.Empty);
        }

        [Test]
        public void GetAllTasks_ShouldReturnAllTasks()
        {
            // Arrange
            _repository.Tasks.Add(new TodoItem(1, "Task 1", false));
            _repository.Tasks.Add(new TodoItem(2, "Task 2", true));

            // Act
            var result = _todoService.GetAllTasks();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetCompletedTasks_ShouldReturnOnlyCompleted()
        {
            // Arrange
            _repository.Tasks.Add(new TodoItem(1, "Task 1", true));
            _repository.Tasks.Add(new TodoItem(2, "Task 2", false));
            _repository.Tasks.Add(new TodoItem(3, "Task 3", true));

            // Act
            var result = _todoService.GetCompletedTasks();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].Id, Is.EqualTo(1));
            Assert.That(result[1].Id, Is.EqualTo(3));
        }

        [Test]
        public void GetPendingTasks_ShouldReturnOnlyPending()
        {
            // Arrange
            _repository.Tasks.Add(new TodoItem(1, "Task 1", true));
            _repository.Tasks.Add(new TodoItem(2, "Task 2", false));
            _repository.Tasks.Add(new TodoItem(3, "Task 3", false));

            // Act
            var result = _todoService.GetPendingTasks();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].Id, Is.EqualTo(2));
            Assert.That(result[1].Id, Is.EqualTo(3));
        }

        // Fake Repository untuk testing
        private class FakeTodoRepository : ITodoRepository
        {
            public List<ITodoItem> Tasks { get; } = new List<ITodoItem>();

            public List<ITodoItem> GetAll() => new List<ITodoItem>(Tasks);

            public ITodoItem? GetById(int id) => Tasks.Find(t => t.Id == id);

            public void Add(ITodoItem item)
            {
                if (item.Id == 0)
                    item.Id = Tasks.Count + 1;
                Tasks.Add(item);
            }

            public void Update(ITodoItem item)
            {
                var index = Tasks.FindIndex(t => t.Id == item.Id);
                if (index != -1)
                    Tasks[index] = item;
            }

            public void Delete(int id)
            {
                var item = GetById(id);
                if (item != null)
                    Tasks.Remove(item);
            }
        }
    }
}