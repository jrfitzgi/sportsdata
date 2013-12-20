using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HtmlAgilityPack;
using SportsData;
using SportsData.Models;

namespace SportsData.Nhl
{
    public class NhlHtmlReportSummary
    {
        public static NhlHtmlReportSummaryModel ParseHtmlBlob(string html)
        {
            NhlHtmlReportSummaryModel model = new NhlHtmlReportSummaryModel();

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNode titleNode = htmlDocument.DocumentNode.SelectSingleNode(@"//title[text()='Playing Roster']");
            
            // Get the top level table for the summary
            HtmlNode tableNode = titleNode.NextSibling;
            while (tableNode != null)
            {
                if (tableNode.Name.Equals("table"))
                {
                    break;
                }

                tableNode = tableNode.NextSibling;
            }
            if (null == tableNode) { return null; }

            HtmlNode visitorNameTBodyNode = tableNode.SelectSingleNode(@".//table/tbody[tr/td/text()='VISITOR']");
            HtmlNode homeNameTBodyNode = tableNode.SelectSingleNode(@".//table/tbody[tr/td/text()='HOME']");

            HtmlNode visitorNameNode = visitorNameTBodyNode.SelectNodes(@"./tr").ElementAt(2).SelectSingleNode(@"./td");
            HtmlNode homeNameNode = homeNameTBodyNode.SelectNodes(@"./tr").ElementAt(2).SelectSingleNode(@"./td");

            string[] visitorInfo = visitorNameNode.InnerHtml.RemoveSpecialWhitespaceCharacters().Split(new string[] {"<br>"}, StringSplitOptions.None);
            string visitorName = visitorInfo.ElementAt(0);
            MatchCollection visitorGameNumbers = Regex.Matches(visitorInfo.ElementAt(1), @"\d+");
            int visitorGameNumber = Convert.ToInt32(visitorGameNumbers[0].Value);
            int visitorAwayGameNumber = Convert.ToInt32(visitorGameNumbers[1].Value);

            string[] homeInfo = homeNameNode.InnerHtml.RemoveSpecialWhitespaceCharacters().Split(new string[] { "<br>" }, StringSplitOptions.None);
            string homeName = homeInfo.ElementAt(0);
            MatchCollection homeGameNumbers = Regex.Matches(homeInfo.ElementAt(1), @"\d+");
            int homeGameNumber = Convert.ToInt32(homeGameNumbers[0].Value);
            int homeHomeGameNumber = Convert.ToInt32(homeGameNumbers[1].Value);

            return model;
        }

    }
}
