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

        static string baseDir = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        static string dataDir = System.IO.Path.Combine(baseDir, "Data");
        public static IRepository<Cinema> Cinemas { get; }
        public static IRepository<Movie> Movies { get; }
        public static IRepository<Session> Sessions { get; }
        public static IRepository<Hall> Halls { get; }
        public static IOrderRepository Orders { get; }
        public static IUserRepository Users { get; }

        //static AppData()
        //{
        //    Cinemas = new JsonRepository<Cinema>(Path.Combine(dataDir, "Cinemas.json"));
        //    Movies = new JsonRepository<Movie>(Path.Combine(dataDir, "Movies.json"));
        //    Sessions = new JsonRepository<Session>(Path.Combine(dataDir, "Sessions.json"));
        //    Halls = new JsonRepository<Hall>(Path.Combine(dataDir, "Halls.json"));
        //    Orders = new OrderRepository(Path.Combine(dataDir, "Orders.json"));
        //    Users = new UserRepository(Path.Combine(dataDir, "Users.json"));
        //}

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
