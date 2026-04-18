using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Models
{
    internal class Cinema : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Hall> Halls { get; set; } = new List<Hall>();

    }
}
