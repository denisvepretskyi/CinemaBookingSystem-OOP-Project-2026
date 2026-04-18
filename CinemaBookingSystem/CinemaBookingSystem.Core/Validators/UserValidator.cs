using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaBookingSystem.Core.Validators
{
    public static class UserValidator
    {
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if(string.IsNullOrEmpty(phoneNumber)) return false;

            if(phoneNumber.StartsWith("+380") && phoneNumber.Length == 13)
                return phoneNumber.Skip(1).Any(char.IsDigit);

            if(phoneNumber.StartsWith("0") && phoneNumber.Length == 10)
                return phoneNumber.Any(char.IsDigit); 

            return false;
        }

        public static bool IsValidPassword(string password)
        {
            if(string.IsNullOrEmpty(password)) return false;
            if(password.Length < 8) return false;
            if(!password.Any(char.IsUpper)) return false;
            if(!password.Any(char.IsLower)) return false;
            if(!password.Any(char.IsDigit)) return false;
            return true;
        }
    }
}
