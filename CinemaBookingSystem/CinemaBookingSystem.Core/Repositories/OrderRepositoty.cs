using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Repositories
{
    public class OrderRepository : JsonRepository<Order>, IOrderRepository
    {
        public OrderRepository(string filePath) : base(filePath)
        {
        }
        public List<Order> GetByUserId(int userId)
        {
            return GetAll().Where(order => order.UserId == userId).ToList();
        }
    }
}
