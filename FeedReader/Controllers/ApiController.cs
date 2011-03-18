using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FeedReader.Controllers
{
    public class ApiController : Controller
    {
        [HttpPost]
        public ActionResult GetFeeds(string username, string password, int? page)
        {
            return View();
        }
    }
}
