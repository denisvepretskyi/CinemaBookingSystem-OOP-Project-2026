using CinemaBookingSystem.Core.Data;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Services;
using CinemaBookingSystem.Core.Validators;
using CinemaBookingSystem.UI.DisplayItems;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using CinemaBookingSystem.UI.DisplayItems;
namespace CinemaBookingSystem.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {       

        public ProfilePage()
        {
            InitializeComponent();
            LoadUserData();
            LoadDisplayTickets();
        }

        private void LoadUserData()
        {
            if (AppServices.AuthorizationService.currentUser != null)
            {
                NameTextBox.Text = AppServices.AuthorizationService.currentUser.Name;
                PhoneTextBox.Text = AppServices.AuthorizationService.currentUser.PhoneNumber;
                PasswordBox.Password = AppServices.AuthorizationService.currentUser.Password;
            }
        }

        private void LoadDisplayTickets()
        {
            List<Order> orders = new List<Order>();
            List<Ticket> tickets = new List<Ticket>();
            List<TicketModel> ticketDisplayItems = new List<TicketModel>();

            if (AppServices.AuthorizationService.currentUser != null)
            {
                orders = AppData.Orders.GetByUserId(AppServices.AuthorizationService.currentUser.Id);
                tickets = orders.SelectMany(o => o.Tickets).ToList();
                foreach (Ticket ticket in tickets)
                {
                    TicketModel displayTicket = new TicketModel()
                    {
                        Id = ticket.Id,
                        OrderId = ticket.OrderId,
                        Row = ticket.Row,
                        Column = ticket.Column,
                        MovieTitle = AppData.Movies.GetById(AppData.Sessions.GetById(ticket.SessionId).MovieId).Title,
                        CinemaName = AppData.Cinemas.GetById(AppData.Sessions.GetById(ticket.SessionId).CinemaId).Name,
                        StartTime = AppData.Sessions.GetById(ticket.SessionId).StartTime
                    };
                    ticketDisplayItems.Add(displayTicket);
                }
            }
            TicketsItemsControl.ItemsSource = ticketDisplayItems;
        }



        //private void LoadDisplayTickets()
        //{
        //    List<Order> orders = new List<Order>();
        //    List<Ticket> tickets = new List<Ticket>();
        //    List<TicketModel> ticketDisplayItems = new List<TicketModel>();

        //    if (AppServices.AuthorizationService.currentUser is Customer customer)
        //    {
        
        //            orders = customer.Orders;
        //            tickets = orders.SelectMany(o => o.Tickets).ToList();
        //            foreach (Ticket ticket in tickets)
        //            {
        //                TicketModel displayTicket = new TicketModel()
        //                {
        //                    Id = ticket.Id,
        //                    OrderId = ticket.OrderId,
        //                    Row = ticket.Row,
        //                    Column = ticket.Column,
        //                    MovieTitle = AppData.Movies.GetById(AppData.Sessions.GetById(ticket.SessionId).MovieId).Title,
        //                    CinemaName = AppData.Cinemas.GetById(AppData.Sessions.GetById(ticket.SessionId).CinemaId).Name,
        //                    StartTime = AppData.Sessions.GetById(ticket.SessionId).StartTime
        //                };
        //                ticketDisplayItems.Add(displayTicket);
        //            }                    
        //    }
        //    TicketsItemsControl.ItemsSource = ticketDisplayItems;
        //}



        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppServices.AuthorizationService.currentUser.PhoneNumber != PhoneTextBox.Text)
            {
                var isPhoneExists = AppData.Users.IsPhoneExists(PhoneTextBox.Text);
                if (isPhoneExists)
                {
                    MessageBox.Show("Цей номер телефону вже використовується!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            var numberValidation = UserValidator.IsValidPhoneNumber(PhoneTextBox.Text);
            if(!numberValidation.IsValid)
            {
                MessageBox.Show(numberValidation.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var passwordValidation = UserValidator.IsValidPassword(PasswordBox.Password);
            if (!passwordValidation.IsValid)
            {
                MessageBox.Show(passwordValidation.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            AppServices.AuthorizationService.currentUser.Name = NameTextBox.Text;
            AppServices.AuthorizationService.currentUser.PhoneNumber = PhoneTextBox.Text;
            AppServices.AuthorizationService.currentUser.Password = PasswordBox.Password;
            AppData.Users.Update(AppServices.AuthorizationService.currentUser);
            MessageBox.Show("Дані успішно збережено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        public void ReturnTicket_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.DataContext is TicketModel ticket)
            {
                var result = AppServices.BookingService.ReturnTicket(ticket.OrderId, ticket.Id);
                if (result.IsSuccess)
                {
                    MessageBox.Show("Квиток успішно повернуто!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadDisplayTickets();
                }
                else
                {
                    MessageBox.Show(result.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            AppServices.AuthorizationService.Logout();
            NavigationService.Navigate(new LoginPage());
        }
    }
}
