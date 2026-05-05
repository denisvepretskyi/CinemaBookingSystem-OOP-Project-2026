using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.UI.DisplayItems
{
    public class TicketDisplayModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string MovieTitle { get; set; }
        public string CinemaName { get; set; }
        public DateTime StartTime { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
