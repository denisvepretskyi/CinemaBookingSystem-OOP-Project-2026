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
    public class MovieManagerTests
    {
        private string _sessionFilePath = $"test_session_db_{Guid.NewGuid()}.json";
        private string _movieFilePath = $"test_movie_db_{Guid.NewGuid()}.json";
        private IJsonRepository<Session> _testSessionRepo;
        private IJsonRepository<Movie> _testMovieRepo;
        private MovieManager _movieManager;

        [TestInitialize]
        public void Setup()
        {
            File.WriteAllText(_sessionFilePath, "[]");
            File.WriteAllText(_movieFilePath, "[]");
            _testSessionRepo = new JsonRepository<Session> (_sessionFilePath);
            _testMovieRepo = new JsonRepository<Movie>(_movieFilePath);
            _movieManager = new MovieManager(_testMovieRepo, _testSessionRepo);

        }

        [TestCleanup]
        public void CleanUp()
        {          
            if (File.Exists(_sessionFilePath)) File.Delete(_sessionFilePath);
            if (File.Exists(_movieFilePath)) File.Delete(_movieFilePath);
        }

        [TestMethod]
        public void AddMovie_Success_Test()
        {
            //Arrange
            //Act
            var result = _movieManager.AddMovie("new movie", new string('a', 101), 120, "director", Genre.Бойовик);
            //Assert
            Assert.IsTrue(result.isValid);
        }

        [TestMethod]
        public void AddMovie_NotValidData_Test()
        {
            //Arrange
            //Act
            var result = _movieManager.AddMovie("", new string('a', 101), 120, "director", Genre.Бойовик);
            //Assert
            Assert.IsFalse(result.isValid);
        }


        [TestMethod]
        public void EditMovie_Success_Test()
        {
            //Arrange
            _movieManager.AddMovie("new movie", new string('a', 101), 120, "director", Genre.Бойовик);
            //Act
            _movieManager.EditMovie(1, "edit movie", new string('a', 101), 120, "director", Genre.Бойовик);
            var movie = _testMovieRepo.GetById(1);
            //Assert
            Assert.AreEqual(movie.Title, "edit movie");
        }

        [TestMethod]
        public void EditMovie_NotValidData_Test()
        {
            //Arrange
            _movieManager.AddMovie("new movie", new string('a', 101), 120, "director", Genre.Бойовик);
            //Act
            var result = _movieManager.EditMovie(1, "edit movie", new string('a', 101), 120, "director", (Genre)50);
            var movie = _testMovieRepo.GetById(1);
            //Assert
            Assert.IsFalse(result.isValid);
            Assert.Contains(Genre.Бойовик, movie.Genres);
        }

        [TestMethod]
        public void DeleteMovie_Success_Test()
        {
            //Arrange
            _movieManager.AddMovie("new movie", new string('a', 101), 120, "director", Genre.Бойовик);
            //Act
            _movieManager.DeleteMovie(1);
            var movies =_testMovieRepo.GetAll().ToList();
            //Assert
            Assert.AreEqual(movies.Count, 0);
        }

        [TestMethod]
        public void DeleteMovie_SessionExists_Test()
        {
            //Arrange
            _movieManager.AddMovie("new movie", new string('a', 101), 120, "director", Genre.Бойовик);
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
            //Act
            var result = _movieManager.DeleteMovie(1);
            var movies = _testMovieRepo.GetAll().ToList();
            //Assert
            Assert.IsFalse(result.isValid);
            Assert.AreEqual(movies.Count, 1);
        }

    }

}
