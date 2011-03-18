using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeedMiner
{
    public struct FeedItem
    {
        public string Title { get; set; }
        public Uri Link { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}