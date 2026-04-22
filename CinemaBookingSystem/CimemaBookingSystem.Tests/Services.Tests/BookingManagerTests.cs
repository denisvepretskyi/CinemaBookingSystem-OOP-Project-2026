using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Repositories;
using CinemaBookingSystem.Core.Services;
using CinemaBookingSystem.Core.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace CimemaBookingSystem.Tests.Services.Tests
{
    [TestClass]
    public class BookingManagerTests
    {
        private string _orderFilePath = $"test_order_db_{Guid.NewGuid()}.json";
        private string _sessionFilePath = $"test_session_db_{Guid.NewGuid()}.json";
        private string _hallFilePath = $"test_hall_db_{Guid.NewGuid()}.json";
        private string _userFilePath = $"test_user_db_{Guid.NewGuid()}.json";

        private IOrderRepository _testOrderRepo;
        private IJsonRepository<Session> _testSessionRepo;
        private IJsonRepository<Hall> _testHallRepo;
        private IUserRepository _testUserRepo;
        private BookingManager _bookingManager;


        [TestInitialize]
        public void Setup()
        {
            File.WriteAllText(_orderFilePath, "[]");
            File.WriteAllText(_sessionFilePath, "[]");
            File.WriteAllText(_hallFilePath, "[]");
            File.WriteAllText(_userFilePath, "[]");

            _testOrderRepo = new OrderRepository(_orderFilePath);
            _testSessionRepo = new JsonRepository<Session>(_sessionFilePath);
            _testHallRepo = new JsonRepository<Hall>(_hallFilePath);
            _testUserRepo = new UserRepository(_userFilePath);
            _bookingManager = new BookingManager(_testOrderRepo, _testSessionRepo, _testHallRepo, _testUserRepo);

            Session testSession = new Session()
            {
                Id = 1,
                CinemaId = 1,
                HallId = 1,
                MovieId = 1,
                StartTime = new DateTime(2027, 3, 12, 14, 0, 0),
                Price = 250
            };
            _testSessionRepo.Add(testSession);
            Hall testHall = new Hall()
            {
                Id = 1,
                RowCount = 10,
                ColumnCount = 10
            };
            _testHallRepo.Add(testHall);
            User testUser = new User()
            {
                Id = 1,
                Name = "Test User",
                PhoneNumber = "0954095522",
                Password = "123pAssword"
            };
            _testUserRepo.Add(testUser);
        }

        [TestCleanup]
        public void CleanUp()
        {
            if (File.Exists(_orderFilePath)) File.Delete(_orderFilePath);
            if (File.Exists(_sessionFilePath)) File.Delete(_sessionFilePath);
            if (File.Exists(_hallFilePath)) File.Delete(_hallFilePath);
        }

        [TestMethod]
        public void CreateOrder_SessionNotFoud_Test()
        {
            //Arrange
            //Act
            var result = _bookingManager.CreateOrder(1, 999, new List<(short, short)> { (1, 1), (1, 2) });
            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Сеанс не знайдено!", result.Message);
        }

        [TestMethod]
        public void CreateOrder_UserNotFoud_Test()
        {
            //Arrange
            //Act
            var result = _bookingManager.CreateOrder(4, 1, new List<(short, short)> { (1, 1), (1, 2) });
            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Користувача не знайдено!", result.Message);
        }

        [TestMethod]
        public void CreateOrder_PastDate_Test()
        {
            //Arrange
            Session pastSession = new Session()
            {
                Id = 2,
                CinemaId = 1,
                HallId = 1,
                MovieId = 1,
                StartTime = new DateTime(2020, 3, 12, 14, 0, 0),
                Price = 250
            };
            _testSessionRepo.Add(pastSession);
            //Act
            var result = _bookingManager.CreateOrder(1, 2, new List<(short, short)> { (1, 1), (1, 2) });
            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Неможливо забронювати квитки на сеанс у минулому!", result.Message);
        }


        [TestMethod]
        public void CreateOrder_TakenSeats_Test()
        {
            //Arrange
            _bookingManager.CreateOrder(1, 1, new List<(short, short)> { (1, 1), (1, 2) });
            //Act
            var result = _bookingManager.CreateOrder(1, 1, new List<(short, short)> { (1, 1) });
            //Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void CreateOrder_SeatNotExists_Test()
        {
            //Arrange
            //Act
            var result = _bookingManager.CreateOrder(1, 1, new List<(short, short)> { (11, 11) });
            //Assert
            Assert.IsFalse(result.IsSuccess);
        }


        [TestMethod]
        public void CreateOrder_OverflowSeats_Test()
        {
            //Arrange
            Session testSession = new Session()
            {
                Id = 2,
                CinemaId = 2,
                HallId = 2,
                MovieId = 2,
                StartTime = new DateTime(2027, 3, 12, 14, 0, 0),
                Price = 250
            };
            _testSessionRepo.Add(testSession);
            Hall testHall = new Hall()
            {
                Id = 2,
                RowCount = 1,
                ColumnCount = 1
            };
            _testHallRepo.Add(testHall);
            //Act
            var result = _bookingManager.CreateOrder(1, 2, new List<(short, short)> { (11, 1), (1, 2) });
            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Не можна купити квитків більше ніж місць у залі!", result.Message);
        }

        [TestMethod]
        public void CreateOrder_Success_Test()
        {
            //Arrange
            
            //Act
            var result = _bookingManager.CreateOrder(1, 1, new List<(short, short)> { (1, 1), (1, 2) });
            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(_testOrderRepo.GetAll().ToList().Count, 1);
            Assert.AreEqual(_testOrderRepo.GetById(1).Tickets.Count, 2);
        }


        [TestMethod]
        public void ReturnTicket_30minutes_before_start_test()
        {
            //Arrange
            Session testSession = new Session()
            {
                StartTime = DateTime.Now.AddMinutes(20),
                Id = 3,
                CinemaId = 3,
                HallId = 3,
                MovieId = 3,
                Price = 250
            };
            _testSessionRepo.Add(testSession);
            _bookingManager.CreateOrder(1, 3, new List<(short, short)> { (1, 1), (1, 2) });
            //Act
            var result = _bookingManager.ReturnTicket(1, 1, 1);
            //Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void ReturnTicket_Success_Test()
        {
            //Arrange         
            _bookingManager.CreateOrder(1, 1, new List<(short, short)> { (1, 1), (1, 2) });
            //Act
            var result = _bookingManager.ReturnTicket(1, 1, 1);
            //Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void GetTakenSeats_Test()
        {
            //Arrange
            _bookingManager.CreateOrder(1, 1, new List<(short, short)> { (1, 1), (1, 2) });
            //Act
            var result = _bookingManager.GetTakenSeats(1);
            //Assert
            Assert.AreEqual(result.Count, 2);
            Assert.IsTrue(result.Contains((1, 1)));
            Assert.IsTrue(result.Contains((1, 2)));
        }
    }
}
