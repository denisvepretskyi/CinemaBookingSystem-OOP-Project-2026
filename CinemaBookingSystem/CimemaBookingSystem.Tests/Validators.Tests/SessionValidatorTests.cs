using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Repositories;
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
        private string _ticketFilePath = $"test_ticket_db_{Guid.NewGuid()}.json";

        private  IJsonRepository<Movie> _testMovieRepo;
        private  IJsonRepository<Session> _testSessionRepo;
        private  IJsonRepository<Ticket> _testTicketRepo;
        private SessionValidator _testSessionValidator;

        [TestInitialize]
        public void Setup()
        {
            File.WriteAllText(_movieFilePath, "[]");
            File.WriteAllText(_sessionFilePath, "[]");
            File.WriteAllText(_ticketFilePath, "[]");

            _testMovieRepo = new JsonRepository<Movie>(_movieFilePath);
            _testSessionRepo = new JsonRepository<Session>(_sessionFilePath);
            _testTicketRepo = new JsonRepository<Ticket>(_ticketFilePath);
            _testSessionValidator = new SessionValidator(_testMovieRepo , _testSessionRepo, _testTicketRepo);

            _testMovieRepo.Add(new()
            {
                Title = "Тестовий фільм",
                Duration = 120
            });
        }

        [TestCleanup]
        public void CleanUp()
        {
            if(File.Exists(_movieFilePath)) File.Delete(_movieFilePath);
            if (File.Exists(_sessionFilePath)) File.Delete(_sessionFilePath);
            if (File.Exists(_ticketFilePath)) File.Delete(_ticketFilePath);
        }

        [TestMethod]
        public void TimeConflict_Exists_Test()
        {
            //Arrange
            Session testSession = new Session()
            {
                HallId = 1,
                CinemaId = 1,
                MovieId = 1,
                StartTime = new DateTime(2026, 7, 10, 14, 0, 0),
                Price = 250
            };
            _testSessionRepo.Add(testSession);
            //Act
            var result = _testSessionValidator.CheckTimeConflicts(1, 1, 1, new DateTime(2026, 7, 10, 15, 0, 0));
            //Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TimeConflict_NotExists_Test()
        {
            //Arrange
            Session testSession = new Session()
            {
                HallId = 1,
                CinemaId = 1,
                MovieId = 1,
                StartTime = new DateTime(2026, 7, 10, 14, 0, 0),
                Price = 250
            };
            _testSessionRepo.Add(testSession);
            //Act
            var result = _testSessionValidator.CheckTimeConflicts(1, 1, 1, new DateTime(2026, 7, 10, 18, 0, 0));
            //Assert
            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public void IsTicketSold_Success_Test()
        {
            //Arrange
            Session testSession = new Session()
            {
                HallId = 1,
                CinemaId = 1,
                MovieId = 1,
                StartTime = new DateTime(2026, 7, 10, 14, 0, 0),
                Price = 250
            };

            Ticket testTicket = new Ticket()
            {
                SessionId = 1,
                Row = 1,
                Column = 1,
                Price = 250
            };
            _testSessionRepo.Add(testSession);
            _testTicketRepo.Add(testTicket);
            //Act
            var result = _testSessionValidator.IsTicketsSold(1);
            //Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void IsTicketSold_NotSuccess_Test()
        {
            //Arrange
            Session testSession = new Session()
            {
                HallId = 1,
                CinemaId = 1,
                MovieId = 1,
                StartTime = new DateTime(2026, 7, 10, 14, 0, 0),
                Price = 250
            };
            _testSessionRepo.Add(testSession);
            //Act
            var result = _testSessionValidator.IsTicketsSold(1);
            //Assert
            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public void Past_Date_Test()
        {
            //Act
            var result = _testSessionValidator.IsValidToAdd(1, 1, 1, new DateTime(2025, 7, 10, 18, 0, 0), 250);
            //Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Negative_Price_Test()
        {
            //Act
            var result = _testSessionValidator.IsValidToAdd(1, 1, 1, new DateTime(2026, 7, 10, 18, 0, 0), -250);
            //Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Valid_Data_Test()
        {
            //Act
            var result = _testSessionValidator.IsValidToAdd(1, 1, 1, new DateTime(2026, 7, 10, 18, 0, 0), 250);
            //Assert
            Assert.IsTrue(result.IsValid);
        }
    }
}
