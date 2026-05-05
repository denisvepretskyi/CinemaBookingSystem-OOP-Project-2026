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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            AppServices.AuthorizationService.Logout();
            UnsubscribeAll();
            AppServices.AuthorizationService.OnAdminLoggedIn += HandleAdminLogin;
            AppServices.AuthorizationService.OnCustomerLoggedIn += HandleCustomerLogin;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginResult = AppServices.AuthorizationService.Login(LoginTextBox.Text, PasswordTextBox.Password);
            if (!loginResult.IsSuccess)
            {
                MessageBox.Show(loginResult.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }  
        
        private void HandleAdminLogin()
        {
            UnsubscribeAll();
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            if (Window.GetWindow(this) != null)
            {
                adminWindow.Left = Window.GetWindow(this).Left;
                adminWindow.Top = Window.GetWindow(this).Top;
                adminWindow.Width = Window.GetWindow(this).ActualWidth;
                adminWindow.Height = Window.GetWindow(this).ActualHeight;
                adminWindow.WindowState = Window.GetWindow(this).WindowState;
            }
            adminWindow.Show();
            Window.GetWindow(this)?.Close();
        }
        
        private void HandleCustomerLogin()
        {
            UnsubscribeAll();
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new SelectCinemaPage());
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            UnsubscribeAll();
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new RegisterPage());
            }
        }

        private void GoLikeGuest_Click(object sender, RoutedEventArgs e)
        {
            UnsubscribeAll();
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new SelectCinemaPage());
            }
        }

        private void UnsubscribeAll()
        {
            AppServices.AuthorizationService.OnAdminLoggedIn -= HandleAdminLogin;
            AppServices.AuthorizationService.OnCustomerLoggedIn -= HandleCustomerLogin;
        }
    }
}
