using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Models
{
    public class Ticket : BaseEntity
    {
        public int SessionId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public decimal Price { get; set; }

    }
}
