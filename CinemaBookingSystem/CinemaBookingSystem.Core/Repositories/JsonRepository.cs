using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using CinemaBookingSystem.Core.Interfaces;
using CinemaBookingSystem.Core.Models;


namespace CinemaBookingSystem.Core.Repositories
{
    internal class JsonRepository<T> : IJsonRepository<T> where T : BaseEntity
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;        

        public JsonRepository(string filePath)
        {
            _filePath = filePath;
            _jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        }

        public IEnumerable<T> GetAll()
        {
            if(!File.Exists(_filePath)) return new List<T>();            
            string json = File.ReadAllText(_filePath);
            if(string.IsNullOrWhiteSpace(json)) return new List<T>();
            return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
        }

        public T GetById(int id)
        {
            List<T> items = GetAll().ToList();
            return items.FirstOrDefault(i => i.Id == id);
        }

        public void Add(T entity)
        {
            List<T> items = GetAll().ToList();
            entity.Id = items.Count > 0 ? items.Max(i => i.Id) + 1 : 1;
            items.Add(entity);
            Save(items);
        }

        public void Update(T entity)
        {
            List<T> items = GetAll().ToList();
            int index = items.FindIndex(item => item.Id == entity.Id);
            if(index == -1) return;
            items[index] = entity;
            Save(items);
        }

        public void Delete(int id)
        {
            List<T> items = GetAll().ToList();
            int index = items.FindIndex(item => item.Id == id);
            if(index == -1) return;
            items.RemoveAt(index);
            Save(items);
        }

        public void Save(List<T> items)
        {
            string json = JsonSerializer.Serialize(items, _jsonOptions);
            File.WriteAllText(_filePath, json);
        }
    }
}
