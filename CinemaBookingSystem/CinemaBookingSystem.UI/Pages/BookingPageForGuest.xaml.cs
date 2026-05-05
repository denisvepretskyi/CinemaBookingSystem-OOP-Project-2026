using CinemaBookingSystem.Core.Data;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Services;
using CinemaBookingSystem.UI.DisplayItems;
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
using static System.Collections.Specialized.BitVector32;

namespace CinemaBookingSystem.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для BookingPageForGuest.xaml
    /// </summary>
    public partial class BookingPageForGuest : Page
    {
        private Session _currentSession;
        public BookingPageForGuest(Session session)
        {
            InitializeComponent();
            _currentSession = session;
            InitializeComponent();
            DataContext = _currentSession;

            var cinema = AppData.Cinemas.GetById(_currentSession.CinemaId);
            var hall = AppData.Halls.GetById(_currentSession.HallId);
            var movie = AppData.Movies.GetById(_currentSession.MovieId);

            CinemaAddressTextBlock.DataContext = cinema;
            HallNameTextBlock.DataContext = hall;
            MovieTitleTextBlock.DataContext = movie;
        }
    }       
     
}
