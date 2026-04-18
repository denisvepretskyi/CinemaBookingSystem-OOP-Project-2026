using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Interfaces
{
    public interface IJsonRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        void Save(List<T> items);
    }
}
