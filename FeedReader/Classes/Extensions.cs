using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Diagnostics.Contracts;

namespace FeedReader.Classes
{
    public static class Extensions
    {
        public static IEnumerable<KeyValuePair<string, object>> AsEnumerable(this HttpSessionStateBase s)
        {
            Contract.Requires(s != null);

            for (int i = 0; i < s.Keys.Count; i++)
            {
                yield return new KeyValuePair<string, object>(s.Keys[i], s[s.Keys[i]]);
            }
        }
    }
}