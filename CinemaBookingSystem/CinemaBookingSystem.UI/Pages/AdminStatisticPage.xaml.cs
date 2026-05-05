using CinemaBookingSystem.Core.Data;
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

namespace CinemaBookingSystem.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminStatisticPage.xaml
    /// </summary>
    public partial class AdminStatisticPage : Page
    {
        List<CinemaStatisticModel> cinemaStatistics = new List<CinemaStatisticModel>();
        public AdminStatisticPage()
        {
            InitializeComponent();
            var cinemas = AppData.Cinemas.GetAll();
            foreach (var cinema in cinemas)
            {
                CinemaStatisticModel cinemaStat = new CinemaStatisticModel()
                {
                    Name = cinema.Name,
                    TicketsSold = AppServices.StatisticService.GetTicketsSoldCount(cinema.Id),
                    MostPopularMovie = AppServices.StatisticService.GetMostPopularMovie(cinema.Id)?.Title ?? "-",
                    TotalAmount = AppServices.StatisticService.GetCinemaTotalAmount(cinema.Id)
                };
                cinemaStatistics.Add(cinemaStat);
            }
            StatisticsItemsControl.ItemsSource = cinemaStatistics;
        }
    }
}
