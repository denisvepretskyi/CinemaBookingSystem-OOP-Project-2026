using CimemaBookingSystem.Tests.Repository.Tests;
using CinemaBookingSystem.Core.Repositories;
using CinemaBookingSystem.Core.Services;
using CinemaBookingSystem.Core.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace CimemaBookingSystem.Tests.Services.Tests
{
    [TestClass]
    public class AuthorizationManagerTests
    {

        private string _userFilePath = $"test_users_db_{Guid.NewGuid()}.json";
        private string _orderFilePath = $"test_order_db_{Guid.NewGuid()}.json";
        private UserRepository _userRepo;
        private OrderRepository _orderRepo;
        private AuthorizationManager _authManager;

        [TestInitialize]
        public void Setup()
        {
            File.WriteAllText(_userFilePath, "[]");
            File.WriteAllText(_orderFilePath, "[]");
            _userRepo = new UserRepository(_userFilePath);
            _orderRepo = new OrderRepository(_orderFilePath);
            _authManager = new AuthorizationManager(_userRepo, _orderRepo);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_userFilePath)) File.Delete(_userFilePath);
            if (File.Exists(_orderFilePath)) File.Delete(_orderFilePath);
        }

        [TestMethod]
        public void Registration_NotValid_Phone_Test()
        {
            //Arrange
            //Act
            var result = _authManager.Register("Test User", "invalid_phone", "validPassword123");
            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Message, "Коректний номер має починатися з '+380' або '0'.");
        }

        [TestMethod]
        public void Registration_NotValid_Password_Test()
        {
            //Arrange
            //Act
            var result = _authManager.Register("Test User", "+380954091234", "abcd");
            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Message, "Пароль повинен містити щонайменше 8 символів!");
        }

        [TestMethod]
        public void Registration_PhoneExists_Test()
        {
            //Arrange
            _authManager.Register("Test User 1", "+380954091234", "validPassword123");
            //Act
            var result = _authManager.Register("Test User 2", "+380954091234", "abcd");
            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.Contains("вже існує", result.Message);
        }

        [TestMethod]
        public void Registration_Success_Test()
        {
            //Arrange
            //Act
            var result = _authManager.Register("Test User 1", "+380954091234", "validPassword123");
            //Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void Login_PhoneNotFound_Test()
        {
            //Arrange
            //Act
            var result = _authManager.Login("+380954091234", "validPassword123");
            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Message, "Користувача не знайдено!");
        }

        [TestMethod]
        public void Login_Wrong_Password_Test()
        {
            //Arrange
            _authManager.Register("Test User 1", "+380954091234", "validPassword123");
            _authManager.Logout();
            //Act
            var result = _authManager.Login("+380954091234", "WrongPassword123");
            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Message, "Невірний пароль!");
        }

        [TestMethod]
        public void Login_Success_Test()
        {
            //Arrange
            _authManager.Register("Test User 1", "+380954091234", "validPassword123");
            _authManager.Logout();
            //Act
            var result = _authManager.Login("+380954091234", "validPassword123");
            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(_authManager.currentUser.Name, "Test User 1");
        }

        [TestMethod]
        public void Logout_Test()
        {
            //Arrange
            _authManager.Register("Test User 1", "+380954091234", "validPassword123");
            //Act
            _authManager.Logout();
            //Assert
            Assert.IsNull(_authManager.currentUser);
        }
    }
}
