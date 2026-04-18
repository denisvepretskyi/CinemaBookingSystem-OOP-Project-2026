using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Models
{
    internal class Order : BaseEntity
    {
        public int UserId { get; set; }
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
        public decimal TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public bool IsCancelled { get; set; }
    }
}
