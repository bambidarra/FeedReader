using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FeedReader.Classes;
using FeedReader.Models;
using System.Net;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace FeedMiner
{
    class Program
    {
        private static DataBase DB;

        static string PrepareString(string s)
        {
            return Regex.Replace(s.Replace("&amp;", "&").Replace("&quot;", "\""), "<[a-zA-Z0-9]+[^>]*>", "");
        }

        static void Main(string[] args)
        {
            DB = DataBase.GetInstance();

            while (true)
            {
                foreach (var f in DB.Feeds.Where(i => i.LastChecked.AddMinutes(15) < DateTime.Now).AsParallel())
                {
                    try
                    {
                        foreach (var fi in FeedParser.Parse(new Uri(f.Url)))
                        {
                            try
                            {
                                FeedReader.Models.FeedItem nFeedItem = new FeedReader.Models.FeedItem
                                {
                                    FeedID = f.ID,
                                    Created = fi.PublicationDate,
                                    Description = PrepareString(fi.Description),
                                    Title = PrepareString(fi.Title),
                                    Url = fi.Link.ToString()
                                };

                                if (!DB.FeedItems.Any(i => i.Url == nFeedItem.Url && i.FeedID == f.ID))
                                {
                                    DB.FeedItems.InsertOnSubmit(nFeedItem);
                                    Console.WriteLine("{0}: {1}", f.Title, nFeedItem.Title);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error occurred inserting {0}.", fi.Title.TrimEnd('.'));
                                Console.WriteLine(e);
                            }
                        }

                        f.LastChecked = DateTime.Now;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to download or parse {0}.", f.Title);
                        Console.WriteLine(e);
                    }
                }


                try
                {
                    DB.SubmitChanges();
                    Thread.Sleep(30 * 1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error inserting feeds:");
                    Console.WriteLine(e.Message);
                    Thread.Sleep(10 * 1000);
                }

                DB.ClearInternalCache();
            }
        }
    }
}