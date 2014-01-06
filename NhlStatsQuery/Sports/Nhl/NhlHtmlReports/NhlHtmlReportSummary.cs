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
    public class NhlHtmlReportSummary : NhlHtmlReportBase
    {
        public static void UpdateSeason([Optional] int year, [Optional] DateTime fromDate, [Optional] bool forceOverwrite)
        {
            // Get the RtssReports for the specified year
            List<NhlRtssReportModel> models = NhlHtmlReportBase.GetRtssReports(year);
            List<NhlHtmlReportSummaryModel> existingModels = null;
            if (forceOverwrite == false)
            {
                // Only query for existing if we are not going to force overwrite all
                existingModels = NhlHtmlReportSummary.GetHtmlSummaryReports(year, fromDate);
            }

            // For each report, get the html blob from blob storage and parse the blob to a report
            List<NhlHtmlReportSummaryModel> results = new List<NhlHtmlReportSummaryModel>();
            foreach (NhlRtssReportModel model in models)
            {
                if (forceOverwrite == false && existingModels.Exists(m => m.NhlRtssReportModelId == model.Id))
                {
                    // In this case, only get data if it is not already populated
                    continue;
                }

                string htmlBlob = HtmlBlob.RetrieveBlob(HtmlBlobType.NhlRoster, model.Id.ToString(), new Uri(model.RosterLink), true);
                NhlHtmlReportSummaryModel report = NhlHtmlReportSummary.ParseHtmlBlob(model.Id, htmlBlob);

                if (null != report)
                {
                    results.Add(report);
                }
            }

            // Save the reports to the db
            using (SportsDataContext db = new SportsDataContext())
            {
                db.NhlHtmlReportSummaries.AddOrUpdate<NhlHtmlReportSummaryModel>(
                    m => m.NhlRtssReportModelId,
                    results.ToArray());
                db.SaveChanges();
            }
        }

        public static NhlHtmlReportSummaryModel ParseHtmlBlob(int rtssReportId, string html)
        {
            if (String.IsNullOrWhiteSpace(html) || html.Equals("404")) { return null; }

            NhlHtmlReportSummaryModel model = new NhlHtmlReportSummaryModel();
            model.NhlRtssReportModelId = rtssReportId;

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            HtmlNode tableNode = htmlDocument.DocumentNode.SelectSingleNode(@".//table[.//table[@id='GameInfo']]");

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

            string[] visitorInfo = visitorNameNode.InnerHtml.RemoveSpecialWhitespaceCharacters().Split(new string[] {"<br>"}, StringSplitOptions.None);
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
            DateTime gameDate = DateTime.Parse(gameInfoRowNodes[3].InnerText);
            model.Date = gameDate;

            string attendanceAndArenaText = gameInfoRowNodes[4].InnerText.RemoveSpecialWhitespaceCharacters();
            string[] attendanceAndArena = attendanceAndArenaText.Split(new string[] {"&nbsp;"}, StringSplitOptions.RemoveEmptyEntries) ;

            if (attendanceAndArena.Count() == 3)
            {
                Match attendanceMatch = Regex.Match(attendanceAndArena[0].Replace(",", String.Empty), @"\d+");
                string attendanceAsString = String.IsNullOrWhiteSpace(attendanceMatch.Value) ? "0" : attendanceMatch.Value;
                model.Attendance = Convert.ToInt32(attendanceAsString);
                model.ArenaName = attendanceAndArena[2];
            }
            else
            {
                model.ArenaName = attendanceAndArena[0];
            }

            model.ArenaName = model.ArenaName.Replace("&amp;", "&");

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
        private static List<NhlHtmlReportSummaryModel> GetHtmlSummaryReports([Optional] int year, [Optional] DateTime fromDate)
        {
            year = NhlGameStatsBaseModel.SetDefaultYear(year);

            List<NhlHtmlReportSummaryModel> existingModels = new List<NhlHtmlReportSummaryModel>();
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
    }
}
