using System;
using System.Collections.Generic;
using System.Text;
using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Enums;
using CinemaBookingSystem.Core.Validators;


namespace CinemaBookingSystem.Core.Services
{
    public class MovieManager
    {
        private readonly IJsonRepository<Movie> _movieRepo;
        private readonly IJsonRepository<Session> _sessionRepo;

        public MovieManager(IJsonRepository<Movie> movieRepo, IJsonRepository<Session> sessionRepo)
        {
            _movieRepo = movieRepo;
            _sessionRepo = sessionRepo;
        }

        public (bool isValid, string message) AddMovie(string title, string description,
            int duration, string director, Genre genre)
        {
            var validation = MovieValidator.IsValidMovie(title, description, duration, director, genre);
            if (!validation.IsValid) return validation;

            var movie = new Movie()
            {
                Title = title,
                Description = description,
                Duration = duration,
                Director = director,
                Genres = new List<Genre> { genre }
            };

            _movieRepo.Add(movie);
            return (true, "Фільм успішно додано!");
        }

        public (bool isValid, string message) EditMovie(int movieId, string newTitle, string newDescription,
            int newDuration, string newDirector, Genre newGenre)
        {
            var movie = _movieRepo.GetById(movieId);
            if (movie == null) return (false, "Фільм не знайдено!");
            var validation = MovieValidator.IsValidMovie(newTitle, newDescription, newDuration, newDirector, newGenre);
            if (!validation.IsValid) return validation;

            movie.Title = newTitle;
            movie.Description = newDescription;
            movie.Duration = newDuration;
            movie.Director = newDirector;
            movie.Genres = new List<Genre> { newGenre };
            _movieRepo.Update(movie);
            return (true, "Фільм успішно оновлено!");
        }

        public (bool isValid, string message) DeleteMovie(int movieId)
        {
            var movie = _movieRepo.GetById(movieId);
            if (movie == null) return (false, "Фільм не знайдено!");
            var sessionsWithMovie = _sessionRepo.GetAll().Where(s => s.MovieId == movieId).ToList();
            if (sessionsWithMovie.Count > 0) return (false, "Неможливо видалити фільм, оскільки він має активні сеанси!");
            _movieRepo.Delete(movieId);
            return (true, "Фільм успішно видалено!");
        }
    }
}
