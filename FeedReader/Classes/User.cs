using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace FeedReader.Classes
{
    public static class UserInfo
    {
        public static Controller CurrentController { get; set; }
        public static bool IsLoggedIn { get; private set; }
        private static DataBase DB { get; set; }
        public static Models.User Info { get; private set; }

        public static bool LogIn(string username, string password)
        {
            if (DB == null) DB = DataBase.GetInstance();
            password = HashPassword(password);

            Models.User user = DB.Users.FirstOrDefault(u => u.Email == username);
            if (user == null || user.PasswordHash != password)
            {
                Thread.Sleep(Properties.Settings.Default.FailedLogInSleep);
                return IsLoggedIn = false;
            }

            CurrentController.Session["UserInfo"] = Info = user;
            return true;
        }

        public static bool LogIn()
        {
            if (DB == null) DB = DataBase.GetInstance();
            if (!CurrentController.Session.AsEnumerable().Any(i => i.Key == "UserInfo" && i.Value.GetType() == typeof(Models.User))) return false;
            Models.User userInfo = (Models.User)CurrentController.Session["UserInfo"];

            Models.User user = DB.Users.FirstOrDefault(u => u.Email == userInfo.Email);
            if (user == null || user.PasswordHash != userInfo.PasswordHash) return IsLoggedIn = false;

            CurrentController.Session["UserInfo"] = Info = user;
            return true;
        }

        public static void LogOut()
        {
            if (CurrentController.Session.AsEnumerable().Any(i => i.Key == "UserInfo")) CurrentController.Session.Remove("UserInfo");
            IsLoggedIn = false;
        }

        public static string HashPassword(string password)
        {
            password = Properties.Settings.Default.HashSeed + password;

            using (SHA1CryptoServiceProvider c = new SHA1CryptoServiceProvider())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(password);
                for (int i = 0; i < Properties.Settings.Default.HashTimes; i++) buffer = c.ComputeHash(buffer);
                return String.Join("", buffer.Select(i => (i < 16 ? "0" : "") + Convert.ToString(i, 16)));
            }
        }
    }
}