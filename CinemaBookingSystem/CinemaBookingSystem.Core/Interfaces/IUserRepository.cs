using System;
using System.Collections.Generic;
using System.Text;
using CinemaBookingSystem.Core.Models;

namespace CinemaBookingSystem.Core.Interfaces
{
    public interface IUserRepository : IJsonRepository<User>
    {
        User GetByPhone(string phoneNumber);
        bool IsPhoneExists(string phoneNumber);

    }
}
