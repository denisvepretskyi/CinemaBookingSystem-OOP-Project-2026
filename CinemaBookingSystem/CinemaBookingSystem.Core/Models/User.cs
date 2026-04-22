using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CinemaBookingSystem.Core.Models
{
    [JsonDerivedType(typeof(Customer), typeDiscriminator: "customer")]
    [JsonDerivedType(typeof(Admin), typeDiscriminator: "admin")]
    public class User : BaseEntity
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
