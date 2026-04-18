using CinemaBookingSystem.Core.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;

namespace CinemaBookingSystem.Core.Services
{
    public class SessionManager
    {
        private readonly IJsonRepository<Session> _sessionRepo;
        private readonly SessionValidator _sessionValidator;

        public SessionManager(IJsonRepository<Session> sessionRepo, SessionValidator sessionValidator)
        {
            _sessionRepo = sessionRepo;
            _sessionValidator = sessionValidator;
        }

        public (bool isValid, string message) AddSession(int cinemaId, int hallId, int movieId, DateTime startTime, decimal price)
        {
            var validation = _sessionValidator.IsValidToAdd(cinemaId, hallId, movieId, startTime, price);
            if (!validation.Item1) return validation;
            var session = new Session()
            {
                CinemaId = cinemaId,
                HallId = hallId,
                MovieId = movieId,
                StartTime = startTime,
                Price = price
            };
            _sessionRepo.Add(session);
            return (true, "Сеанс успішно додано!");
        }

        public (bool isValid, string message) EditSession(int sessionId, int cinemaId, int hallId, int movieId, DateTime startTime, decimal price)
        {
            var session = _sessionRepo.GetById(sessionId);
            var validation = _sessionValidator.IsValidToEdit(sessionId, cinemaId, hallId, movieId, startTime, price);
            if (!validation.Item1) return validation;
            session.CinemaId = cinemaId;
            session.HallId = hallId;
            session.MovieId = movieId;
            session.StartTime = startTime;
            session.Price = price;
            _sessionRepo.Update(session);
            return (true, "Сеанc успішно оновлено!");
        }

        public (bool isValid, string message) DeleteSession(int sessionId)
        {
            var validation = _sessionValidator.IsValidToDelete(sessionId);
            if (!validation.Item1) return validation;
            _sessionRepo.Delete(sessionId);
            return (true, "Сеанс успішно видалено!");
        }
    }
}
