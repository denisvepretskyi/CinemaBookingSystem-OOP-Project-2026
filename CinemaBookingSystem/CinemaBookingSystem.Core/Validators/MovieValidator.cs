using System;
using System.Collections.Generic;
using System.Text;
using CinemaBookingSystem.Core.Enums;

namespace CinemaBookingSystem.Core.Validators
{
    public static class MovieValidator
    {
        public static (bool IsValid, string Message) IsValidMovie(string title, string description, int duration, string director, List<Genre> genres)
        {
            if (string.IsNullOrEmpty(title)) return (false, "Назва не може бути порожньою!");
            if (!string.IsNullOrWhiteSpace(description) && (description.Length < 100 || description.Length > 1000)) return (false, "Опис має бути від 100 до 1000 символів!");
            if (duration <= 0) return (false, "Тривалість має бути більшою за нуль!");
            if(genres == null || genres.Count == 0) return (false, "Потрібно вказати хоча б один жанр!");
            if (!string.IsNullOrWhiteSpace(director) && director.Length > 100) return (false, "Ім'я режисера не може перевищувати 100 символів!");
                        
            foreach(var genre in genres) if(!Enum.IsDefined(typeof(Genre), genre)) return (false, "Невірний жанр!");
            return (true, string.Empty);
        }
    }
}
