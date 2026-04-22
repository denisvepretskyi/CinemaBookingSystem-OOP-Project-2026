using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Repositories;
using CinemaBookingSystem.Core.Services;
using CinemaBookingSystem.Core.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace CimemaBookingSystem.Tests.Validators.Tests
{
    [TestClass]
    public class SessionValidatorTests
    {
        private string _movieFilePath = $"test_movie_db_{Guid.NewGuid()}.json";
        private string _sessionFilePath = $"test_session_db_{Guid.NewGuid()}.json";
        private string _orderFilePath = $"test_order_db_{Guid.NewGuid()}.json";
        private string _hallFilePath = $"test_hall_db_{Guid.NewGuid()}.json";
        private string _userFilePath = $"test_user_db_{Guid.NewGuid()}.json";

        private  IJsonRepository<Movie> _testMovieRepo;
        private  IJsonRepository<Session> _testSessionRepo;
        private  IOrderRepository _testOrderRepo;
        private IJsonRepository<Hall> _testHallRepo;
        private IUserRepository _testUserRepo;
        private SessionValidator _testSessionValidator;
        private BookingManager _bookingManager;

        [TestInitialize]
        public void Setup()
        {
            File.WriteAllText(_movieFilePath, "[]");
            File.WriteAllText(_sessionFilePath, "[]");
            File.WriteAllText(_orderFilePath, "[]");
            File.WriteAllText(_hallFilePath, "[]");
            File.WriteAllText(_userFilePath, "[]");

            _testMovieRepo = new JsonRepository<Movie>(_movieFilePath);
            _testSessionRepo = new JsonRepository<Session>(_sessionFilePath);
            _testOrderRepo = new OrderRepository(_orderFilePath);
            _testHallRepo = new JsonRepository<Hall>(_hallFilePath);
            _testUserRepo = new UserRepository(_userFilePath);
            _testSessionValidator = new SessionValidator(_testMovieRepo , _testSessionRepo, _testOrderRepo);
            _bookingManager = new BookingManager(_testOrderRepo, _testSessionRepo, _testHallRepo, _testUserRepo);

            _testMovieRepo.Add(new()
            {
                Id = 1,
                Title = "Тестовий фільм",
                Duration = 120
            });
            _testMovieRepo.Add(new()
            {
                Id = 2,
                Title = "Тестовий фільм 2",
                Duration = 120
            });

            _testSessionRepo.Add(new()
            {
                Id = 1,
                CinemaId = 1,
                HallId = 1,
                MovieId = 1,
                StartTime = new DateTime(2026, 7, 10, 14, 0, 0),
                Price = 250
            });
            _testUserRepo.Add(new()
            {
                Id = 1,
                Name = "Тестовий користувач",
                PhoneNumber = "0954095522",
                Password = "passwA1ord",
            });
            _testHallRepo.Add(new()
            {
                Id = 1,
                CinemaId = 1,
                Name = "Зал 1",
                RowCount = 10,
                ColumnCount = 10
            });
        }

        [TestCleanup]
        public void CleanUp()
        {
            if(File.Exists(_movieFilePath)) File.Delete(_movieFilePath);
            if (File.Exists(_sessionFilePath)) File.Delete(_sessionFilePath);
            if (File.Exists(_orderFilePath)) File.Delete(_orderFilePath);
        }


        [DataTestMethod]
        [DataRow("2025-07-10T18:00:00", 250)]
        [DataRow("2026-07-10T18:00:00", -250)]
        [DataRow("2026-07-10T15:00:00", 250)]
        public void IsValidToAdd_NotValidData_Test(string dateString, int price)
        {
            //Arrange
            DateTime testDate = DateTime.Parse(dateString);
            //Act
            var result = _testSessionValidator.IsValidToAdd(1, 1, 1, testDate, price);
            var sessions = _testSessionRepo.GetAll().ToList();
            //Assert
            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public void IsValidToAdd_ValidData_Test()
        {
            //Arrange
            _testSessionRepo.Add(new Session()
            {
                HallId = 1,
                CinemaId = 1,
                MovieId = 1,
                StartTime = new DateTime(2026, 7, 10, 14, 0, 0),
                Price = 250
            });
            DateTime testDate = new DateTime(2026, 7, 10, 18, 0, 0);
            //Act
            var result = _testSessionValidator.IsValidToAdd(1, 1, 1, testDate, 250);
            //Assert
            Assert.IsTrue(result.IsValid);
        }


        [DataTestMethod]
        [DataRow("2025-07-10T18:00:00", 250)]
        [DataRow("2026-07-10T18:00:00", -250)]
        [DataRow("2026-07-10T20:00:00", 250)]
        public void IsValidToEdit_NotValidData_Test(string dateString, int price)
        {
            //Arrange
            _testSessionRepo.Add(new()
            {
                Id = 2,
                CinemaId = 1,
                HallId = 1,
                MovieId = 2,
                StartTime = new DateTime(2026, 7, 10, 19, 0, 0),
                Price = 250
            });

            DateTime testDate = DateTime.Parse(dateString);
            //Act
            var result = _testSessionValidator.IsValidToEdit(1, 1, 1, 1, testDate, price);
            //Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void IsValidToEdit_ValidData_Test()
        {
            //Arrange 
            //Act
            var result = _testSessionValidator.IsValidToEdit(1, 1, 1, 1, new DateTime(2026, 7, 10, 18, 0, 0), 400);
            //Assert
            Assert.IsTrue(result.IsValid);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void IsValidToDelete_NotValidData_Test(int sessionId)
        {
            //Arrange
            _bookingManager.CreateOrder(1, sessionId, new List<(short, short)> { (1, 1), (1, 2) });
            //Act
            var result = _testSessionValidator.IsValidToDelete(sessionId);
            //Assert
            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public void IsValidToDelete_ValidData_Test()
        {
            //Arrange
            //Act
            var result = _testSessionValidator.IsValidToDelete(1);
            //Assert
            Assert.IsTrue(result.IsValid);
        }
    }
}
