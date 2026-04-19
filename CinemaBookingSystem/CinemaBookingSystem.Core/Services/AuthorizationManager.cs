using System;
using System.Collections.Generic;
using System.Text;
using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Repositories;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Validators;

namespace CinemaBookingSystem.Core.Services
{
    public class AuthorizationManager
    {
        public User currentUser {  get; private set; }
        IUserRepository _userRepo;

        public AuthorizationManager(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }   

        public (bool, string) Login(string phoneNumber, string password)
        {
            User userToLogin = _userRepo.GetByPhone(phoneNumber);
            if (userToLogin == null) return (false,"Користувача не знайдено!") ;
            if (userToLogin.Password != password) return (false, "Невірний пароль!");
            currentUser = userToLogin;
            return (true, "");
        }
        
        public void Logout()
        {
            currentUser = null;
        }

        public (bool, string) Register(string name, string phoneNumber, string password)
        {
            if (_userRepo.IsPhoneExists(phoneNumber)) return (false, "Користувач з таким номером телефону вже існує!");

            var numberValidation = UserValidator.IsValidPhoneNumber(phoneNumber);
            if (numberValidation.IsValid == false) return numberValidation;

            var passwordValidation = UserValidator.IsValidPassword(password);
            if (passwordValidation.IsValid == false) return passwordValidation;

            User newUser = new Customer()
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Password = password
            };
            _userRepo.Add(newUser);
            currentUser = newUser;
            return (true, "");
        }
    }
}
