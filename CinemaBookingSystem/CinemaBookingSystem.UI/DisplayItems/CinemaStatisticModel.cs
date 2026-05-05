using CinemaBookingSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.UI.DisplayItems
{
    internal class CinemaStatisticModel
    {
        public string Name { get; set; }
        public int TicketsSold { get; set; }
        public string MostPopularMovie { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
