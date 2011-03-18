using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeedReader.ViewModels
{
    public class ViewFeedsViewModel
    {
        public IEnumerable<Models.FeedItem> FeedItems { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}