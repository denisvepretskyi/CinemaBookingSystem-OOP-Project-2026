using CinemaBookingSystem.Core.Data;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Services;
using CinemaBookingSystem.UI.DisplayItems;
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
            SessionDataGrid.ItemsSource = null;

            // 1. Отримуємо всі сеанси
            var rawSessions = AppData.Sessions.GetAll();

            // 2. Витягуємо АБСОЛЮТНО ВСІ квитки з усіх замовлень у системі
            var allTickets = AppData.Orders.GetAll()
                                           .Where(o => o.Tickets != null)
                                           .SelectMany(o => o.Tickets)
                                           .ToList();

            var displaySessions = new List<SessionModel>();

            foreach (var session in rawSessions)
            {
                // 3. Рахуємо, скільки квитків було куплено саме на цей сеанс
                int soldCount = allTickets.Count(t => t.SessionId == session.Id);

                // Підтягуємо об'єкти фільму та кінотеатру, щоб взяти їхні назви
                var movie = AppData.Movies.GetById(session.MovieId);
                var cinema = AppData.Cinemas.GetById(session.CinemaId);

                // 4. Пакуємо все в зручну модель для XAML
                displaySessions.Add(new SessionModel
                {
                    Id = session.Id,
                    MovieTitle = movie != null ? movie.Title : "Невідомий фільм",
                    CinemaName = cinema != null ? cinema.Name : "Невідомий кінотеатр",
                    StartTime = session.StartTime,
                    SoldTickets = soldCount // Наша нова колонка!
                });
            }

            // Віддаємо готовий список з усіма даними в таблицю
            SessionDataGrid.ItemsSource = displaySessions;
        }


        private void AddSession_Click(object sender, RoutedEventArgs e)
        {
            SessionEditWindow window = new SessionEditWindow();
            window.ShowDialog();
            LoadSession();
        }

        private void EditSession_Click(object sender, RoutedEventArgs e)
        {
            // Змінили Session на SessionModel
            if (sender is Button button && button.DataContext is SessionModel selectedSessionModel)
            {
                // Перевіряємо за ID нашої моделі
                var TryEdit = AppServices.SessionValidator.IsTicketsSold(selectedSessionModel.Id);

                if (TryEdit.IsValid)
                {
                    MessageBox.Show($"Помилка при редагуванні сеансу: {TryEdit.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    Session realSession = AppData.Sessions.GetById(selectedSessionModel.Id);

                    if (realSession != null)
                    {
                        SessionEditWindow window = new SessionEditWindow(realSession);
                        window.ShowDialog();
                        LoadSession(); 
                    }
                }
            }
        }

        private void DeleteSession_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is SessionModel selectedSessionModel)
            {
                var TryDelete = AppServices.SessionService.DeleteSession(selectedSessionModel.Id);

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
