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
    /// Логика взаимодействия для AdminSessionPage.xaml
    /// </summary>
    public partial class AdminSessionPage : Page
    {
        public AdminSessionPage()
        {
            InitializeComponent();
            LoadSession();
        }

        private void LoadSession()
        {
            SessionDataGrid.ItemsSource = null; // Скидаємо прив'язку
            SessionDataGrid.ItemsSource = AppData.Sessions.GetAll(); // Завантажуємо з бази
        }

      
        private void AddSession_Click(object sender, RoutedEventArgs e)
        {
            SessionEditWindow window = new SessionEditWindow();
            window.ShowDialog();
            LoadSession();
        }

        private void EditSession_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Session selectedSession)
            {
                var TryEdit = AppServices.SessionValidator.IsTicketsSold(selectedSession.Id);
                if (TryEdit.IsValid)
                {
                    MessageBox.Show($"Помилка при редагуванні сеансу: {TryEdit.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    SessionEditWindow window = new SessionEditWindow(selectedSession);
                    window.ShowDialog();
                    LoadSession();
                }
            }
        }

        private void DeleteSession_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Session selectedSession)
            {
                var TryDelete = AppServices.SessionService.DeleteSession(selectedSession.Id);
                if (TryDelete.isValid)
                {
                    MessageBox.Show("Сеанс успішно видалено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadSession();
                }
                else
                {
                    MessageBox.Show($"Помилка при видаленні сеансу: {TryDelete.message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
