using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Services
{
    public class CinemaCatalogManager
    {
        private readonly IRepository<Cinema> _cinemaRepo;
        private readonly IRepository<Hall> _hallRepo; 

        public CinemaCatalogManager(IRepository<Cinema> cinemaRepo, IRepository<Hall> hallRepo)
        {
            _cinemaRepo = cinemaRepo;
            _hallRepo = hallRepo;
        }

        public List<Cinema> GetCinemasWithHalls()
        {
            var cinemas = _cinemaRepo.GetAll();
            var allHalls = _hallRepo.GetAll();

            foreach (var cinema in cinemas)            
                cinema.Halls = allHalls.Where(hall => hall.CinemaId == cinema.Id).ToList();            

            return cinemas.ToList();
        }
    }
}
