﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;
using SportsData.Models;

namespace SportsData.Social
{
    public class FacebookQuery : SocialBaseQuery
    {
        public FacebookQuery()
        {
            this.BaseAddress = "http://www.facebook.com/";
            this.PageFormatString = "/{0}/likes";
        }

        public static List<FacebookSnapshot> GetFacebookSnapshots(List<FacebookAccount> twitterAccounts)
        {
            return (new FacebookQuery()).GetSnapshots<FacebookSnapshot, FacebookAccount>(twitterAccounts);
        }

        public static FacebookSnapshot GetFacebookSnapshot(FacebookAccount twitterAccount)
        {
            return (new FacebookQuery()).GetSnapshot(twitterAccount) as FacebookSnapshot;
        }

        protected override SocialBaseSnapshot ParsePage(HtmlNode documentNode, SocialBaseAccount account)
        {
            FacebookSnapshot accountSnapshot = new FacebookSnapshot();
            accountSnapshot.FacebookAccountId = account.Id;

            if (null == documentNode)
            {
                return null;
            }

            // We need to strip out comment tags. Facebook puts the this data in comment tags and HAP does not parse through comments.
            documentNode.InnerHtml = documentNode.InnerHtml.Replace("<!--", String.Empty).Replace("-->", String.Empty);

            string totalLikesXPath = @"//h3[text() = 'Total Likes']/../../../../div/span[@class='timelineLikesBigNumber fsm']";
            HtmlNode totalLikes = documentNode.SelectSingleNode(totalLikesXPath);
            HtmlNodeCollection likes = documentNode.SelectNodes(totalLikesXPath);
            if (null == totalLikes)
            {
                accountSnapshot.TotalLikes = -1;
                accountSnapshot.Log += "Could not find totalLikes using " + totalLikesXPath + Environment.NewLine;
            }
            else
            {
                accountSnapshot.TotalLikes = int.Parse(totalLikes.InnerText, NumberStyles.AllowThousands);
            }

            string peopleTalkingAboutThisXPath = @"//h3[text() = 'People Talking About This']/../../../../div/span[@class='timelineLikesBigNumber fsm']";
            HtmlNode peopleTalkingAboutThis = documentNode.SelectSingleNode(peopleTalkingAboutThisXPath);
            if (null == peopleTalkingAboutThis)
            {
                accountSnapshot.PeopleTalkingAboutThis = -1;
                accountSnapshot.Log += "Could not find peopleTalkingAboutThis using " + peopleTalkingAboutThisXPath + Environment.NewLine;
            }
            else
            {
                accountSnapshot.PeopleTalkingAboutThis = int.Parse(peopleTalkingAboutThis.InnerText, NumberStyles.AllowThousands);
            }

            string mostPopularWeekXPath = @"//span[text()='Most Popular Week']";
            HtmlNode mostPopularWeek = documentNode.SelectSingleNode(mostPopularWeekXPath);
            if (null != mostPopularWeek && null != mostPopularWeek.PreviousSibling)
            {
                accountSnapshot.MostPopularWeek = DateTime.Parse(mostPopularWeek.PreviousSibling.InnerText);
            }
            else
            {
                accountSnapshot.MostPopularWeek = new DateTime(1900, 1, 1);
                accountSnapshot.Log += "Could not find mostPopularWeek using " + mostPopularWeekXPath + Environment.NewLine;
            }

            string mostPopularCityXPath = @"//span[text()='Most Popular City']";
            HtmlNode mostPopularCity = documentNode.SelectSingleNode(mostPopularCityXPath);
            if (null != mostPopularCity && null != mostPopularCity.PreviousSibling)
            {
                accountSnapshot.MostPopularCity = mostPopularCity.PreviousSibling.InnerText;
            }
            else
            {
                accountSnapshot.MostPopularCity = String.Empty;
                accountSnapshot.Log += "Could not find mostPopularCity using " + mostPopularCityXPath + Environment.NewLine;
            }

            string mostPopularAgeGroupXPath = @"//span[text()='Most Popular Age Group']";
            HtmlNode mostPopularAgeGroup = documentNode.SelectSingleNode(mostPopularAgeGroupXPath);
            if (null != mostPopularAgeGroup && null != mostPopularAgeGroup.PreviousSibling)
            {
                accountSnapshot.MostPopularAgeGroup = mostPopularAgeGroup.PreviousSibling.InnerText;
            }
            else
            {
                accountSnapshot.MostPopularAgeGroup = String.Empty;
                accountSnapshot.Log += "Could not find mostPopularAgeGroup using " + mostPopularAgeGroupXPath + Environment.NewLine;
            }

            return accountSnapshot;
        }
    }
}
