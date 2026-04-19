using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Models
{
    public class Hall : BaseEntity
    {
        public int CinemaId { get; set; }
        public string Name { get; set; }
        public short RowCount { get; set; }
        public short ColumnCount { get; set; }
    }
}
