using CinemaBookingSystem.Core.Validators;
using System;
using System.Collections.Generic;
using System.Text;


namespace CimemaBookingSystem.Tests.Validators.Tests
{
    [TestClass]
    public class UserValidatorTests
    {
        [DataTestMethod]
        [DataRow("1234567890")]
        [DataRow("099123456")]
        [DataRow("09912345678")]
        [DataRow("38095409333")]
        [DataRow("3809540955300")]
        [DataRow("099123abcd")]
        [DataRow("099-123-45")]
        [DataRow("")]
        [DataRow(null)]
        public void NotValid_Phone_Test(string phoneNumber)
        {
            // Arrange
            // Act
            var result = UserValidator.IsValidPhoneNumber(phoneNumber);
            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [DataTestMethod]
        [DataRow("+380991234567")]
        [DataRow("0991234567")]
        public void Valid_Phone_Test(string phoneNumber)
        {
            // Arrange
            // Act
            var result = UserValidator.IsValidPhoneNumber(phoneNumber);
            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("1234567")]
        [DataRow("0a345678b")]
        [DataRow("0A345678B")]
        [DataRow("abcdefgh")]
        public void NotValid_Password_Test(string password)
        {
            // Arrange
            // Act
            var result = UserValidator.IsValidPassword(password);
            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Valid_Password_Test()
        {
            // Arrange
            string password = "01aAjg93dhA7";
            // Act
            var result = UserValidator.IsValidPassword(password);
            // Assert
            Assert.IsTrue(result.IsValid);
        }
    }
}
