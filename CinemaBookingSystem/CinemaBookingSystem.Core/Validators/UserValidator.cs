using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Validators
{
    public static class UserValidator
    {
        public static (bool IsValid, string Message) IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return (false, "Номер телефона не може бути порожнім!");

            if (!phoneNumber.StartsWith("+380") && !phoneNumber.StartsWith("0"))
                return (false, "Коректний номер має починатися з '+380' або '0'.");

            if (phoneNumber.StartsWith("+380"))
            {
                if (phoneNumber.Length != 13) return (false, "Номер у форматі +380 має містити 13 символів.");            
                if (!phoneNumber.Skip(1).All(char.IsDigit)) return (false, "Номер має містити лише цифри.");
            }

            if (phoneNumber.StartsWith("0"))
            {
                if (phoneNumber.Length != 10) return (false, "Номер у форматі 0XX має містити 10 цифр.");
                if (!phoneNumber.All(char.IsDigit)) return (false, "Номер має містити лише цифри.");
            }

            return (true, string.Empty);
        }

        public static (bool IsValid, string Message) IsValidPassword(string password)
        {
            if(string.IsNullOrEmpty(password)) return (false, "Пароль не може бути порожнім!");
            if(password.Length < 8) return (false, "Пароль повинен містити щонайменше 8 символів!");
            if(!password.Any(char.IsUpper)) return (false, "Пароль повинен містити щонайменше одну велику літеру!");
            if(!password.Any(char.IsLower)) return (false, "Пароль повинен містити щонайменше одну малу літеру!");
            if(!password.Any(char.IsDigit)) return (false, "Пароль повинен містити щонайменше одну цифру!");
            return (true, "");
        }
    }
}
