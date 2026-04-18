using System;
using System.Collections.Generic;
using System.Text;
using CinemaBookingSystem.Core.Enums;

namespace CinemaBookingSystem.Core.Validators
{
    public static class MovieValidator
    {
        public static (bool isValid, string error) IsValidMovie(string title, string description, int duration, string director, Genre genre)
        {
            if (string.IsNullOrEmpty(title)) return (false, "Назва не може бути порожньою!");
            if (description.Length < 100 || description.Length > 1000) return (false, "Опис має бути від 100 до 1000 символів!");
            if (duration <= 0) return (false, "Тривалість має бути більше нуля!");
            if (string.IsNullOrEmpty(director)) return (false, "Режисер не може бути порожнім!");
            if(!Enum.IsDefined(typeof(Genre), genre)) return (false, "Невірний жанр!");
            return (true, string.Empty);
        }
    }
}
