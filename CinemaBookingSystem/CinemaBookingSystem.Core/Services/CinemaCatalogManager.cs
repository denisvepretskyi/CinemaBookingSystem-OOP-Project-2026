using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Services
{
    public class CinemaCatalogManager
    {
        private readonly IJsonRepository<Cinema> _cinemaRepo;
        private readonly IJsonRepository<Hall> _hallRepo; 

        public CinemaCatalogManager(IJsonRepository<Cinema> cinemaRepo, IJsonRepository<Hall> hallRepo)
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
