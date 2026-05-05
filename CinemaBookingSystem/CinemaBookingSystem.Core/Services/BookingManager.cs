using System;
using System.Collections.Generic;
using System.Text;
using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Repositories;
using CinemaBookingSystem.Core.Models;
using CinemaBookingSystem.Core.Data;

namespace CinemaBookingSystem.Core.Services
{
    public class BookingManager
    {
        private IOrderRepository _orderRepo;
        private IJsonRepository<Session> _sessionRepo;
        private IJsonRepository<Hall> _hallRepo;
        private IUserRepository _userRepo;
        public BookingManager
            (IOrderRepository orderRepo, 
            IJsonRepository<Session> sessionRepo,
            IJsonRepository<Hall> hallRepo,
            IUserRepository userRepo)
        {
            _orderRepo = orderRepo;
            _sessionRepo = sessionRepo;
            _hallRepo = hallRepo;
            _userRepo = userRepo;
        }


        // Створення замовлення
        public (bool IsSuccess, string Message) CreateOrder(int userId, int sessionId, List<(short row, short seat)> seats)
        {
            if (_userRepo.GetById(userId) == null) return (false, "Користувача не знайдено!");

            Session currentSession = _sessionRepo.GetById(sessionId);
            if (currentSession == null) return (false, "Сеанс не знайдено!");

            if (currentSession.StartTime < DateTime.Now) return (false, "Неможливо забронювати квитки на сеанс у минулому!");

            Hall currentHall = _hallRepo.GetById(currentSession.HallId);
            if (seats.Count > currentHall.RowCount * currentHall.ColumnCount) return (false, "Не можна купити квитків більше ніж місць у залі!");

            var takenSeats = GetTakenSeats(sessionId);
            foreach (var requestedSeat in seats)
            {
                if (requestedSeat.row <= 0 || requestedSeat.row > currentHall.RowCount ||
                    requestedSeat.seat <= 0 || requestedSeat.seat > currentHall.ColumnCount)
                    return (false, $"Некоректне місце: Ряд {requestedSeat.row}, Місце {requestedSeat.seat} не існує в цьому залі!");

                if (takenSeats.Contains((requestedSeat.row, requestedSeat.seat)))
                    return (false, $"Місце Ряд {requestedSeat.row}, Місце {requestedSeat.seat} вже зайнято іншим користувачем!");
            }
            int maxTicketId = 0;
            var allOrders = _orderRepo.GetAll();

            if (allOrders != null && allOrders.Any(o => o.Tickets != null && o.Tickets.Any()))
            {
                maxTicketId = allOrders.SelectMany(o => o.Tickets).Max(t => t.Id);
            }

            Order newOrder = new Order()
            {
                UserId = userId,
                Date = DateTime.Now,
                TotalPrice = 0
            };
            _orderRepo.Add(newOrder);

            foreach (var seat in seats)
            {
                maxTicketId++;

                Ticket newTicket = new Ticket()
                {
                    Id = maxTicketId, // Встановлюємо згенерований Id
                    SessionId = sessionId,
                    Row = seat.row,
                    Column = seat.seat,
                    Price = currentSession.Price,
                    OrderId = newOrder.Id
                };

                newOrder.TotalPrice += newTicket.Price;
                newOrder.Tickets.Add(newTicket);

            }
            _orderRepo.Update(newOrder);
            return (true, "Бронювання успішно створено!");
        }


        // Повернення квитка
        public (bool IsSuccess, string Message) ReturnTicket(int orderId, int ticketId)
        {
            Order order = _orderRepo.GetById(orderId);
            if (order == null) return (false, "Замовлення не знайдено!");
            Ticket ticketToCancel = order.Tickets.Find(t => t.Id == ticketId);
            if (ticketToCancel == null) return (false, "Квиток не знайдено!");
            var sessionToCancel = _sessionRepo.GetById(ticketToCancel.SessionId);
            if(sessionToCancel.StartTime.AddMinutes(-30) <= DateTime.Now ) return (false, "Не можна повертати квиток за 30 хв. до початку сеансу!");
            order.Tickets.Remove(ticketToCancel);
            order.TotalPrice -= ticketToCancel.Price;
            if(order.Tickets.Count == 0)            
                _orderRepo.Delete(orderId);
            else
                _orderRepo.Update(order);
            return (true, "Квиток успішно повернуто!");
        }


        // Повертає список зайнятіх місць
        public List<(short Row, short Seat)> GetTakenSeats(int sessionId)
        {
            return _orderRepo.GetAll()
                .SelectMany(order => order.Tickets) 
                .Where(ticket => ticket.SessionId == sessionId) 
                .Select(ticket => (ticket.Row, ticket.Column)) 
                .ToList();
        }
    }
}
