using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FeedReader.Models;
using System.Reflection;

namespace FeedReader.Classes
{
    public class DataBase : DBDataContext
    {
        private static DataBase Instance = null;

        private DataBase() : base(Properties.Settings.Default.ConnectionString) { }

        public static DataBase GetInstance()
        {
            if (Instance == null) Instance = new DataBase();
            return Instance;
        }

        public void ClearInternalCache()
        {
            var method = this.GetType().GetMethod("ClearCache", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            method.Invoke(this, null);
        }
    }
}