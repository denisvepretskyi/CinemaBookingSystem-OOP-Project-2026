using CinemaBookingSystem.Core.Enums;
using CinemaBookingSystem.Core.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace CimemaBookingSystem.Tests.Validators.Tests
{
    [TestClass]
    public class MovieValidatorTests
    {

        [TestMethod]
        public void NotValid_Title_Test()
        {
            // Arrange
            string title = "";
            Genre genre = (Genre)1;
            int duration = 120;
            string director = null;
            string description = null;
            // Act
            var result = MovieValidator.IsValidMovie(title, description, duration, director, genre);
            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Message, "Назва не може бути порожньою!");
        }

        [TestMethod]
        public void NotValid_Duration_Test()
        {
            // Arrange
            string title = "Valid Title";
            Genre genre = (Genre)1;
            int duration = -1;
            string director = null;
            string description = null;
            // Act
            var result = MovieValidator.IsValidMovie(title, description, duration, director, genre);
            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Message, "Тривалість має бути більшою за нуль!");
        }

        [TestMethod]
        public void NotValid_Genre_Test()
        {
            // Arrange
            string title = "Valid Title";
            Genre genre = (Genre)50;
            int duration = 120;
            string director = null;
            string description = null;
            // Act
            var result = MovieValidator.IsValidMovie(title, description, duration, director, genre);
            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Message, "Невірний жанр!");
        }

        [TestMethod]
        public void NotValid_Description_Test()
        {
            // Arrange
            string title = "Valid Title";
            Genre genre = (Genre)1;
            int duration = 120;
            string director = null;
            string description = "little description";
            // Act
            var result = MovieValidator.IsValidMovie(title, description, duration, director, genre);
            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Message, "Опис має бути від 100 до 1000 символів!");
        }

        [TestMethod]
        public void NotValid_Director_Test()
        {
            // Arrange
            string title = "Valid Title";
            Genre genre = (Genre)1;
            int duration = 120;
            string director = new string('a', 101);
            string description = null;
            // Act
            var result = MovieValidator.IsValidMovie(title, description, duration, director, genre);
            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Message, "Ім'я режисера не може перевищувати 100 символів!");
        }


        [TestMethod]
        public void Valid_Data_Test()
        {
            // Arrange
            string title = "Valid Title";
            Genre genre = (Genre)1;
            int duration = 120;
            string director = new string('a', 50);
            string description = null;
            // Act
            var result = MovieValidator.IsValidMovie(title, description, duration, director, genre);
            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(result.Message, string.Empty);
        }
    }
}
