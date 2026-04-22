using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using CinemaBookingSystem.Core.Services;

namespace CimemaBookingSystem.Tests.Repositories.Tests
{
    [TestClass]
    public class OrderRepositoryTests
    {
        private string _orderFilePath = $"test_order_db_{Guid.NewGuid()}.json";
        private string _userFilePath = $"test_user_db_{Guid.NewGuid()}.json";
        private string _sessionFilePath = $"test_session_db_{Guid.NewGuid()}.json";
        private string _hallFilePath = $"test_hall_db_{Guid.NewGuid()}.json";

        private IOrderRepository _testOrderRepo;
        private IUserRepository _testUserRepo;
        private IJsonRepository<Session> _testSessionRepo;
        private IJsonRepository<Hall> _testHallRepo;
        private AuthorizationManager _authorizationManager;
        private BookingManager _bookingManager;
                
        [TestInitialize]
        public void Setup()
        {
            File.WriteAllText(_orderFilePath, "[]");
            File.WriteAllText(_userFilePath, "[]");
            File.WriteAllText(_sessionFilePath, "[]");
            File.WriteAllText(_hallFilePath, "[]");
            _testOrderRepo = new OrderRepository(_orderFilePath);
            _testUserRepo = new UserRepository(_userFilePath);
            _testSessionRepo = new JsonRepository<Session>(_sessionFilePath);
            _testHallRepo = new JsonRepository<Hall>(_hallFilePath);
            _authorizationManager = new AuthorizationManager(_testUserRepo, _testOrderRepo);
            _bookingManager = new BookingManager(_testOrderRepo, _testSessionRepo, _testHallRepo, _testUserRepo);

            _testUserRepo.Add(new User
            { 
                Id = 1, 
                Name = "Test User", 
                PhoneNumber = "0954095512",
                Password = "Password123"
            }); 

            _testSessionRepo.Add(new Session
            {
                Id = 1,
                MovieId = 1,
                StartTime = DateTime.Now.AddHours(1),
                HallId = 1,
                CinemaId = 1
            });

            _testHallRepo.Add(new Hall
            {
                Id = 1,
                CinemaId = 1,
                Name = "Test Hall",
                RowCount = 5,
                ColumnCount = 5
            });

        }
        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_orderFilePath)) File.Delete(_orderFilePath);
            if (File.Exists(_userFilePath)) File.Delete(_userFilePath);
        }



        [TestMethod]
        public void GetByUserId_NotEmptyList_Test()
        {
            // Arrange
            _bookingManager.CreateOrder(1, 1, new List<(short, short)> { (1, 1), (1, 2) });
            _authorizationManager.Login("0954095512", "Password123");
            //Act
            var result = _testOrderRepo.GetByUserId(1);
            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result[0].Tickets.Count);
        }


        [TestMethod]
        public void GetByUserId_EmptyList_Test()
        {
            // Arrange
            //Act
            var result = _testOrderRepo.GetByUserId(1);
            //Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}
