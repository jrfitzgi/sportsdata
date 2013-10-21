using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace SportsData.Facebook
{
    public class FacebookQuery
    {
        private const string baseAddress = "http://www.facebook.com/";
        private const string pageFormatString = "/{0}/likes";

        public static List<FacebookAccountSnapshot> GetSnapshots(List<FacebookAccount> accounts)
        {
            List<FacebookAccountSnapshot> results = new List<FacebookAccountSnapshot>();

            // Store the date so all records are stamped with the same date
            DateTime dateOfSnapshot = DateTime.UtcNow;

            foreach (FacebookAccount account in accounts)
            {
                FacebookAccountSnapshot accountSnapshot = FacebookQuery.GetSnapshot(account);
                if (null != accountSnapshot)
                {
                    accountSnapshot.DateOfSnapshot = dateOfSnapshot; // overwrite the date
                    results.Add(accountSnapshot);
                }
            }

            return results;
        }

        public static FacebookAccountSnapshot GetSnapshot(FacebookAccount account)
        {
            // Construct the url
            string relativeUrl = String.Format(FacebookQuery.pageFormatString, account.Id); // Eg. /torontomapleleafs/likes
            Uri url = new Uri(relativeUrl, UriKind.Relative);

            // Make an http request
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.69 Safari/537.36");
            //IE is User-Agent: Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)
            httpClient.BaseAddress = new Uri(FacebookQuery.baseAddress);

            Task<string> httpResponseMessage = httpClient.GetStringAsync(url);
            string responseString = httpResponseMessage.Result;

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseString);

            HtmlNode documentNode = document.DocumentNode;

            FacebookAccountSnapshot result = FacebookQuery.ParsePage(documentNode, account);

            result.DateOfSnapshot = DateTime.UtcNow;
            return result;
        }

        private static FacebookAccountSnapshot ParsePage(HtmlNode documentNode, FacebookAccount account)
        {
            FacebookAccountSnapshot accountSnapshot = new FacebookAccountSnapshot();
            accountSnapshot.FacebookAccountId = account.Id;

            if (null == documentNode)
            {
                return null;
            }

            string totalLikesXPath = @"//span[@class='timelineLikesBigNumber fsm']";
            HtmlNode totalLikes = documentNode.SelectSingleNode(totalLikesXPath);
            if (null == totalLikes)
            {
                accountSnapshot.TotalLikes = -1;
                accountSnapshot.Log += "Could not find totalLikes using " + totalLikesXPath + Environment.NewLine;
            }
            else
            {
                accountSnapshot.TotalLikes = int.Parse(totalLikes.InnerText, NumberStyles.AllowThousands);
            }


            return accountSnapshot;
        }
    }
}
