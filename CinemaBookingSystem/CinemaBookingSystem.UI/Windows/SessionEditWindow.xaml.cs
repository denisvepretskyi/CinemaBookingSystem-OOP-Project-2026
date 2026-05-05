using CinemaBookingSystem.Core.Data;
using CinemaBookingSystem.Core.Enums;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Services;
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

namespace CinemaBookingSystem.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для SessionEditWindow.xaml
    /// </summary>
    public partial class SessionEditWindow : Window
    {
        Session currentSession;

        //add
        public SessionEditWindow()
        {
            InitializeComponent();
            LoadComboBox();
            this.Title = "Cтворення сеансу";
        }


        //edit
        public SessionEditWindow(Session session)
        {
            InitializeComponent();
            LoadComboBox();
            currentSession = session;
            MovieComboBox.SelectedValue = session.MovieId;
            CinemaComboBox.SelectedValue = session.CinemaId;
            HallComboBox.SelectedValue = session.HallId;
            SessionDatePicker.SelectedDate = session.StartTime.Date;
            TimeTextBox.Text = session.StartTime.ToString("HH:mm");
            PriceTextBox.Text = session.Price.ToString("F2");
            this.Title = "Редагування сеансу";
        }

        private void LoadComboBox()
        {
            MovieComboBox.ItemsSource = AppData.Movies.GetAll();
            CinemaComboBox.ItemsSource = AppServices.CinemaCatalogService.GetCinemasWithHalls();
        }

        private void CinemaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CinemaComboBox.SelectedItem is Cinema selectedCinema)
            {
                HallComboBox.ItemsSource = selectedCinema.Halls;
                if (selectedCinema.Halls != null && selectedCinema.Halls.Count > 0)
                {
                    HallComboBox.SelectedIndex = 0;
                }
            }
            else
            {
                HallComboBox.ItemsSource = null;
            }
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {

            if(CinemaComboBox.SelectedValue == null || HallComboBox.SelectedValue == null || MovieComboBox.SelectedValue == null)
            {
                MessageBox.Show("Оберіть кінотеатр, зал та фільм!", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SessionDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Оберіть дату сеансу!", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DateTime selectedDate = SessionDatePicker.SelectedDate.Value.Date;
            if (!TimeSpan.TryParse(TimeTextBox.Text, out TimeSpan parsedTime))
            {
                MessageBox.Show("Введіть коректний час у форматі ГГ:ХХ (наприклад 14:30)!", "Помилка вводу", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DateTime StartTime = selectedDate.Add(parsedTime);

            if(!decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                    MessageBox.Show("Некоректна ціна квитка!", "Помилка вводу", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;                
            }

            if (currentSession == null)
            {
                var TryAdd = AppServices.SessionService.AddSession(
                   (int)CinemaComboBox.SelectedValue,
                   (int)HallComboBox.SelectedValue,
                   (int)MovieComboBox.SelectedValue,
                   StartTime,
                   price);

                if(!TryAdd.isValid)
                {
                    MessageBox.Show($"Не вдалося додати сеанс: {TryAdd.message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("Сеанс успішно додано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                    return;
                }
            }
            else
            {
                var TryEdit = AppServices.SessionService.EditSession(
                   currentSession.Id,
                   (int)CinemaComboBox.SelectedValue,
                   (int)HallComboBox.SelectedValue,
                   (int)MovieComboBox.SelectedValue,
                   StartTime,
                   price);

                if (!TryEdit.isValid)
                {
                    MessageBox.Show($"Не вдалося редагувати сеанс: {TryEdit.message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("Сеанс успішно редаговано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                    return;
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
