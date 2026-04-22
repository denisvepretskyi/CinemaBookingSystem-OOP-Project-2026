using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CinemaBookingSystem.Core.Models
{
    public class Cinema : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }

        [JsonIgnore]
        public List<Hall> Halls { get; set; } = new List<Hall>();

    }
}
