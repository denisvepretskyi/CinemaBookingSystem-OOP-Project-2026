using System;
using System.Collections.Generic;
using System.Text;
using CinemaBookingSystem.Core.Enums;

namespace CinemaBookingSystem.Core.Models
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public int Duration { get; set; }
        public string Director { get; set; }


    }
}
