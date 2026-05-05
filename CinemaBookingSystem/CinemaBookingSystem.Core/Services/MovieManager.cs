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
            int duration, string director, List<Genre> genres, string url)
        {
            // Увага: переконайтеся, що ваш MovieValidator також оновлено для прийому List<Genre>,
            // або просто приберіть жанри з валідатора, якщо він їх не перевіряє.
            var validation = MovieValidator.IsValidMovie(title, description, duration, director, genres);
            if (!validation.IsValid) return validation;

            var movie = new Movie()
            {
                Title = title,
                Description = description,
                Duration = duration,
                Director = director,
                Genres = genres, // Тепер просто присвоюємо список
                PosterPath = url
            };

            _movieRepo.Add(movie);
            return (true, "Фільм успішно додано!");
        }

        public (bool isValid, string message) EditMovie(int movieId, string newTitle, string newDescription,
            int newDuration, string newDirector, List<Genre> newGenres , string url) // Змінено на List<Genre>
        {
            var movie = _movieRepo.GetById(movieId);
            if (movie == null) return (false, "Фільм не знайдено!");

            var validation = MovieValidator.IsValidMovie(newTitle, newDescription, newDuration, newDirector, newGenres);
            if (!validation.IsValid) return validation;

            movie.Title = newTitle;
            movie.Description = newDescription;
            movie.Duration = newDuration;
            movie.Director = newDirector;
            movie.Genres = newGenres; // Присвоюємо новий список
            movie.PosterPath = url;
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
