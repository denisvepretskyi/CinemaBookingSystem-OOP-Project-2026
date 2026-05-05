using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Data
{
    public static class AppData
    {

        static string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        static string dataDir = Path.Combine(baseDir, "Data");
        public static IJsonRepository<Cinema> Cinemas { get; }
        public static IJsonRepository<Movie> Movies { get; }
        public static IJsonRepository<Session> Sessions { get; }
        public static IJsonRepository<Hall> Halls { get; }
        public static IOrderRepository Orders { get; }
        public static IUserRepository Users { get; }

        static AppData()
        {
            Cinemas = new JsonRepository<Cinema>("D:\\ЛАБЫ\\2 КУРС\\2\\ООП курсач\\CinemaBookingSystem-OOP-Project-2026\\CinemaBookingSystem\\CinemaBookingSystem.Core\\Data\\Cinemas.json");
            Movies = new JsonRepository<Movie>("D:\\ЛАБЫ\\2 КУРС\\2\\ООП курсач\\CinemaBookingSystem-OOP-Project-2026\\CinemaBookingSystem\\CinemaBookingSystem.Core\\Data\\Movies.json");
            Sessions = new JsonRepository<Session>("D:\\ЛАБЫ\\2 КУРС\\2\\ООП курсач\\CinemaBookingSystem-OOP-Project-2026\\CinemaBookingSystem\\CinemaBookingSystem.Core\\Data\\Sessions.json");
            Halls = new JsonRepository<Hall>("D:\\ЛАБЫ\\2 КУРС\\2\\ООП курсач\\CinemaBookingSystem-OOP-Project-2026\\CinemaBookingSystem\\CinemaBookingSystem.Core\\Data\\Halls.json");
            Orders = new OrderRepository("D:\\ЛАБЫ\\2 КУРС\\2\\ООП курсач\\CinemaBookingSystem-OOP-Project-2026\\CinemaBookingSystem\\CinemaBookingSystem.Core\\Data\\Orders.json");
            Users = new UserRepository("D:\\ЛАБЫ\\2 КУРС\\2\\ООП курсач\\CinemaBookingSystem-OOP-Project-2026\\CinemaBookingSystem\\CinemaBookingSystem.Core\\Data\\Users.json");
        }
    }
}
