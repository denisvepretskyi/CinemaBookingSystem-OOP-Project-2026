using CinemaBookingSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        public List<Order> GetByUserId(int userId);
    }
}
