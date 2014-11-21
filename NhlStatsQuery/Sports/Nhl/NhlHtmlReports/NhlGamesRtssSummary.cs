using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HtmlAgilityPack;
using SportsData;
using SportsData.Models;

namespace SportsData.Nhl
{
    public class NhlGamesRtssSummary : NhlHtmlReportBase
    {
        public static void UpdateSeason([Optional] int year, [Optional] DateTime fromDate, [Optional] bool forceOverwrite)
        {
            // Get the RtssReports for the specified year
            List<Nhl_Games_Rtss> models = NhlHtmlReportBase.GetRtssReports(year);
            List<Nhl_Games_Rtss_Summary> existingModels = null;
            if (forceOverwrite == false)
            {
                // Only query for existing if we are not going to force overwrite all
                existingModels = NhlGamesRtssSummary.GetHtmlSummaryReports(year, fromDate);
            }

            // For each report, get the html blob from blob storage and parse the blob to a report
            List<Nhl_Games_Rtss_Summary> results = new List<Nhl_Games_Rtss_Summary>();
            foreach (Nhl_Games_Rtss model in models)
            {
                if (forceOverwrite == false && existingModels.Exists(m => m.NhlRtssReportModelId == model.Id))
                {
                    // In this case, only get data if it is not already populated
                    continue;
                }

                Nhl_Games_Rtss_Summary report = null;
                if (!model.GameLink.Equals("#"))
                {
                    string htmlBlob = HtmlBlob.RetrieveBlob(HtmlBlobType.NhlRoster, model.Id.ToString(), new Uri(model.GameLink), true);
                    report = NhlGamesRtssSummary.ParseHtmlBlob(model.Id, htmlBlob);
                }

                if (null != report)
                {
                    results.Add(report);
                }
            }

            // Save the reports to the db
            using (SportsDataContext db = new SportsDataContext())
            {
                db.NhlHtmlReportSummaries.AddOrUpdate<Nhl_Games_Rtss_Summary>(
                    m => m.NhlRtssReportModelId,
                    results.ToArray());
                db.SaveChanges();
            }
        }

        public static Nhl_Games_Rtss_Summary ParseHtmlBlob(int rtssReportId, string html)
        {
            if (String.IsNullOrWhiteSpace(html) || html.Equals("404")) { return null; }

            Nhl_Games_Rtss_Summary model = new Nhl_Games_Rtss_Summary();
            model.NhlRtssReportModelId = rtssReportId;

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNode documentNode = htmlDocument.DocumentNode;

            // Special Case
            // The html for this game doesn't follow the same format as the other games 
            if (null != documentNode.SelectSingleNode(@"./html/head/link[@href='RO010002_files/editdata.mso']"))
            {
                return NhlGamesRtssSummary.BruinsRangersSpecialCase(rtssReportId);
            }

            HtmlNode tableNode = documentNode.SelectSingleNode(@".//table[.//table[@id='GameInfo']]");

            if (null == tableNode) { return null; }

            #region Get team names, score, game numbers

            HtmlNode visitorTableNode = tableNode.SelectSingleNode(@".//table[@id='Visitor']");
            HtmlNode homeTableNode = tableNode.SelectSingleNode(@".//table[@id='Home']");

            HtmlNode visitorScoreNode = visitorTableNode.SelectNodes(@".//tr").ElementAt(1).SelectNodes(@".//tr/td").ElementAt(1);
            HtmlNode homeScoreNode = homeTableNode.SelectNodes(@".//tr").ElementAt(1).SelectNodes(@".//tr/td").ElementAt(1);

            model.VisitorScore = Convert.ToInt32(visitorScoreNode.InnerText);
            model.HomeScore = Convert.ToInt32(homeScoreNode.InnerText);

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

            string[] visitorInfo = visitorNameNode.InnerHtml.RemoveSpecialWhitespaceCharacters().Split(new string[] { "<br>" }, StringSplitOptions.None);
            model.Visitor = visitorInfo.ElementAt(0);
            MatchCollection visitorGameNumbers = Regex.Matches(visitorInfo.ElementAt(1), @"\d+");
            model.VisitorGameNumber = Convert.ToInt32(visitorGameNumbers[0].Value);
            model.VisitorAwayGameNumber = Convert.ToInt32(visitorGameNumbers[1].Value);

            string[] homeInfo = homeNameNode.InnerHtml.RemoveSpecialWhitespaceCharacters().Split(new string[] { "<br>" }, StringSplitOptions.None);
            model.Home = homeInfo.ElementAt(0);
            MatchCollection homeGameNumbers = Regex.Matches(homeInfo.ElementAt(1), @"\d+");
            model.HomeGameNumber = Convert.ToInt32(homeGameNumbers[0].Value);
            model.HomeHomeGameNumber = Convert.ToInt32(homeGameNumbers[1].Value);

            #endregion

            #region Date, time, attendance, league game number

            HtmlNode gameInfoTableNode = tableNode.SelectSingleNode(@".//table[@id='GameInfo']");
            HtmlNodeCollection gameInfoRowNodes = gameInfoTableNode.SelectNodes(@".//tr");

            // Special Case
            // A workaround for a bug in one of the reports which should actually be Wednesday, October 3, 2007
            string gameDateText = gameInfoRowNodes[3].InnerText.RemoveSpecialWhitespaceCharacters();
            if (gameDateText.Equals("Saturday, October 2, 2007", StringComparison.InvariantCultureIgnoreCase))
            {
                gameDateText = "Wednesday, October 3, 2007";
            }

            DateTime gameDate = DateTime.Parse(gameDateText);
            model.Date = gameDate;

            string attendanceAndArenaText = gameInfoRowNodes[4].InnerText.RemoveSpecialWhitespaceCharacters();
            attendanceAndArenaText = attendanceAndArenaText.Replace("&nbsp;", " ").Replace(",", String.Empty);

            Match attendanceMatch = Regex.Match(attendanceAndArenaText, @"\d+");
            string attendanceAsString = String.IsNullOrWhiteSpace(attendanceMatch.Value) ? "0" : attendanceMatch.Value;
            model.Attendance = Convert.ToInt32(attendanceAsString);
            
            // Find 'at' and assume the arena name follows
            string token = " at ";
            int tokenIndex = attendanceAndArenaText.IndexOf(token, StringComparison.InvariantCultureIgnoreCase);
            
            // If 'at' can't be found, try '@'
            if (tokenIndex < 0)
            {
                token = " @ ";
                tokenIndex = attendanceAndArenaText.IndexOf(token, StringComparison.InvariantCultureIgnoreCase);
            }

            int arenaNameIndex = tokenIndex + token.Length;

            if (tokenIndex >= 0)
            {
                model.ArenaName = attendanceAndArenaText.Substring(arenaNameIndex);
            }
            else
            {
                // just take the whole string
                model.ArenaName = attendanceAndArenaText;
            }

            //string attendanceAndArenaText = gameInfoRowNodes[4].InnerText.RemoveSpecialWhitespaceCharacters();
            //string[] attendanceAndArena = attendanceAndArenaText.Split(new string[] {"&nbsp;"}, StringSplitOptions.RemoveEmptyEntries) ;

            //if (attendanceAndArena.Count() == 3)
            //{
            //    Match attendanceMatch = Regex.Match(attendanceAndArena[0].Replace(",", String.Empty), @"\d+");
            //    string attendanceAsString = String.IsNullOrWhiteSpace(attendanceMatch.Value) ? "0" : attendanceMatch.Value;
            //    model.Attendance = Convert.ToInt32(attendanceAsString);
            //    model.ArenaName = attendanceAndArena[2];
            //}
            //else
            //{
            //    model.ArenaName = attendanceAndArena[0];
            //}

            model.ArenaName = model.ArenaName.Replace("&amp;", "&").Trim();

            model.GameStatus = gameInfoRowNodes[7].InnerText.RemoveSpecialWhitespaceCharacters();

            if (model.GameStatus.Equals("Final", StringComparison.InvariantCultureIgnoreCase))
            {
                string[] startAndEndTimes = gameInfoRowNodes[5].InnerText.RemoveSpecialWhitespaceCharacters().Split(new string[] { ";", "&nbsp;" }, StringSplitOptions.None);
                model.StartTime = String.Concat(startAndEndTimes[1], " ", startAndEndTimes[2]);
                model.EndTime = String.Concat(startAndEndTimes[4], " ", startAndEndTimes[5]);
            }

            Match leagueGameNumberMatch = Regex.Match(gameInfoRowNodes[6].InnerText, @"\d+");
            model.LeagueGameNumber = Convert.ToInt32(leagueGameNumberMatch.Value);

            #endregion

            return model;
        }

        /// <summary>
        /// Get the NhlHtmlReportSummaryModels for the specified year
        /// </summary>
        private static List<Nhl_Games_Rtss_Summary> GetHtmlSummaryReports([Optional] int year, [Optional] DateTime fromDate)
        {
            year = NhlModelHelper.SetDefaultYear(year);

            List<Nhl_Games_Rtss_Summary> existingModels = new List<Nhl_Games_Rtss_Summary>();
            using (SportsDataContext db = new SportsDataContext())
            {
                existingModels = (from m in db.NhlHtmlReportSummaries
                                  where
                                      m.NhlRtssReportModel.Year == year &&
                                      m.NhlRtssReportModel.Date >= fromDate
                                  select m).ToList();
            }

            return existingModels;
        }

        private static Nhl_Games_Rtss_Summary BruinsRangersSpecialCase(int rtssReportId)
        {
            Nhl_Games_Rtss_Summary model = new Nhl_Games_Rtss_Summary();
            model.NhlRtssReportModelId = rtssReportId;

            model.VisitorScore = 2;
            model.Visitor = "BOSTON BRUINS";
            model.VisitorAwayGameNumber = 1;
            model.VisitorGameNumber = 1;

            model.HomeScore = 1;
            model.Home = "NEW YORK RANGERS";
            model.HomeHomeGameNumber = 1;
            model.HomeGameNumber = 1;

            model.Date = new DateTime(2009, 9, 15);
            model.Attendance = 11111;
            model.ArenaName = "Madison Square Garden";
            model.StartTime = "7:12 EDT";
            model.EndTime = "9:24 EDT";
            model.LeagueGameNumber = 2;
            model.GameStatus = "Final";

            return model;
        }

    }
}
