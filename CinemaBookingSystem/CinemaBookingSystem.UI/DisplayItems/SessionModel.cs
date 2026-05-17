using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.UI.DisplayItems
{
    public class SessionModel
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public string CinemaName { get; set; }
        public DateTime StartTime { get; set; }
        public int SoldTickets { get; set; }
    }
}
