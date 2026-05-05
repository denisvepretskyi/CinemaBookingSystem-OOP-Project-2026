using CinemaBookingSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Services
{
    public class HallManager
    {
        public List<(short Row, short Column)> GetAllSeatsCoordinates(Hall hall)
        {
            var allSeats = new List<(short Row, short Column)>();
            if (hall == null) return allSeats;
            for (short r = 1; r <= hall.RowCount; r++)
            {
                for (short c = 1; c <= hall.ColumnCount; c++)
                {
                    allSeats.Add((r, c));
                }
            }
            return allSeats;
        }
    }
}
