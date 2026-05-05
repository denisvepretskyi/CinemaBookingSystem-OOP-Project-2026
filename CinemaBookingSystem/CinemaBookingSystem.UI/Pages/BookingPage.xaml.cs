using CinemaBookingSystem.Core.Data;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Services;
using CinemaBookingSystem.UI.DisplayItems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для BookingPage.xaml
    /// </summary>
    public partial class BookingPage : Page
    {


        private List<SeatItem> _displaySeats;
        private Session _currentSession;
        public BookingPage(Session session)
        {
            _currentSession = session;
            InitializeComponent();
            DataContext = _currentSession;

            var cinema = AppData.Cinemas.GetById(_currentSession.CinemaId);
            var hall = AppData.Halls.GetById(_currentSession.HallId);
            var movie = AppData.Movies.GetById(_currentSession.MovieId);

            CinemaAddressTextBlock.DataContext = cinema;
            HallNameTextBlock.DataContext = hall;
            MovieTitleTextBlock.DataContext = movie;

            _displaySeats = new List<SeatItem>();

            if (hall != null)
            {
                SeatsItemsControl.DataContext = hall;
                var allCoordinates = AppServices.HallService.GetAllSeatsCoordinates(hall);
                var takenCoordinates = AppServices.BookingService.GetTakenSeats(_currentSession.Id);

                List<SeatItem> displaySeats = new List<SeatItem>();

                foreach (var coord in allCoordinates)
                {
                    bool isSeatOccupied = takenCoordinates.Contains(coord);

                    // Створюємо місце
                    _displaySeats.Add(new SeatItem
                    {
                        Row = coord.Row,
                        Column = coord.Column,
                        IsTaken = isSeatOccupied 
                    });
                }
                // Віддаємо готові дані на екран
                SeatsItemsControl.ItemsSource = _displaySeats;
            }
        }

        private void SeatButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Отримуємо кнопку, на яку натиснули
            Button clickedButton = sender as Button;

            // 2. Дістаємо об'єкт SeatItem, який прив'язаний до цієї кнопки
            if (clickedButton != null && clickedButton.DataContext is SeatItem seat)
            {
                seat.IsSelected = !seat.IsSelected;
                bool hasSelectedSeats = _displaySeats.Any(s => s.IsSelected);
                ConfirmBookingButton.Tag = hasSelectedSeats;
            }
        }

        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            var _selectedSeats = _displaySeats.Where(s => s.IsSelected).ToList();
            var _selectedSeatsCoordinates = _selectedSeats.Select(s => (s.Row, s.Column)).ToList();

            var booking = AppServices.BookingService.CreateOrder(AppServices.AuthorizationService.currentUser.Id, _currentSession.Id, _selectedSeatsCoordinates);
            if(booking.IsSuccess)
            {
                MessageBox.Show("Бронювання успішне!");
                foreach (var seat in _displaySeats)
                {
                    if (seat.IsSelected)
                    {
                        seat.IsTaken = true;
                        seat.IsSelected = false; 
                    }
                }
                SeatsItemsControl.Items.Refresh();
                clickedButton.Tag = false;
                this.NavigationService.Refresh();
            }
            else
            {
                MessageBox.Show($"Помилка бронювання: {booking.Message}");
            }
        }
    }
}
