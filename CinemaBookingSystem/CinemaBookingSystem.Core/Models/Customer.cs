using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Models
{
    public class Customer : User
    {
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
