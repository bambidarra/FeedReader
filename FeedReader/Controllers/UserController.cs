using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeedReader.Classes;

namespace FeedReader.Controllers
{
    public class UserController : Controller
    {
        public UserController()
        {
            UserInfo.CurrentController = this;
        }

        public DataBase DB { get; set; }

        public ActionResult Register()
        {
            if (UserInfo.LogIn()) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult RegisterSubmit(string email, string password, string passwordAgain)
        {
            List<string> errors;
            if (Validator.IsValidUser(email, password, passwordAgain, out errors))
            {
                DB = DataBase.GetInstance();
                DB.Users.InsertOnSubmit(new Models.User { Email = email, PasswordHash = Classes.UserInfo.HashPassword(password), Registered = DateTime.Now });
                DB.SubmitChanges();
                UserInfo.LogIn(email, password);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Login()
        {
            if (UserInfo.LogIn()) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult LoginSubmit(string email, string password)
        {
            UserInfo.LogIn(email, password);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            UserInfo.LogOut();
            return RedirectToAction("Index", "Home");
        }
    }
}