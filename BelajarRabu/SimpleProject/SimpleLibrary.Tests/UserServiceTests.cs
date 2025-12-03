using Moq;
using SimpleLibrary.Repositories;
using SimpleLibrary.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using SimpleLibrary.Models;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

namespace SimpleLibrary.Tests
{
    [TestFixture] // Ini wajib di NUnit untuk menandai class test
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.object);


        }

        [Test]
        public void GetActiveUserById_WhenUserIsActive_ReturnUser()
        {
            var activeUser = new User
            {
                Id = 1,
                Name = "John",
                Email = "john@example.com",
                IsActive = true
            };

            _mockUserRepository
            .Setup(repo => repo.GetActiveUserById(1))
            .Returns(activeUser);

            var result = _userService.GetActiveUserById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(1));
            Assert.That(result.IsActive, Is.True);
        }
        [Test]
        public void GetActiveUserById_WhenUserIsNotActive_ReturnsNull()
        {
            var NotactiveUser = new User
            {
                Id = 2,
                Name = "Je",
                Email = "je@gmail.com",
                IsActive = false
            };

            _mockUserRepository.Setup(repo => repo.GetActiveUserById(2))
            .Returns(NotactiveUser);


            var result = _userService.GetActiveUserById(2);

            Assert.That(result, Is.Null);
        }

        [Test] // Test case untuk user tidak ditemukan
        public void GetActiveUserById_WhenUserNotFound_ReturnsNull()
        {
            _mockUserRepository.Setup(repo => repo.GetUserById(999))
            .Returns((User)null);

            var result = _userService.GetActiveUserById(999)

            Assert.That(result, Is.Null);

        }

        [Test]
        public void GetActiveUsers_ReturnOnlyActiveUsers()
        {
            var mixedUsers = new List<User>
            {
                new User {Id = 1, Name = "Active User 1", IsActive = true},
                new User {Id = 2, Name = "Inactive User", IsActive = false},
                new User {Id = 3, Name = "Active User 2", IsActive = true}
            };

            _mockUserRepository.Setup(repo => repo.GetAllUsers())
            .Returns(mixedUsers);

            var result = _userService.GetActiveUsers();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Has.All.Property("IsActive").True);
        
            _mockUserRepository.Verify(repo => repo.GetAllUsers(), TimestampAttribute.Once);
        }

    }
}