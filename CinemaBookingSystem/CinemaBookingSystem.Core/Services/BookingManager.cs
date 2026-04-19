using System;
using System.Collections.Generic;
using System.Text;
using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Repositories;
using CinemaBookingSystem.Core.Models;

namespace CinemaBookingSystem.Core.Services
{
    public class BookingManager
    {
        private IJsonRepository<Order> _orderRepo;
        private IJsonRepository<Session> _sessionRepo;
        private IJsonRepository<Hall> _hallRepo;
        public BookingManager(IJsonRepository<Order> orderRepo, IJsonRepository<Session> sessionRepo)
        {
            _orderRepo = orderRepo;
            _sessionRepo = sessionRepo;
        }

        public (bool, string) CreateOrder(int userId, int sessionId, List<(short row, short seat)> seats)
        {
            Session currentSession = _sessionRepo.GetById(sessionId);
            Hall currentHall = _hallRepo.GetById(currentSession.HallId);
            if (currentSession == null) return (false, "Сеанс не знайдено!");
            if(currentSession.StartTime > DateTime.Now) return (false, "Дата не може бути в минулому часі!");
            if (seats.Count > currentHall.RowCount * currentHall.ColumnCount) return (false, "Не можна купити квитків більше ніж місць у залі!");
            Order newOrder = new Order()
            {
                UserId = userId,
                Date = DateTime.Now,
                TotalPrice = 0,
                IsCancelled = false
            };

            foreach(var seat in seats)
            {
                Ticket newTicket = new Ticket()
                {
                    SessionId = sessionId,
                    Row = seat.row,
                    Column = seat.seat,
                    Price = currentSession.Price
                };

                newOrder.TotalPrice += newTicket.Price;
                newOrder.Tickets.Add(newTicket);
            }
            _orderRepo.Add(newOrder);
            return (true, "");
        }

        public (bool, string) ReturnTicket(int orderId, int row, int column)
        {
            Order order = _orderRepo.GetById(orderId);
            if (order == null) return (false, "Замовлення не знайдено!");
            Ticket ticketToCancel = order.Tickets.Find(t => t.Row == row && t.Column == column);
            if (ticketToCancel == null) return (false, "Квиток не знайдено!");
            var sessionToCancel = _sessionRepo.GetById(ticketToCancel.SessionId);
            if(sessionToCancel.StartTime.AddMinutes(-30) <= DateTime.Now ) return (false, "Не можна повертати квиток за 30 хв. до початку сеансу!");
            order.Tickets.Remove(ticketToCancel);
            order.TotalPrice -= ticketToCancel.Price;
            if(order.Tickets.Count == 0)            
                _orderRepo.Delete(orderId);
            else
                _orderRepo.Update(order);
            return (true, "");
        }
    }
}
