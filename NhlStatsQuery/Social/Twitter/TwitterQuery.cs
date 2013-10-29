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
    public class TwitterQuery : SocialBaseQuery
    {
        public TwitterQuery()
        {
            this.BaseAddress = "https://twitter.com/";
            this.PageFormatString = "/{0}"; // baseAddress + formatString = https://twitter.com/MapleLeafs
        }

        public static List<TwitterSnapshot> GetTwitterSnapshots(List<TwitterAccount> twitterAccounts)
        {
            return (new TwitterQuery()).GetSnapshots<TwitterSnapshot,TwitterAccount>(twitterAccounts);
        }

        public static TwitterSnapshot GetTwitterSnapshot(TwitterAccount twitterAccount)
        {
            return (new TwitterQuery()).GetSnapshot(twitterAccount) as TwitterSnapshot;
        }

        protected override SocialBaseSnapshot ParsePage(HtmlNode documentNode, SocialBaseAccount account)
        {
            TwitterSnapshot twitterAccountSnapshot = new TwitterSnapshot();
            twitterAccountSnapshot.TwitterAccountId = account.Id;

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
