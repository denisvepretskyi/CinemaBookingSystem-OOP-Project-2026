using CinemaBookingSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Interfaces
{
    public interface IOrderRepository : IJsonRepository<Order>
    {
        public List<Order> GetByUserId(int userId);
    }
}
