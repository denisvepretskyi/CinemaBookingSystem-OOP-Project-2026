using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Models
{
    public class Session : BaseEntity, IComparable<Session>
    {
        public int HallId { get; set; }
        public int CinemaId { get; set; }
        public int MovieId { get; set; }
        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }

        public int CompareTo(Session other)
        {
            if (other == null) return 1;            
            return this.StartTime.CompareTo(other.StartTime);
        }

    }
}
