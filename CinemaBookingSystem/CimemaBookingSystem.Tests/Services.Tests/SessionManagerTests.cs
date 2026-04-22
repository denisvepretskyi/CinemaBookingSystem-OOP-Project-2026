using CinemaBookingSystem.Core.Enums;
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
    public class SessionManagerTests
    {
        private string _movieFilePath = $"test_movie_db_{Guid.NewGuid()}.json";
        private string _cinemaFilePath = $"test_cinema_db_{Guid.NewGuid()}.json";
        private string _hallFilePath = $"test_hall_db_{Guid.NewGuid()}.json";
        private string _sessionFilePath = $"test_session_db_{Guid.NewGuid()}.json";
        private string _orderFilePath = $"test_order_db_{Guid.NewGuid()}.json";
        private IJsonRepository<Movie> _testMovieRepo;
        private IJsonRepository<Session> _testSessionRepo;
        private IOrderRepository _testOrderRepo;
        private SessionValidator _testSessionValidator;
        private IJsonRepository<Cinema> _testCinemaRepo;
        private IJsonRepository<Hall> _testHallRepo;
        private SessionManager _testSessionManager;
        [TestInitialize]
        public void Setup()
        {
            File.WriteAllText(_movieFilePath, "[]");
            File.WriteAllText(_sessionFilePath, "[]");
            File.WriteAllText(_orderFilePath, "[]");
            File.WriteAllText(_cinemaFilePath, "[]");
            File.WriteAllText(_hallFilePath, "[]");
            _testMovieRepo = new JsonRepository<Movie>(_movieFilePath);
            _testSessionRepo = new JsonRepository<Session>(_sessionFilePath);
            _testOrderRepo = new OrderRepository(_orderFilePath);
            _testSessionValidator = new SessionValidator(_testMovieRepo, _testSessionRepo, _testOrderRepo);
            _testCinemaRepo = new JsonRepository<Cinema>(_cinemaFilePath);
            _testHallRepo = new JsonRepository<Hall>(_hallFilePath);
            _testSessionManager = new SessionManager
                (_testSessionRepo,
                _testSessionValidator,
                _testMovieRepo,
                _testCinemaRepo,
                _testHallRepo);

            _testMovieRepo.Add(new Movie()
            {
                Title = "Тестовий фільм",
                Duration = 120,
                Genres = { Genre.Бойовик }
            });

            _testCinemaRepo.Add(new Cinema()
            {
                Name = "Тестовий кінотеатр",
                Address = "Адреса"
            });

            _testHallRepo.Add(new Hall()
            {
                Name = "Тестовий зал",
                RowCount = 5,
                ColumnCount = 5
            });
        }

        [TestCleanup]
        public void CleanUp()
        {
            if (File.Exists(_movieFilePath)) File.Delete(_movieFilePath);
            if (File.Exists(_sessionFilePath)) File.Delete(_sessionFilePath);
            if (File.Exists(_orderFilePath)) File.Delete(_orderFilePath);
            if (File.Exists(_cinemaFilePath)) File.Delete(_cinemaFilePath);
            if (File.Exists(_hallFilePath)) File.Delete(_hallFilePath);
        }


      
        [TestMethod]
        public void AddSession_Success_Test()
        {
            //Arramge
            //Act
            var result = _testSessionManager.AddSession(1, 1, 1, DateTime.Now.AddHours(1), 250);
            var session = _testSessionRepo.GetAll().ToList();
            //Assert
            Assert.IsTrue(result.isValid);
            Assert.AreEqual(1, session.Count);
        }

        [DataTestMethod]
        [DataRow(1, 1, 10)]
        [DataRow(10, 1, 1)]
        [DataRow(1, 10, 1)]
        public void AddSession_EntityNotExists_Test(int cinemaId, int hallId, int movieId)
        {
            //Arramge
            //Act
            var result = _testSessionManager.AddSession(cinemaId, hallId, movieId, DateTime.Now.AddHours(1), 250);
            var session = _testSessionRepo.GetAll().ToList();
            //Assert
            Assert.IsFalse(result.isValid);
            Assert.AreEqual(0, session.Count);
        }        

        [TestMethod]
        public void AddSession_NotValidData_Test()
        {
            //Arramge
            //Act
            var result = _testSessionManager.AddSession(1, 1, 1, DateTime.Now.AddHours(1), -250);
            var session = _testSessionRepo.GetAll().ToList();
            //Assert
            Assert.IsFalse(result.isValid);
            Assert.AreEqual(0, session.Count);
        }


        [TestMethod]
        public void EditSession_Success_Test()
        {
            //Arramge
            _testSessionManager.AddSession(1, 1, 1, DateTime.Now.AddHours(1), 250);
            //Act
            var result = _testSessionManager.EditSession(1, 1, 1, 1, DateTime.Now.AddHours(1), 300);
            var session = _testSessionRepo.GetById(1);
            //Assert
            Assert.IsTrue(result.isValid);
            Assert.AreEqual(session.Price, 300);
        }

        [DataTestMethod]
        [DataRow(1, 1, 10)]
        [DataRow(10, 1, 1)]
        [DataRow(1, 10, 1)]
        public void EditSession_EntityNotExists_Test(int cinemaId, int hallId, int movieId)
        {
            //Arramge
            _testSessionManager.AddSession(1, 1, 1, DateTime.Now.AddHours(1), 250);
            //Act
            var result = _testSessionManager.EditSession(1, cinemaId, hallId, movieId, DateTime.Now.AddHours(1), 300);
            var session = _testSessionRepo.GetById(1);
            //Assert
            Assert.IsFalse(result.isValid);
            Assert.AreEqual(session.CinemaId, 1);
            Assert.AreEqual(session.HallId, 1);
            Assert.AreEqual(session.MovieId, 1);
        }

        [TestMethod]
        public void EditSession_NotValidData_Test()
        {
            //Arramge
            _testSessionManager.AddSession(1, 1, 1, DateTime.Now.AddHours(1), 250);
            //Act
            var result = _testSessionManager.EditSession(1, 1, 1, 1, DateTime.Now.AddHours(1), -300);
            var session = _testSessionRepo.GetById(1);
            //Assert
            Assert.IsFalse(result.isValid);
            Assert.AreEqual(session.Price, 250);
        }

        [TestMethod]
        public void DeleteSession_Success_Test()
        {
            //Arramge
            _testSessionManager.AddSession(1, 1, 1, DateTime.Now.AddHours(1), 250);
            //Act
            var result = _testSessionManager.DeleteSession(1);
            var session = _testSessionRepo.GetAll().ToList();
            //Assert
            Assert.IsTrue(result.isValid);
            Assert.AreEqual(0, session.Count);

        }
           

    }
}
