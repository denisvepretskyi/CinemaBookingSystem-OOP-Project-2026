using System;
using System.Collections.Generic;
using System.Text;
using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Repositories;
namespace CimemaBookingSystem.Tests.Repository.Tests
{
    public class TestEntity : BaseEntity
    {
        public string Name { get; set; }
    }


    [TestClass]
    public class JsonRepositoryTests
    {
        private string TestFilePath = $"test_db_{Guid.NewGuid()}.json";
        private JsonRepository<TestEntity> _repository;


        [TestInitialize]
        public void Setup()
        {
            if (File.Exists(TestFilePath)) File.Delete(TestFilePath);
            File.WriteAllText(TestFilePath, "[]");
            _repository = new JsonRepository<TestEntity>(TestFilePath);
        }


        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(TestFilePath)) File.Delete(TestFilePath);
        }


        [TestMethod]
        public void Add_Item_Test()
        {
            // Arrange
            TestEntity testEntity = new TestEntity { Name = "Test Name" };
            // Act
            _repository.Add(testEntity);
            var testItems = _repository.GetAll().ToList();
            // Assert            
            Assert.AreEqual(1, testItems.Count);
            Assert.AreEqual(1, testItems[0].Id);
            Assert.AreEqual("Test Name", testItems[0].Name);
        }


        [TestMethod]
        public void Delete_Item_Test()
        {
            // Arrange
            TestEntity testEntity1 = new TestEntity { Name = "Test Name 1" };
            TestEntity testEntity2 = new TestEntity { Name = "Test Name 2" };
            // Act
            _repository.Add(testEntity1);
            _repository.Add(testEntity2);
            _repository.Delete(testEntity1.Id);
            var testItems = _repository.GetAll().ToList();
            // Assert            
            Assert.AreEqual(1, testItems.Count);
            Assert.IsNull(_repository.GetById(1));
        }

        [TestMethod]
        public void GetById_Test()
        {
            // Arrange
            TestEntity testEntity1 = new TestEntity { Name = "Test Name 1" };
            TestEntity testEntity2 = new TestEntity { Name = "Test Name 2" };
            // Act
            _repository.Add(testEntity1);
            _repository.Add(testEntity2);
            // Assert
            Assert.AreEqual("Test Name 1", _repository.GetById(1).Name);
            Assert.AreEqual("Test Name 2", _repository.GetById(2).Name);
            Assert.IsNull(_repository.GetById(3));
        }

        [TestMethod]
        public void Update_Test()
        {
            // Arrange
            TestEntity testEntity1 = new TestEntity { Name = "Test Name 1" };
            // Act
            _repository.Add(testEntity1);
            testEntity1.Name = "Updated Name";
            _repository.Update(testEntity1);
            // Assert
            Assert.AreEqual("Updated Name", _repository.GetById(1).Name);
        }

        [TestMethod]
        public void GetAll_Success_Test()
        {
            // Arrange
            TestEntity testEntity1 = new TestEntity { Name = "Test Name 1" };
            TestEntity testEntity2 = new TestEntity { Name = "Test Name 2" };
            TestEntity testEntity3 = new TestEntity { Name = "Test Name 3" };
            TestEntity testEntity4 = new TestEntity { Name = "Test Name 4" };
            // Act
            _repository.Add(testEntity1);
            _repository.Add(testEntity2);
            _repository.Add(testEntity3);
            _repository.Add(testEntity4);
            var testItems = _repository.GetAll().ToList();
            // Assert
            Assert.AreEqual(4, testItems.Count);
            Assert.AreEqual(testItems[0].Name, "Test Name 1");
            Assert.AreEqual(testItems[3].Name, "Test Name 4");
        }

        [TestMethod]
        public void GetAll_EmptyDB_Test()
        {
            // Arrange        
            // Act            
            var testItems = _repository.GetAll().ToList();
            // Assert
            Assert.AreEqual(0, testItems.Count);
        }
    }
}
