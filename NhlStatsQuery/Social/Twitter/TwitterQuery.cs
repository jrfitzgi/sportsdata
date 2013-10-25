using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace SportsData.Social
{
    public class TwitterQuery
    {
        private const string baseAddress = "https://twitter.com/";
        private const string pageFormatString = "/{0}"; // baseAddress + formatString = https://twitter.com/MapleLeafs

        public static List<TwitterSnapshot> GetTwitterSnapshots(List<TwitterAccount> twitterAccounts)
        {
            List<TwitterSnapshot> results = new List<TwitterSnapshot>();

            // Store the date so all records are stamped with the same date
            DateTime dateOfSnapshot = DateTime.UtcNow;

            foreach (TwitterAccount twitterAccount in twitterAccounts)
            {
                TwitterSnapshot twitterAccountSnapshot = TwitterQuery.GetTwitterSnapshot(twitterAccount);
                if (null != twitterAccountSnapshot)
                {
                    twitterAccountSnapshot.DateOfSnapshot = dateOfSnapshot; // overwrite the date
                    results.Add(twitterAccountSnapshot);
                }
            }

            return results;
        }

        public static TwitterSnapshot GetTwitterSnapshot(TwitterAccount twitterAccount)
        {
            // Construct the url
            string relativeUrl = String.Format(TwitterQuery.pageFormatString, twitterAccount.Id); // Eg. /MapleLeafs
            Uri url = new Uri(relativeUrl, UriKind.Relative);

            // Make an http request
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(TwitterQuery.baseAddress);

            Task<string> httpResponseMessage = httpClient.GetStringAsync(url);
            string responseString = httpResponseMessage.Result;

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseString);

            HtmlNode documentNode = document.DocumentNode;

            TwitterSnapshot result = TwitterQuery.ParsePage(documentNode, twitterAccount);

            result.DateOfSnapshot = DateTime.UtcNow;
            return result;
        }

        private static TwitterSnapshot ParsePage(HtmlNode documentNode, TwitterAccount twitterAccount)
        {
            TwitterSnapshot twitterAccountSnapshot = new TwitterSnapshot();
            twitterAccountSnapshot.TwitterAccountId = twitterAccount.Id;

            if (null == documentNode)
            {
                return null;
            }

            string tweetCountXPath = @"//a[@data-element-term='tweet_stats']/strong";
            HtmlNode tweetCount = documentNode.SelectSingleNode(tweetCountXPath);
            if (null == tweetCount)
            {
                twitterAccountSnapshot.Tweets = -1;
                twitterAccountSnapshot.Log += "Could not find tweetCount using " + tweetCountXPath + Environment.NewLine;
            }
            else
            {
                twitterAccountSnapshot.Tweets = int.Parse(tweetCount.InnerText, NumberStyles.AllowThousands);
            }

            string followingCountXPath = @"//a[@data-element-term='following_stats']/strong";
            HtmlNode followingCount = documentNode.SelectSingleNode(followingCountXPath);
            if (null == followingCount)
            {
                twitterAccountSnapshot.Following = -1;
                twitterAccountSnapshot.Log += "Could not find followingCount using " + followingCountXPath + Environment.NewLine;
            }
            else
            {
                twitterAccountSnapshot.Following = int.Parse(followingCount.InnerText, NumberStyles.AllowThousands);
            }


            string followerCountXPath = @"//a[@data-element-term='follower_stats']/strong";
            HtmlNode followerCount = documentNode.SelectSingleNode(followerCountXPath);
            if (null == followerCount)
            {
                twitterAccountSnapshot.Followers = -1;
                twitterAccountSnapshot.Log += "Could not find followerCount using " + followerCountXPath + Environment.NewLine;
            }
            else
            {
                twitterAccountSnapshot.Followers = int.Parse(followerCount.InnerText, NumberStyles.AllowThousands);
            }

            return twitterAccountSnapshot;
        }
    }
}
