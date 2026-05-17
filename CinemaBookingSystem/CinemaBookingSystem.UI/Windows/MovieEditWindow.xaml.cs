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
using System.Windows.Shapes;

namespace CinemaBookingSystem.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для MovieEditWindow.xaml
    /// </summary>
    public partial class MovieEditWindow : Window
    {
        // Властивість, яка зберігатиме наш фільм
        public Movie CurrentMovie { get; private set; } = null;
        public List<Genre> SelectedGenres { get; private set; } = new List<Genre>();

        public delegate void MovieTableUpdater();
        public event MovieTableUpdater OnMovieAdded;



        // Конструктор
        public MovieEditWindow()
        {
            InitializeComponent();
            GenresListBox.ItemsSource = Enum.GetValues(typeof(Genre));               
            this.Title = "Додати новий фільм";            
        }

        public MovieEditWindow(Movie movie)
        {
            InitializeComponent();
            GenresListBox.ItemsSource = Enum.GetValues(typeof(Genre));
            this.Title = "Редагувати фільм";
            CurrentMovie = movie;
            if (CurrentMovie.Genres != null)
            {
                foreach (var genre in CurrentMovie.Genres)
                {
                    GenresListBox.SelectedItems.Add(genre);
                }
                this.DataContext = CurrentMovie;
            }
        }

        

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            int duration = 0;
            int.TryParse(DurationTextBox.Text, out duration);
            if (CurrentMovie == null)
            { 
                var TryAdd = AppServices.MovieService.AddMovie(
                    TitleTextBox.Text,
                    DescriptionTextBox.Text,
                    duration,
                    DirectorTextBox.Text,
                    new List<Genre>(GenresListBox.SelectedItems.Cast<Genre>()),
                    PosterPathTextBox.Text
                );

                if (TryAdd.isValid)
                {
                    MessageBox.Show("Фільм успішно додано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    OnMovieAdded?.Invoke(); 
                }
                else
                    MessageBox.Show($"Помилка при додаванні фільму: {TryAdd.message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                                   
            }

            else
            {

                var TryEdit = AppServices.MovieService.EditMovie(
                    CurrentMovie.Id,
                    TitleTextBox.Text,
                    DescriptionTextBox.Text,
                    duration,
                    DirectorTextBox.Text,
                    new List<Genre>(GenresListBox.SelectedItems.Cast<Genre>()),
                    PosterPathTextBox.Text
                );

                if (TryEdit.isValid)
                    MessageBox.Show("Фільм успішно редаговано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);

                else
                    MessageBox.Show($"Помилка при редагуванні фільму: {TryEdit.message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                

            }
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Закриваємо вікно без збереження (DialogResult = false)
            this.Close();
        }
    }
}
