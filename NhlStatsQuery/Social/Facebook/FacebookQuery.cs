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
    public class FacebookQuery : SocialBaseQuery
    {
        public FacebookQuery()
        {
            this.BaseAddress = "http://www.facebook.com/";
            this.PageFormatString = "/{0}/likes";
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
            HtmlNode mostPopularWeek = documentNode.SelectSingleNode(mostPopularWeekXPath).PreviousSibling;
            if (null == mostPopularWeek)
            {
                accountSnapshot.MostPopularWeek = new DateTime(1900, 1, 1);
                accountSnapshot.Log += "Could not find mostPopularWeek using " + mostPopularWeekXPath + Environment.NewLine;
            }
            else
            {
                accountSnapshot.MostPopularWeek = DateTime.Parse(mostPopularWeek.InnerText);
            }

            string mostPopularCityXPath = @"//span[text()='Most Popular City']";
            HtmlNode mostPopularCity = documentNode.SelectSingleNode(mostPopularCityXPath).PreviousSibling;
            if (null == mostPopularCity)
            {
                accountSnapshot.MostPopularCity = String.Empty;
                accountSnapshot.Log += "Could not find mostPopularCity using " + mostPopularCityXPath + Environment.NewLine;
            }
            else
            {
                accountSnapshot.MostPopularCity = mostPopularCity.InnerText;
            }

            string mostPopularAgeGroupXPath = @"//span[text()='Most Popular Age Group']";
            HtmlNode mostPopularAgeGroup = documentNode.SelectSingleNode(mostPopularAgeGroupXPath).PreviousSibling;
            if (null == mostPopularAgeGroup)
            {
                accountSnapshot.MostPopularAgeGroup = String.Empty;
                accountSnapshot.Log += "Could not find mostPopularAgeGroup using " + mostPopularAgeGroupXPath + Environment.NewLine;
            }
            else
            {
                accountSnapshot.MostPopularAgeGroup = mostPopularAgeGroup.InnerText;
            }

            return accountSnapshot;
        }
    }
}
