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
        private readonly IJsonRepository<Movie> _movieRepo;
        private readonly IJsonRepository<Cinema> _cinemaRepo;
        private readonly IJsonRepository<Hall> _hallRepo;
        private readonly SessionValidator _sessionValidator;

        public SessionManager
            (IJsonRepository<Session> sessionRepo, 
            SessionValidator sessionValidator,
            IJsonRepository<Movie> movieRepo,
            IJsonRepository<Cinema> cinemaRepo,
            IJsonRepository<Hall> hallRepo)
        {
            _sessionRepo = sessionRepo;
            _sessionValidator = sessionValidator;
            _movieRepo = movieRepo;
            _cinemaRepo = cinemaRepo;
            _hallRepo = hallRepo;
        }

        private (bool IsValid, string Message) CheckEntitiesExist(int cinemaId, int hallId, int movieId)
        {
            if (_cinemaRepo.GetById(cinemaId) == null)
                return (false, "Обраний кінотеатр не знайдено в базі!");

            if (_hallRepo.GetById(hallId) == null)
                return (false, "Обраний зал не знайдено в базі!");

            if (_movieRepo.GetById(movieId) == null)
                return (false, "Обраний фільм не знайдено в базі!");

            return (true, string.Empty);
        }

        public (bool isValid, string message) AddSession(int cinemaId, int hallId, int movieId, DateTime startTime, decimal price)
        {
            var isExists = CheckEntitiesExist(cinemaId, hallId, movieId);
            if (!isExists.IsValid) return isExists;
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
            if (session == null) return (false, "Сеанс не знайдено");
            var isExists = CheckEntitiesExist(cinemaId, hallId, movieId);
            if (!isExists.IsValid) return isExists;
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
            if (_sessionRepo.GetById(sessionId) == null) return (false, "Сеанс не знайдено");
            var validation = _sessionValidator.IsValidToDelete(sessionId);
            if (!validation.Item1) return validation;
            _sessionRepo.Delete(sessionId);
            return (true, "Сеанс успішно видалено!");
        }
    }
}
