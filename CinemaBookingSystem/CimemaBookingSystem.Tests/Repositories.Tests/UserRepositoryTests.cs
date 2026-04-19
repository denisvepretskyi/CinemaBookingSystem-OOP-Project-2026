using CimemaBookingSystem.Tests.Repository.Tests;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CimemaBookingSystem.Tests.Repositories.Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private string TestFilePath = $"users_test_db_{Guid.NewGuid()}.json";
        private UserRepository _userRepository;


        [TestInitialize]
        public void Setup()
        {
            if (File.Exists(TestFilePath)) File.Delete(TestFilePath);
            File.WriteAllText(TestFilePath, "[]");
            _userRepository = new UserRepository(TestFilePath);
        }


        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(TestFilePath)) File.Delete(TestFilePath);
        }


        [TestMethod]
        public void GetByPhone_Success_Test()
        {
            // Arrange
            Customer testCustomer = new Customer()
            {                
                Name = "Test User 1",
                PhoneNumber = "0501112233",
                Password = "password123",
                Orders = new List<Order>()
            };
            // Act
            _userRepository.Add(testCustomer);
            User user = _userRepository.GetByPhone("0501112233");            
            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(testCustomer.Name, user.Name);
            Assert.AreEqual(testCustomer.PhoneNumber, user.PhoneNumber);
            Assert.AreEqual(testCustomer.Password, user.Password);
        }

        [TestMethod]
        public void GetByPhone_NotSuccess_Test()
        {
            // Arrange
            Customer testCustomer = new Customer()
            {
                Name = "Test User 1",
                PhoneNumber = "0501112233",
                Password = "password123",
                Orders = new List<Order>()
            };
            // Act
            _userRepository.Add(testCustomer);
            User user = _userRepository.GetByPhone("0501112222");
            // Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetByPhone_EmptyDB_Test()
        {
            // Arrange            
            // Act
            User user = _userRepository.GetByPhone("0501112222");
            // Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void IsPhoneExists_Success_Test()
        {
            // Arrange
            Customer testCustomer = new Customer()
            {
                Name = "Test User 1",
                PhoneNumber = "0501112233",
                Password = "password123",
                Orders = new List<Order>()
            };
            // Act
            _userRepository.Add(testCustomer);
            // Assert
            Assert.IsTrue(_userRepository.IsPhoneExists(testCustomer.PhoneNumber));
        }

        [TestMethod]
        public void IsPhoneExists_NotSuccess_Test()
        {
            // Arrange
            Customer testCustomer = new Customer()
            {
                Name = "Test User 1",
                PhoneNumber = "0501112233",
                Password = "password123",
                Orders = new List<Order>()
            };
            // Act
            _userRepository.Add(testCustomer);
            // Assert
            Assert.IsFalse(_userRepository.IsPhoneExists("0501112222"));
        }

        [TestMethod]
        public void IsPhoneExists_EmptyDB_Test()
        {
            // Arrange
            // Act
            // Assert
            Assert.IsFalse(_userRepository.IsPhoneExists("0501112222"));
        }
    }
}
