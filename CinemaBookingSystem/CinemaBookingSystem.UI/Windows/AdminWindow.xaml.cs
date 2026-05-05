using CinemaBookingSystem.Core.Services;
using CinemaBookingSystem.UI.Pages;
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
using System.Windows.Shapes;

namespace CinemaBookingSystem.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            AdminContentFrame.Navigate(new AdminMoviePage());
        }
        private void NavMovies_Click(object sender, RoutedEventArgs e)
        {
            AdminContentFrame.Navigate(new AdminMoviePage());
        }

        private void NavSessions_Click(object sender, RoutedEventArgs e)
        {
            AdminContentFrame.Navigate(new AdminSessionPage());
        }

        private void NavStats_Click(object sender, RoutedEventArgs e)
        {
            AdminContentFrame.Navigate(new AdminStatisticPage());

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            AppServices.AuthorizationService.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            if (Window.GetWindow(this) != null)
            {
                mainWindow.Left = Window.GetWindow(this).Left;
                mainWindow.Top = Window.GetWindow(this).Top;
                mainWindow.Width = Window.GetWindow(this).ActualWidth;
                mainWindow.Height = Window.GetWindow(this).ActualHeight;
                mainWindow.WindowState = Window.GetWindow(this).WindowState;
            }
            mainWindow.Show();
            this.Close();

        }

    }
}
