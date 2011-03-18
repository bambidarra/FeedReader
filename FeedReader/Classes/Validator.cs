using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace FeedReader.Classes
{
    public static class Validator
    {
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$");
        }

        public static bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^([a-zA-Z0-9\.\-_/\+\*,@!#\$%&\(\)=]){6,25}$");
        }

        public static bool IsValidUser(string email, string password, string passwordAgain, out List<string> errors)
        {
            errors = new List<string>();
            if (!IsValidEmail(email)) errors.Add(Properties.Resources.NotAValidEmailAddress);
            if (!IsValidPassword(password)) errors.Add(Properties.Resources.NotAValidPassword);
            if (password != passwordAgain) errors.Add(Properties.Resources.PasswordsDonTMatch);
            if (!errors.Any())
            {
                DataBase db = DataBase.GetInstance();
                if (db.Users.Any(u => u.Email == email)) errors.Add(Properties.Resources.EmailIsTaken);
            }
            return !errors.Any();
        }
    }
}