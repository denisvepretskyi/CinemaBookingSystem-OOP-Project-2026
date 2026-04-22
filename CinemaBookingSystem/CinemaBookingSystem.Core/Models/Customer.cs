using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CinemaBookingSystem.Core.Models
{
    public class Customer : User
    {
        [JsonIgnore]
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
