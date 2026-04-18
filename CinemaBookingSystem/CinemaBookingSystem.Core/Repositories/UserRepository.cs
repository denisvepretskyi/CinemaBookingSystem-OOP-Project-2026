using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;

namespace CinemaBookingSystem.Core.Repositories
{
    internal class UserRepository : JsonRepository<User>, IUserRepository
    {
        public UserRepository(string filePath) : base(filePath)
        {
        }
        public User GetByPhone(string phoneNumber)
        {
            return GetAll().FirstOrDefault(u => u.PhoneNumber == phoneNumber);
        }
        public bool IsPhoneExists(string phoneNumber)
        {
            return GetAll().Any(u => u.PhoneNumber == phoneNumber);
        }
    }
}
