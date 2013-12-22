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

            HtmlNode tableNode = htmlDocument.DocumentNode.SelectSingleNode(@".//table[.//table[@id='GameInfo']]");
            
            //HtmlNode titleNode = htmlDocument.DocumentNode.SelectSingleNode(@"//title[text()='Playing Roster']");
            
            //// Get the top level table for the summary
            //HtmlNode tableNode = titleNode.NextSibling;
            //while (tableNode != null)
            //{
            //    if (tableNode.Name.Equals("table"))
            //    {
            //        break;
            //    }

            //    tableNode = tableNode.NextSibling;
            //}
            if (null == tableNode) { return null; }

            #region Get team names, score, game numbers

            HtmlNode visitorTableNode = tableNode.SelectSingleNode(@".//table[@id='Visitor']");
            HtmlNode homeTableNode = tableNode.SelectSingleNode(@".//table[@id='Home']");

            HtmlNode visitorScoreNode = visitorTableNode.SelectNodes(@".//tr").ElementAt(1).SelectNodes(@".//tr/td").ElementAt(1);
            HtmlNode homeScoreNode = homeTableNode.SelectNodes(@".//tr").ElementAt(1).SelectNodes(@".//tr/td").ElementAt(1);

            int visitorScore = Convert.ToInt32(visitorScoreNode.InnerText);
            int homeScore = Convert.ToInt32(homeScoreNode.InnerText);

            HtmlNode visitorNameNode;
            if (visitorTableNode.SelectSingleNode(@"./tbody") != null)
            {
                visitorNameNode = visitorTableNode.SelectNodes(@"./tbody/tr").ElementAt(2).SelectSingleNode(@"./td");
            }
            else
            {
                visitorNameNode = visitorTableNode.SelectNodes(@"./tr").ElementAt(2).SelectSingleNode(@"./td");
            }

            HtmlNode homeNameNode;
            if (homeTableNode.SelectSingleNode(@"./tbody") != null)
            {
                homeNameNode = homeTableNode.SelectNodes(@"./tbody/tr").ElementAt(2).SelectSingleNode(@"./td");
            }
            else
            {
                homeNameNode = homeTableNode.SelectNodes(@"./tr").ElementAt(2).SelectSingleNode(@"./td");
            }

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

            #endregion

            #region Date, time, attendance, league game number

            HtmlNode gameInfoTableNode = tableNode.SelectSingleNode(@".//table[@id='GameInfo']");
            HtmlNodeCollection gameInfoRowNodes = gameInfoTableNode.SelectNodes(@".//tr");
            DateTime gameDate = DateTime.Parse(gameInfoRowNodes[3].InnerText);

            string attendanceAndArenaText = gameInfoRowNodes[4].InnerText.RemoveSpecialWhitespaceCharacters();
            string[] attendanceAndArena = attendanceAndArenaText.Split(new string[] {"&nbsp;"}, StringSplitOptions.RemoveEmptyEntries) ;

            if (attendanceAndArena.Count() == 3)
            {
                Match attendanceMatch = Regex.Match(attendanceAndArena[0].Replace(",", String.Empty), @"\d+");
                int attendance = Convert.ToInt32(attendanceMatch.Value);
                string arena = attendanceAndArena[2];
            }
            else
            {
                string arena = attendanceAndArena[0];
            }

            string gameStatus = gameInfoRowNodes[7].InnerText.RemoveSpecialWhitespaceCharacters();

            if (gameStatus.Equals("Final", StringComparison.InvariantCultureIgnoreCase))
            {
                string[] startAndEndTimes = gameInfoRowNodes[5].InnerText.RemoveSpecialWhitespaceCharacters().Split(new string[] { ";", "&nbsp;" }, StringSplitOptions.None);
                string startTime = String.Concat(startAndEndTimes[1], " ", startAndEndTimes[2]);
                string endTime = String.Concat(startAndEndTimes[4], " ", startAndEndTimes[5]);
            }

            Match leagueGameNumberMatch = Regex.Match(gameInfoRowNodes[6].InnerText, @"\d+");
            int leagueGameNumber = Convert.ToInt32(leagueGameNumberMatch.Value);

            #endregion

            return model;
        }

    }
}
