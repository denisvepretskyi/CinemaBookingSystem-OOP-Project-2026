using CinemaBookingSystem.Core.Data;
using CinemaBookingSystem.Core.Models;
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
    /// Логика взаимодействия для MovieDetailsPage.xaml
    /// </summary>
    public partial class MovieDetailsPage : Page
    {
        Movie _currentMovie;
        public MovieDetailsPage(Movie movie, Cinema cinema)
        {
            InitializeComponent();
            _currentMovie = movie;
            DataContext = _currentMovie;
            cinemaNane.DataContext = cinema;
            List<Session> sessions = AppData.Sessions.GetAll().
                Where(s => s.CinemaId == cinema.Id).
                Where(s => s.MovieId == movie.Id).
                Where(s => s.StartTime > DateTime.Now).
                ToList();
            sessions.Sort();
            SessionGrid.ItemsSource = sessions;
            if (_currentMovie.Genres != null && _currentMovie.Genres.Count > 0)            
                GenresText.Text = string.Join(", ", _currentMovie.Genres).TrimEnd(',', ' ');            
            else            
                GenresText.Text = "Немає даних";            
        }

        private void Session_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (Window.GetWindow(this) is MainWindow mainWindow)
                if (clickedButton != null && clickedButton.DataContext is Session selectedSession)
            {
                if (AppServices.AuthorizationService.currentUser == null)
                {                   
                    mainWindow.MainFrame.Navigate(new BookingPageForGuest(selectedSession));
                    return;
                }               
                mainWindow.MainFrame.Navigate(new BookingPage(selectedSession));
            }
        }
    }
}
