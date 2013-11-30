using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using HtmlAgilityPack;
using SportsData.Models;

namespace SportsData.Nhl.Query
{
    /// <summary>
    /// Represents a query that will be used to retrieve stats from a url
    /// </summary>
    public partial class NhlStatsQuery
    {
        /// <summary>
        /// Gets stats for pre-season, regular season and playoffs from a season and stores them in the db
        /// </summary>
        public static void GetAndStoreStats(string season)
        {
            int intSeason = Int32.Parse(season);

            NhlStatsQuery.GetAndStoreStats(intSeason, NhlSeasonType.PreSeason);
            NhlStatsQuery.GetAndStoreStats(intSeason, NhlSeasonType.RegularSeason);
            NhlStatsQuery.GetAndStoreStats(intSeason, NhlSeasonType.Playoff);
        }

        protected static void GetAndStoreStats(int season, NhlSeasonType gameType)
        {
            string queryUrl = "http://www.nhl.com/ice/gamestats.htm?fetchKey={0}{1}ALLSATALL&viewName=summary&sort=gameDate&pg={2}";

            int startPage = 1;

            // Get the data from the first page
            HtmlNode tablePage1 = NhlStatsQuery.GetStatsTable(NhlStatsQuery.GetPage(queryUrl, season, gameType, startPage));

            // Parse out the total number of pages from the first page's html
            int endPage = NhlStatsQuery.GetNumPages(tablePage1);
            //int endPage = 2; 

            int expectedNumResults = NhlStatsQuery.GetNumExpectedResults(tablePage1);

            // Start saving rows to the db if we have less results than expected
            if (NhlStatsQuery.GetNumResultsInDb(season, gameType) >= expectedNumResults)
            {
                return;
            }
            else
            {
                NhlStatsQuery.SaveRowsToDb(tablePage1, gameType);
            }


            for (int i = startPage + 1; i <= endPage; i++)
            {
                string html = NhlStatsQuery.GetPage(queryUrl, season, gameType, i);
                HtmlNode nextPage = NhlStatsQuery.GetStatsTable(html);
                NhlStatsQuery.SaveRowsToDb(nextPage, gameType);

                if (NhlStatsQuery.GetNumResultsInDb(season, gameType) >= expectedNumResults)
                {
                    return;
                }
            }
        }

        public static string GetStatsFromDb(string season, bool performUpdate)
        {
            if (performUpdate)
            {
                NhlStatsQuery.GetAndStoreStats(season);
            }

            SportsDataContext db = new SportsDataContext();

            int intSeason = Convert.ToInt32(season);
            var results = (from g in db.NhlGameSummaries
                           where g.Season == intSeason
                           orderby g.Date descending
                           select g);

            int numResults = results.Count();

            // Create an instance of an HtmlTable control.
            HtmlTable table = new HtmlTable();
            table.Border = 1;
            table.CellPadding = 3;

            // Add title row
            table.Rows.Add(BuildTitleRow(intSeason, numResults));

            // Add header row
            table.Rows.Add(BuildHeaderRow(intSeason));

            // Add rows to the table 
            foreach (NhlGameSummary gameSummary in results)
            {
                table.Rows.Add(BuildRow(gameSummary));
            }

            using (StringWriter sw = new StringWriter())
            {
                table.RenderControl(new HtmlTextWriter(sw));
                return sw.ToString();
            }
        }
    }
}
