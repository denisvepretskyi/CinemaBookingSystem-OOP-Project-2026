using CinemaBookingSystem.Core.Data;
using CinemaBookingSystem.Core.Enums;
using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Repositories;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CinemaBookingSystem.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigationCommands.BrowseBack.InputGestures.Clear();
            MainFrame.Navigate(new LoginPage());
            var cinemas = AppData.Cinemas.GetAll();
            CinemaComboBox.ItemsSource = cinemas;
            LoadGenresIntoMenu();       
        }

        // Фільтрування сеансів зв кінотеатром
        private void CinemaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Отримуємо об'єкт кінотеатру, який обрав користувач
            if (CinemaComboBox.SelectedItem is Cinema selectedCinema)
            {
                MainFrame.Navigate(new CatalogPage(selectedCinema));
            }
            CinemaPlaceholder.Visibility = Visibility.Collapsed;


            // Скидання вибору жанру при зміні кінотеатру
            if (GenreContextMenu != null)
            {
                foreach (var item in GenreContextMenu.Items)
                {
                    if (item is MenuItem menuItem)
                    {
                        if (menuItem.Tag != null && menuItem.Tag.Equals(-1))
                            menuItem.IsChecked = true;
                        else
                            menuItem.IsChecked = false;
                    }
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text;
            if (MainFrame.Content is CatalogPage currentPage)
            {
                currentPage.SearchMovies(searchText);
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.ContextMenu != null)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.IsOpen = true;
            }
        }

        private void GenreMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;
            if (clickedItem != null)
            {
                // Отримуємо доступ до всього меню, в якому лежить цей пункт
                if (clickedItem.Parent is ContextMenu parentMenu)
                {
                    // Перебираємо всі пункти меню і прибираємо з них галочки
                    foreach (var item in parentMenu.Items)
                    {
                        if (item is MenuItem menuItem) menuItem.IsChecked = false;                        
                    }
                }                
                clickedItem.IsChecked = true;
                // Витягуємо ID жанру
                int selectedGenreId = Convert.ToInt32(clickedItem.Tag);

                //Оновлення каталогу на сторінці
                if (MainFrame.Content is CatalogPage currentPage)
                {
                    currentPage.FilterMoviesByGenre(selectedGenreId);
                }
            }
        }

        private void LoadGenresIntoMenu()
        {
            MenuItem allGenresItem = new MenuItem();
            allGenresItem.Header = "Усі жанри";
            allGenresItem.Tag = -1;
            allGenresItem.IsCheckable = true;
            allGenresItem.IsChecked = true;

            allGenresItem.Click += GenreMenuItem_Click;

            GenreContextMenu.Items.Add(allGenresItem);
            GenreContextMenu.Items.Add(new Separator());

            foreach (var genre in Enum.GetValues(typeof(Genre))) 
            {
                MenuItem item = new MenuItem();
                item.Header = genre.ToString();
                item.Tag = (int)genre;

                // Дозволяємо галочку для кожного згенерованого жанру
                item.IsCheckable = true;
                item.Click += GenreMenuItem_Click;
                GenreContextMenu.Items.Add(item);
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is MovieDetailsPage || e.Content is BookingPage || e.Content is ProfilePage)
            {
                TopMenu.Visibility = Visibility.Collapsed;
                GlobalBackButton.Visibility = Visibility.Visible;
            }
            if (e.Content is LoginPage)
            {
                TopMenu.Visibility = Visibility.Collapsed;
                GlobalBackButton.Visibility = Visibility.Collapsed;
                ProfileButton.Visibility = Visibility.Collapsed;
            }
            if (e.Content is RegisterPage)
            {
                TopMenu.Visibility = Visibility.Collapsed;
                GlobalBackButton.Visibility = Visibility.Visible;
                ProfileButton.Visibility = Visibility.Collapsed;
            }
            if (e.Content is SelectCinemaPage || e.Content is CatalogPage) 
            {
                TopMenu.Visibility = Visibility.Visible;
                GlobalBackButton.Visibility = Visibility.Collapsed;
                ProfileButton.Visibility = Visibility.Visible;
            }
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if(AppServices.AuthorizationService.currentUser == null)
            {
                MainFrame.Navigate(new LoginPage());
            }
            else MainFrame.Navigate(new ProfilePage());
        }

        private void GlobalBackButton_Click(Object sender, RoutedEventArgs e)
        {
            if (MainFrame.NavigationService.CanGoBack)
            {
                MainFrame.NavigationService.GoBack();
            }
        }
    }
}
