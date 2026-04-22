using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CinemaBookingSystem.Core.Services
{
    public class StatisticManager
    {
        private readonly IJsonRepository<Order> _orderRepo;
        private readonly IJsonRepository<Session> _sessionRepo;
        private readonly IJsonRepository<Movie> _movieRepo;
        private readonly IJsonRepository<Cinema> _cinemaRepo;

        public StatisticManager
            (IJsonRepository<Order> orderRepo, 
            IJsonRepository<Session> sessionRepo, 
            IJsonRepository<Movie> movieRepo, 
            IJsonRepository<Cinema> cinemaRepo)
        {
            _orderRepo = orderRepo;
            _sessionRepo = sessionRepo;
            _movieRepo = movieRepo;
            _cinemaRepo = cinemaRepo;
        }

        public decimal GetCinemaTotalAmount(int cinemaId)
        {
            if(_cinemaRepo.GetById(cinemaId) == null)  return 0;
            List<int> sessionsIds = _sessionRepo.GetAll()
                .Where(session => session.CinemaId == cinemaId)
                .Select(session => session.Id).ToList();

            decimal totalAmount = _orderRepo.GetAll().
                SelectMany(order => order.Tickets).
                Where(ticket => sessionsIds.Contains(ticket.SessionId)).
                Sum(ticket => ticket.Price);
            return totalAmount;
        }

        public Movie GetMostPopularMovie(int cinemaId)
        {
            if (_cinemaRepo.GetById(cinemaId) == null) return null;
            var cinemaSessions = _sessionRepo.GetAll()
                .Where(session => session.CinemaId == cinemaId);

            var sessionsIds = cinemaSessions.Select(session => session.Id).ToList();
            if (sessionsIds.Count == 0) return null;

            List<Ticket> tickets = _orderRepo.GetAll()
                .SelectMany (order => order.Tickets)
                .Where(ticket => sessionsIds.Contains(ticket.SessionId)).ToList();
            if(tickets.Count == 0) return null;

            var mostPopularMovieId = tickets
                .Select(ticket => cinemaSessions.First(s => s.Id == ticket.SessionId).MovieId)
                .GroupBy(movieId => movieId)
                .OrderByDescending(group => group.Count())
                .Select(group => group.Key)
                .FirstOrDefault();

            return _movieRepo.GetById(mostPopularMovieId);
        }

    }
}
