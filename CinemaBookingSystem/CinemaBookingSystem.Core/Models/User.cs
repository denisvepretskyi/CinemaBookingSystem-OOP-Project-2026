using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Models
{
    internal class User : BaseEntity
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
