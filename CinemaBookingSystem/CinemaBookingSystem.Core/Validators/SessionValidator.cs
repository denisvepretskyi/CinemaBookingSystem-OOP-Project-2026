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
        private readonly IOrderRepository _orderRepo;

        public SessionValidator(IJsonRepository<Movie> movieRepo, IJsonRepository<Session> sessionRepo, IOrderRepository orderRepo)
        {
            _movieRepo = movieRepo;
            _sessionRepo = sessionRepo;
            _orderRepo = orderRepo;
        }

        private (bool IsValid, string Message) CheckTimeConflicts(int cinemaId, int hallId, int movieId, DateTime currentStartTime)
        {
            return CheckTimeConflicts(cinemaId, hallId, movieId, currentStartTime, -1);
        }

        
        private (bool IsValid, string Message) CheckTimeConflicts(int cinemaId, int hallId, int movieId, DateTime currentStartTime, int excludeSessionId)
        {
            var currentMovie = _movieRepo.GetById(movieId);
            if (currentMovie == null) return (false, "Фільм не знайдено!");

            var currentEndTime = currentStartTime.AddMinutes(currentMovie.Duration).AddMinutes(15);
            var sessions = _sessionRepo.GetAll();

            foreach (var session in sessions)
            {
                if (session.Id == excludeSessionId)
                {
                    continue;
                }

                if (session.CinemaId == cinemaId && session.HallId == hallId)
                {
                    var movie = _movieRepo.GetById(session.MovieId);
                    var sessionEndTime = session.StartTime.AddMinutes(movie.Duration).AddMinutes(15);

                    if (currentStartTime < sessionEndTime && currentEndTime > session.StartTime)
                    {
                        return (true, "Конфікт часу!");
                    }
                }
            }
            return (false, "");
        }

        private (bool IsValid, string Message) IsTicketsSold(int sessionId)
        {
            var allOrders = _orderRepo.GetAll();
            var hasSoldTicket = allOrders
                .SelectMany(order => order.Tickets) 
                .Any(ticket => ticket.SessionId == sessionId); 

            if (hasSoldTicket)            
                return (true, "Неможливо видалити сеанс, оскільки на нього продано квитки!");          

            return (false, "");
        }


        public (bool IsValid, string Message) IsValidToAdd(int cinemaId, int hallId, int movieId, DateTime startTime, decimal price)
        {
            string message;
            if (startTime < DateTime.Now) return (false, "Не можна додати сеанс на минулу дату!");
            (bool isTimeConflict, message) = CheckTimeConflicts(cinemaId, hallId, movieId, startTime);
            
            if (isTimeConflict) return (false, message);
            if (price <= 0) return (false, "Ціна має бути більшою за нуль!");
            return (true, "");
        }
        public (bool IsValid, string Message) IsValidToEdit(int sessionId, int cinemaId,int hallId, int movieId, DateTime startTime, decimal price)
        {            
            string message;
            var session = _sessionRepo.GetById(sessionId);
            if (session == null) return (false, "Сеанс не знайдено!");
            if (startTime < DateTime.Now) return (false, "Цей сеанс вже минув!");
            (bool isTimeConflict, message) = CheckTimeConflicts(cinemaId, hallId, movieId, startTime, sessionId);
            if (isTimeConflict) return (false, message);
            if (price <= 0) return (false, "Ціна має бути більшою за нуль!");
            return (true, "");
        }

        public (bool IsValid, string Message) IsValidToDelete(int sessionId)
        {
            var session = _sessionRepo.GetById(sessionId);
            if (session == null) return (false, "Сеанс не знайдено!");

            var isTicketsSold = IsTicketsSold(sessionId);
            if (isTicketsSold.Item1) return (false, isTicketsSold.Item2);
            return (true, "");
        }
    }
}
