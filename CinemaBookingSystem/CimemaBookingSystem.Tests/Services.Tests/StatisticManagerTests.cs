using CinemaBookingSystem.Core.Enums;
using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Repositories;
using CinemaBookingSystem.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CimemaBookingSystem.Tests.Services.Tests
{
    [TestClass]
    public class StatisticManagerTests
    {
        private string _orderFilePath = $"test_order_db_{Guid.NewGuid()}.json";
        private string _sessionFilePath = $"test_session_db_{Guid.NewGuid()}.json";
        private string _movieFilePath = $"test_movie_db_{Guid.NewGuid()}.json";
        private string _cinemaFilePath = $"test_cinema_db_{Guid.NewGuid()}.json";
        private string _hallFilePath = $"test_hall_db_{Guid.NewGuid()}.json";
        private string _userFilePath = $"test_user_db_{Guid.NewGuid()}.json";

        private IOrderRepository _orderRepo;
        private IJsonRepository<Session> _sessionRepo;
        private IJsonRepository<Movie> _movieRepo;
        private IJsonRepository<Cinema> _cinemaRepo;
        private IJsonRepository<Hall> _hallRepo;
        private IUserRepository _userRepo;

        private StatisticManager _statisticManager;
        private BookingManager _bookingManager;

        [TestInitialize]
        public void Setup()
        {
            File.WriteAllText(_movieFilePath, "[]");
            File.WriteAllText(_sessionFilePath, "[]");
            File.WriteAllText(_orderFilePath, "[]");
            File.WriteAllText(_cinemaFilePath, "[]");
            File.WriteAllText(_hallFilePath, "[]");
            File.WriteAllText(_userFilePath, "[]");

            _movieRepo = new JsonRepository<Movie>(_movieFilePath);
            _sessionRepo = new JsonRepository<Session>(_sessionFilePath);
            _orderRepo = new OrderRepository(_orderFilePath);
            _cinemaRepo = new JsonRepository<Cinema>(_cinemaFilePath);
            _hallRepo = new JsonRepository<Hall>(_hallFilePath);
            _userRepo = new UserRepository(_userFilePath);

            _statisticManager = new StatisticManager(_orderRepo, _sessionRepo, _movieRepo, _cinemaRepo);
            _bookingManager = new BookingManager(_orderRepo, _sessionRepo, _hallRepo, _userRepo);

            _cinemaRepo.Add(new Cinema()
            {
                Name = "Тестовий кінотеатр",
                Address = "Адреса"
            });

            _hallRepo.Add(new Hall()
            {                  
                CinemaId = 1, 
                RowCount = 10,
                ColumnCount = 10 
            });

            _sessionRepo.Add(new Session()
            {
                CinemaId = 1,
                HallId = 1,
                MovieId = 1,
                StartTime = new DateTime(2027, 3, 12, 14, 0, 0),
                Price = 250
            });
            _userRepo.Add(new User()
            {
                Name = "Test User 1",
                PhoneNumber = "0954095522",
                Password = "123pAssword"
            });
            _userRepo.Add(new User()
            {
                Name = "Test User 2",
                PhoneNumber = "0953095522",
                Password = "123pAssword"
            });
        }

        [TestCleanup]
        public void CleanUp()
        {
            if (File.Exists(_movieFilePath)) File.Delete(_movieFilePath);
            if (File.Exists(_sessionFilePath)) File.Delete(_sessionFilePath);
            if (File.Exists(_cinemaFilePath)) File.Delete(_cinemaFilePath);
            if (File.Exists(_orderFilePath)) File.Delete(_orderFilePath);
        }


        [TestMethod]
        public void GetCinemaTotalAmount_Success_Test()
        {
            //Arrange
            _bookingManager.CreateOrder(1, 1, new List<(short, short)> { (1, 2), (1, 3) });
            _bookingManager.CreateOrder(2, 1, new List<(short, short)> { (1, 4), (1, 5) });
            //Act
            var result = _statisticManager.GetCinemaTotalAmount(1);
            //Assert
            Assert.AreEqual(result, 1000);
        }

        [TestMethod]
        public void GetCinemaTotalAmount_CinemaNotExists_Test()
        {
            //Arrange
            //Act
            var result = _statisticManager.GetCinemaTotalAmount(3);
            //Assert
            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void GetMostPopularMovie_Success_Test()
        {
            //Arrange
            _movieRepo.Add(new Movie()
            {
                Title = "Test 1",
                Genres = new List<Genre> { Genre.Бойовик },
                Duration = 120               
            });

            _movieRepo.Add(new Movie()
            {
                Title = "Test 2",
                Genres = new List<Genre> { Genre.Бойовик },
                Duration = 120
            });

            _sessionRepo.Add(new Session()
            {
                CinemaId = 1,
                HallId = 1,
                MovieId = 2,
                StartTime = new DateTime(2027, 3, 12, 14, 0, 0),
                Price = 250
            });

            _bookingManager.CreateOrder(1, 1, new List<(short, short)> { (1, 2), (1, 3) });
            _bookingManager.CreateOrder(2, 2, new List<(short, short)> { (1, 4), (1, 5) });
            _bookingManager.CreateOrder(2, 2, new List<(short, short)> { (2, 4), (2, 5) });
            //Act
            var result = _statisticManager.GetMostPopularMovie(1);
            //Assert
            Assert.AreEqual(result.Id, 2);
        }

        [TestMethod]
        public void GetMostPopularMovie_CinemaNotExists_Test()
        {
            //Arrange
            _movieRepo.Add(new Movie()
            {
                Title = "Test 1",
                Genres = new List<Genre> { Genre.Бойовик },
                Duration = 120
            });

            _movieRepo.Add(new Movie()
            {
                Title = "Test 2",
                Genres = new List<Genre> { Genre.Бойовик },
                Duration = 120
            });

            _bookingManager.CreateOrder(1, 1, new List<(short, short)> { (1, 2), (1, 3) });
            _bookingManager.CreateOrder(2, 1, new List<(short, short)> { (1, 4), (1, 5) });
            _bookingManager.CreateOrder(3, 1, new List<(short, short)> { (2, 4), (2, 5) });
            //Act
            var result = _statisticManager.GetMostPopularMovie(3);
            //Assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public void GetMostPopularMovie_OrdersNotExists_Test()
        {
            //Arrange
            _movieRepo.Add(new Movie()
            {
                Title = "Test 1",
                Genres = new List<Genre> { Genre.Бойовик },
                Duration = 120
            });

            _movieRepo.Add(new Movie()
            {
                Title = "Test 2",
                Genres = new List<Genre> { Genre.Бойовик },
                Duration = 120
            });
            //Act
            var result = _statisticManager.GetMostPopularMovie(1);
            //Assert
            Assert.IsNull(result);
        }
    }     
}
