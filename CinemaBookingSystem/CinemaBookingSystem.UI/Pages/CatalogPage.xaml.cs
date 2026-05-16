using CinemaBookingSystem.Core.Data;
using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Repositories;
using CinemaBookingSystem.Core.Services;
using CinemaBookingSystem.UI.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CinemaBookingSystem.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {

        private List<Movie> _availableMovies;
        private Cinema _currentCinema;

        public CatalogPage(Cinema currentCinema)
        {
            InitializeComponent();
            _currentCinema = currentCinema;
            List<Session> cinemaSessions = AppData.Sessions.GetAll().
                Where(session => session.CinemaId == currentCinema.Id).
                Where(session => session.StartTime > DateTime.Now).ToList();
            var movieIds = cinemaSessions.Select(session => session.MovieId).Distinct().ToList();
            _availableMovies = AppData.Movies.GetAll().Where(movie => movieIds.Contains(movie.Id)).ToList();
            MoviesGrid.ItemsSource = _availableMovies;
        }

        public void SearchMovies(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                MoviesGrid.ItemsSource = _availableMovies;
            }
            else
            {
                var filteredMovies = _availableMovies
                    .Where(m => m.Title.ToLower().Contains(searchText.ToLower()))
                    .ToList();
                MoviesGrid.ItemsSource = filteredMovies;
            }
        }

        public void FilterMoviesByGenre(int genreId)
        {
            if (genreId == -1)
            {
                MoviesGrid.ItemsSource = _availableMovies;
                return;
            }
            var filteredMovies = _availableMovies
                .Where(m => m.Genres != null && m.Genres.Contains((Core.Enums.Genre)genreId)).ToList();

            MoviesGrid.ItemsSource = filteredMovies;
        }

        private void MoviePoster_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            // Дістаємо з DataContext фільм
            if (clickedButton != null && clickedButton.DataContext is Movie clickedMovie)
            {
                if (Window.GetWindow(this) is MainWindow mainWindow)
                {
                    mainWindow.MainFrame.Navigate(new MovieDetailsPage(clickedMovie, _currentCinema));
                }
            }
        }



    }
}
