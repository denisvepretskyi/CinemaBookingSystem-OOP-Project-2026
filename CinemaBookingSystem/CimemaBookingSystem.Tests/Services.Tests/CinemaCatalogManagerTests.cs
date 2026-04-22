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
    public class CinemaCatalogManagerTests
    {
        private string _cinemaFilePath = $"test_cinema_db_{Guid.NewGuid()}.json";
        private string _hallFilePath = $"test_hall_db_{Guid.NewGuid()}.json";

        private IJsonRepository<Cinema> _testCinemaRepo;
        private IJsonRepository<Hall> _testHallRepo;
        private CinemaCatalogManager _cinemaCatalogManager;

        [TestInitialize]
        public void Setup()
        {
            File.WriteAllText(_cinemaFilePath, "[]");
            File.WriteAllText(_hallFilePath, "[]");

            _testCinemaRepo = new JsonRepository<Cinema>(_cinemaFilePath);
            _testHallRepo = new JsonRepository<Hall>(_hallFilePath);
            _cinemaCatalogManager = new CinemaCatalogManager(_testCinemaRepo, _testHallRepo);

            _testCinemaRepo.Add(new Cinema()
            {
                Id = 1,
                Name = "Test Cinema",
                Address = "123 Test St"
            });

            _testHallRepo.Add(new Hall()
                {
                    Id = 1,
                    CinemaId = 1,
                    Name = "Hall 1",
                    RowCount = 10,
                    ColumnCount = 20
                });
    
                _testHallRepo.Add(new Hall()
                {
                    Id = 2,
                    CinemaId = 1,
                    Name = "Hall 2",
                    RowCount = 15,
                    ColumnCount = 25
                });
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_cinemaFilePath)) File.Delete(_cinemaFilePath);
            if (File.Exists(_hallFilePath)) File.Delete(_hallFilePath);
        }

        [TestMethod]
        public void GetCinemasWithHalls_Test()
        {
            //Arrange
            //Act
            var result = _cinemaCatalogManager.GetCinemasWithHalls();
            //Assert
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result[0].Halls.Count, 2);
        }

    }        
}
