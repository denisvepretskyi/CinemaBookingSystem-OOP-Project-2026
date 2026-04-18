using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CinemaBookingSystem.Core.Services
{
    internal class StatisticManager
    {
        private IJsonRepository<Order> _orderRepo;
        private IJsonRepository<Session> _sessionRepo;
        private IJsonRepository<Movie> _movieRepo;

        public StatisticManager(IJsonRepository<Order> orderRepo, IJsonRepository<Session> sessionRepo, IJsonRepository<Movie> movieRepo)
        {
            _orderRepo = orderRepo;
            _sessionRepo = sessionRepo;
            _movieRepo = movieRepo;
        }

        public decimal GetCinemaTotalAmount(int cinemaId)
        {
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
