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
        IOrderRepository _orderRepo;
        public delegate void UserStatusHandler();
        public event UserStatusHandler OnUserLoggedIn;
        public event UserStatusHandler OnUserLoggedOut;

        public AuthorizationManager(IUserRepository userRepo, IOrderRepository orderRepo)
        {
            _userRepo = userRepo;
            _orderRepo = orderRepo;
        }   

        public (bool IsSuccess, string Message) Login(string phoneNumber, string password)
        {
            User userToLogin = _userRepo.GetByPhone(phoneNumber);
            if (userToLogin == null) return (false,"Користувача не знайдено!") ;
            if (userToLogin.Password != password) return (false, "Невірний пароль!");

            if(userToLogin is Customer customerInfo)            
                customerInfo.Orders = _orderRepo.GetByUserId(customerInfo.Id); // підтягуємо замовлення
                      
            currentUser = userToLogin;
            OnUserLoggedIn?.Invoke();
            return (true, "");            
        }
        
        public void Logout()
        {
            currentUser = null;
            OnUserLoggedOut?.Invoke();
            return;
        }

        public (bool IsSuccess, string Message) Register(string name, string phoneNumber, string password)
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
            OnUserLoggedIn?.Invoke();
            return (true, "");
        }
    }
}
