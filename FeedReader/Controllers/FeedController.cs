using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeedReader.Classes;
using FeedReader.ViewModels;
using FeedReader.Models;
using System.Data.Linq;

namespace FeedReader.Controllers
{
    public class FeedController : Controller
    {
        public DataBase DB { get; set; }

        public FeedController()
        {
            UserInfo.CurrentController = this;
        }

        public ActionResult View(int? page)
        {
            if (!UserInfo.LogIn()) return RedirectToAction("Login", "User");
            if (page <= 0) page = null;
            this.DB = DataBase.GetInstance();
            this.DB.ClearInternalCache();

            var q = (from feed in UserInfo.Info.UserFeeds
                     from feedItem in feed.Feed.FeedItems
                     where !UserInfo.Info.UserFeedItems.Any(f => f.FeedItemID == feedItem.ID && f.Read)
                     select feedItem);

            return View(new ViewFeedsViewModel { CurrentPage = page ?? 1, FeedItems = q.OrderByDescending(f => f.Created).Skip(page.HasValue ? Math.Abs((page.Value - 1) * Properties.Settings.Default.PageLength) : 0).Take(Properties.Settings.Default.PageLength), Pages = (int)Math.Ceiling(q.Count() / (double)Properties.Settings.Default.PageLength) });
        }

        public ActionResult Open(int? id)
        {
            if (!id.HasValue || !UserInfo.LogIn()) return RedirectToAction("Login", "User");
            this.DB = DataBase.GetInstance();
            this.DB.ClearInternalCache();
            FeedItem feedItem = this.DB.FeedItems.FirstOrDefault(i => i.ID == id.Value);
            if (feedItem == null) return RedirectToAction("Login", "User");
            UserFeedItem ufi = UserInfo.Info.UserFeedItems.FirstOrDefault(i => i.FeedItemID == id.Value);
            if (ufi != null) ufi.Read = true;
            else this.DB.UserFeedItems.InsertOnSubmit(new UserFeedItem { FeedItemID = id.Value, UserID = UserInfo.Info.ID, Read = true });
            this.DB.SubmitChanges();
            return Redirect(feedItem.Url);
        }
    }
}