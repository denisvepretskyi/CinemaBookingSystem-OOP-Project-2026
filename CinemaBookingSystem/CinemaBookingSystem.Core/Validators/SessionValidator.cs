using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Validators
{
    public class SessionValidator
    {
        private readonly IJsonRepository<Movie> _movieRepo;
        private readonly IJsonRepository<Session> _sessionRepo;
        private readonly IJsonRepository<Ticket> _ticketRepo;

        public SessionValidator(IJsonRepository<Movie> movieRepo, IJsonRepository<Session> sessionRepo, IJsonRepository<Ticket> ticketRepo)
        {
            _movieRepo = movieRepo;
            _sessionRepo = sessionRepo;
            _ticketRepo = ticketRepo;
        }

        public (bool, string) CheckTimeConflicts(int cinemaId, int hallId, int movieId, DateTime currentStartTime)
        {
            var currentMovie = _movieRepo.GetById(movieId);
            var currentEndTime = currentStartTime.AddMinutes(currentMovie.Duration).AddMinutes(15);
            var sessions = _sessionRepo.GetAll();
            foreach (var session in sessions)
            {
                if (session.CinemaId == cinemaId && session.HallId == hallId)
                {
                    var movie = _movieRepo.GetById(session.MovieId);
                    var sessionEndTime = session.StartTime.AddMinutes(movie.Duration).AddMinutes(15);
                    if (currentStartTime < sessionEndTime && currentEndTime > session.StartTime)
                    {
                        return (true, "Конфікт часу!"); //  конфлікт існує
                    }
                }
            }
            return (false, ""); // конфлікт відсутній
        }

        public (bool, string) IsTicketsSold(int sessionId)
        {
            var tickets = _ticketRepo.GetAll();
            var hasSoldTicket = tickets.Any(t => t.SessionId == sessionId);
            if (hasSoldTicket)
            {
                return (true, "Неможливо видалити сеанс, оскільки на нього продано квитки!");
            }
            return (false, "");
        }


        public (bool, string) IsValidToAdd(int cinemaId, int hallId, int movieId, DateTime startTime, decimal price)
        {
            string message;
            (bool isTimeConflict, message) = CheckTimeConflicts(cinemaId, hallId, movieId, startTime);
            if (startTime < DateTime.Now) return (false, "Не можна додати сеанс на минулу дату!");
            if (isTimeConflict) return (false, message);
            if (price <= 0) return (false, "Ціна має бути більшою за нуль!");
            return (true, "");
        }

        public (bool, string) IsValidToEdit(int sessionId, int cinemaId, int hallId, int movieId, DateTime startTime, decimal price)
        {
            var session = _sessionRepo.GetById(sessionId);
            string message;

            if (session == null) return (false, "Сеанс не знайдено!");
            if (session.StartTime < DateTime.Now) return (false, "Цей сеанс вже минув!");

            (bool isTimeConflict, message) = CheckTimeConflicts(cinemaId, hallId, movieId, startTime);
            if (isTimeConflict) return (false, message);
            if (price <= 0) return (false, "Ціна має бути більшою за нуль!");
            return (true, "");
        }


        public (bool, string) IsValidToDelete(int sessionId)
        {
            var session = _sessionRepo.GetById(sessionId);
            if (session == null) return (false, "Сеанс не знайдено!");

            var isTicketsSold = IsTicketsSold(sessionId);
            if (isTicketsSold.Item1) return (false, isTicketsSold.Item2);
            return (true, "");
        }
    }
}
