//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Net;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;

//using HtmlAgilityPack;
//using SportsData.Models;

//namespace SportsData.Nhl.Query
//{
//    /// <summary>
//    /// Represents a query that will be used to retrieve stats from a url
//    /// </summary>
//    public partial class NhlSummaryQuery : NhlBaseClass
//    {
//        /// <summary>
//        /// Not actually used yet. Need to migrate this class to use the NhlBaseClass pattern.
//        /// </summary>
//        protected override string RelativeUrlFormatString
//        {
//            get { return "/ice/gamestats.htm?fetchKey={0}{1}ALLSATALL&viewName=summary&sort=gameDate&pg={2}"; }
//        }

//        protected override Type ModelType
//        {
//            get
//            {
//                return typeof(NhlGameSummaryModel);
//            }
//        }

//        public override void AddOrUpdateDb(List<NhlGameStatsBaseModel> models)
//        {
//            throw new NotImplementedException();
//        }

//        protected override NhlGameStatsBaseModel MapHtmlRowToModel(HtmlNode row, NhlSeasonType nhlSeasonType)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// Gets stats for pre-season, regular season and playoffs from a season and stores them in the db
//        /// </summary>
//        public static void GetAndStoreStats(string season)
//        {
//            int intSeason = Int32.Parse(season);

//            NhlSummaryQuery.GetAndStoreStats(intSeason, NhlSeasonType.PreSeason);
//            NhlSummaryQuery.GetAndStoreStats(intSeason, NhlSeasonType.RegularSeason);
//            NhlSummaryQuery.GetAndStoreStats(intSeason, NhlSeasonType.Playoff);
//        }

//        protected static void GetAndStoreStats(int season, NhlSeasonType gameType)
//        {
//            string queryUrl = "http://www.nhl.com/ice/gamestats.htm?fetchKey={0}{1}ALLSATALL&viewName=summary&sort=gameDate&pg={2}";

//            int startPage = 1;

//            // Get the data from the first page
//            HtmlNode tablePage1 = NhlSummaryQuery.GetStatsTable(NhlSummaryQuery.GetPage(queryUrl, season, gameType, startPage));

//            // Parse out the total number of pages from the first page's html
//            int endPage = NhlBaseClass.GetPageCount(tablePage1);

//            int expectedNumResults = NhlBaseClass.GetResultsCount(tablePage1);

//            // Start saving rows to the db if we have less results than expected
//            if (NhlSummaryQuery.GetNumResultsInDb(season, gameType) >= expectedNumResults)
//            {
//                return;
//            }
//            else
//            {
//                NhlSummaryQuery.SaveRowsToDb(tablePage1, gameType);
//            }


//            for (int i = startPage + 1; i <= endPage; i++)
//            {
//                string html = NhlSummaryQuery.GetPage(queryUrl, season, gameType, i);
//                HtmlNode nextPage = NhlSummaryQuery.GetStatsTable(html);
//                NhlSummaryQuery.SaveRowsToDb(nextPage, gameType);

//                if (NhlSummaryQuery.GetNumResultsInDb(season, gameType) >= expectedNumResults)
//                {
//                    return;
//                }
//            }
//        }

//        /// <summary>
//        /// Parses the data out of an html tr row and saves it in NhlStatsContext db
//        /// </summary>
//        protected static void SaveRowToDb(HtmlNode tr, NhlSeasonType gameType)
//        {
//            HtmlNodeCollection tds = tr.SelectNodes("./td");

//            using (SportsDataContext db = new SportsDataContext())
//            {
//                NhlGameSummaryModel gameSummary = new NhlGameSummaryModel();
//                gameSummary.Date = Convert.ToDateTime(tds[0].InnerText.Replace("'", "/"));
//                gameSummary.Home = tds[3].InnerText;

//                var result = from g in db.NhlGameSummaries
//                             where g.Date == gameSummary.Date && g.Home == gameSummary.Home
//                             select g;
//                if (result.Any())
//                {
//                    // do not add the record if it already exists
//                    return;
//                }

//                gameSummary.Year = NhlGameSummaryModel.GetSeason(gameSummary.Date).Item2;
//                gameSummary.NhlSeasonType = gameType;
//                gameSummary.Visitor = tds[1].InnerText;
//                gameSummary.VisitorScore = ConvertStringToInt(tds[2].InnerText);
//                gameSummary.HomeScore = ConvertStringToInt(tds[4].InnerText);
//                gameSummary.OS = tds[5].InnerText;
//                gameSummary.WGoalie = tds[6].InnerText;
//                gameSummary.WGoal = tds[7].InnerText;
//                gameSummary.VisitorShots = ConvertStringToInt(tds[8].InnerText);
//                gameSummary.VisitorPPGF = ConvertStringToInt(tds[9].InnerText);
//                gameSummary.VisitorPPOpp = ConvertStringToInt(tds[10].InnerText);
//                gameSummary.VisitorPIM = ConvertStringToInt(tds[11].InnerText);
//                gameSummary.HomeShots = ConvertStringToInt(tds[12].InnerText);
//                gameSummary.HomePPGF = ConvertStringToInt(tds[13].InnerText);
//                gameSummary.HomePPOpp = ConvertStringToInt(tds[14].InnerText);
//                gameSummary.HomePIM = ConvertStringToInt(tds[15].InnerText);
//                gameSummary.Attendance = ConvertStringToInt(tds[16].InnerText.Replace(",", String.Empty));

//                db.NhlGameSummaries.Add(gameSummary);
//                db.SaveChanges();
//            }
//        }

//        private static int GetNumResultsInDb(int season, NhlSeasonType gameType)
//        {
//            SportsDataContext db = new SportsDataContext();

//            int intSeason = Convert.ToInt32(season);
//            var results = (from g in db.NhlGameSummaries
//                           where g.Year == intSeason && g.NhlSeasonType == gameType
//                           orderby g.Date descending
//                           select g);

//            return results.Count();
//        }

//        private static int ConvertStringToInt(string s)
//        {
//            int result;
//            bool success = Int32.TryParse(s, out result);

//            if (!success)
//            {
//                return 0;
//            }
//            else
//            {
//                return result;
//            }
//        }

//        private static string GetPage(string queryFormatString, int season, NhlSeasonType gameType, int pageNum)
//        {
//            string uriString = String.Format(queryFormatString, season, Convert.ToInt32(gameType), pageNum);
//            Uri uri = new Uri(uriString);

//            HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
//            request.Method = "GET";

//            WebResponse response = request.GetResponse();

//            string responseString;
//            Stream s = response.GetResponseStream();
//            StreamReader sr = new StreamReader(s);
//            responseString = sr.ReadToEnd();

//            return responseString;
//        }

//        protected static void SaveRowsToDb(HtmlNode node, NhlSeasonType gameType)
//        {
//            // Verify that the tables contain rows
//            if (null == node || null == node.SelectNodes("./tbody/tr"))
//            {
//                // This is unexpected
//                return;
//            }

//            foreach (HtmlNode row in node.SelectNodes("./tbody/tr"))
//            {
//                NhlSummaryQuery.SaveRowToDb(row, gameType);
//            }
//        }

//        protected static int CountRows(HtmlNode table)
//        {
//            // Verify that the tables contain rows
//            if (null == table.SelectNodes("./tbody/tr"))
//            {
//                // This is unexpected
//                return 0;
//            }

//            HtmlNodeCollection rows = table.SelectNodes("./tbody/tr");
//            return rows.Count;
//        }

//        /// <summary>
//        /// Finds the stats table in the html document and returns it as an HTMLNode
//        /// </summary>
//        /// <param name="htmlString"></param>
//        /// <returns></returns>
//        protected static HtmlNode GetStatsTable(string htmlString)
//        {
//            HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
//            htmlDocument.LoadHtml(htmlString);

//            foreach (HtmlNode table in htmlDocument.DocumentNode.SelectNodes("//table"))
//            {
//                HtmlAttribute classAttribute = table.Attributes["class"];
//                if (null != classAttribute && classAttribute.Value.Equals("data stats", StringComparison.CurrentCultureIgnoreCase))
//                {
//                    return table;
//                }
//            }

//            return null;
//        }
//    }
//}