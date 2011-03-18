using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeedReader.Classes;
using FeedReader.ViewModels;

namespace FeedReader.Controllers
{
    public class ApiController : Controller
    {
        [HttpPost]
        public ActionResult GetFeeds(string username, string password, int? page)
        {
            if (!UserInfo.LogIn(username, password))
            {
                Response.Write("Login Failed");
                return null;
            }

            if (page <= 0) page = null;
            DataBase DB = DataBase.GetInstance();
            DB.ClearInternalCache();

            var q = (from feed in UserInfo.Info.UserFeeds
                     from feedItem in feed.Feed.FeedItems
                     where !UserInfo.Info.UserFeedItems.Any(f => f.FeedItemID == feedItem.ID && f.Read)
                     select feedItem);

            return View(new ViewFeedsViewModel { CurrentPage = page ?? 1, FeedItems = q.OrderByDescending(f => f.Created).Skip(page.HasValue ? Math.Abs((page.Value - 1) * Properties.Settings.Default.PageLength) : 0).Take(Properties.Settings.Default.PageLength), Pages = (int)Math.Ceiling(q.Count() / (double)Properties.Settings.Default.PageLength) });
        }
    }
}
