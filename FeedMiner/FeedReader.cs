using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Net;

namespace FeedMiner
{
    /// <summary>
    /// A feed parser.
    /// </summary>
    public static class FeedParser
    {
        public static IEnumerable<FeedItem> Parse(Stream stream) { return FeedParser.Parse(stream, null); }
        public static IEnumerable<FeedItem> Parse(Stream stream, Encoding encoding)
        {
            if (encoding == null) encoding = Encoding.Default;
            XDocument feed = XDocument.Load(new StreamReader(stream, encoding));

            foreach (XElement item in feed.Root.Element("channel").Elements("item"))
            {
                FeedItem fi;

                try
                {
                    fi = new FeedItem
                    {
                        Title = item.Element("title").Value,
                        Link = new Uri(item.Element("link").Value),
                        Description = item.Element("description").Value,
                        PublicationDate = DateTime.Parse(item.Element("pubDate").Value.Replace("EST", "-5").Replace("+0000", "GMT"))
                    };
                }
                catch { continue; }
                yield return fi;
            }
        }

        public static IEnumerable<FeedItem> Parse(string path) { return FeedParser.Parse(path, null); }
        public static IEnumerable<FeedItem> Parse(string path, Encoding encoding)
        {
            return FeedParser.Parse(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), encoding);
        }

        public static IEnumerable<FeedItem> Parse(Uri path) { return FeedParser.Parse(path, null); }
        public static IEnumerable<FeedItem> Parse(Uri path, Encoding encoding)
        {
            WebRequest req = (WebRequest)HttpWebRequest.Create(path);
            WebResponse res = req.GetResponse();
            return FeedParser.Parse(res.GetResponseStream(), encoding);
        }
    }
}