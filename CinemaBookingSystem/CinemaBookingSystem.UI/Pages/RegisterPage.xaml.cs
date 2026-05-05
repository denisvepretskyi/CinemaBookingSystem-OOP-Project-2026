using CinemaBookingSystem.Core.Data;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Repositories;
using CinemaBookingSystem.Core.Services;
using CinemaBookingSystem.Core.Validators;
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
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {      
        
        public RegisterPage()
        {
            InitializeComponent(); 
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var register = AppServices.AuthorizationService.Register(NameTextBox.Text, LoginTextBox.Text, PasswordTextBox.Text);
            if(!register.IsSuccess)
            {
                MessageBox.Show(register.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show(register.Message, "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new LoginPage());
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
