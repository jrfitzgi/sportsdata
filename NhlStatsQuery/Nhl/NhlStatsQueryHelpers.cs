using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using HtmlAgilityPack;

namespace SportsData.Nhl.Query
{
    public class HockeySeason
    {
        public string OneYearName { get; set; }
        public string TwoYearName { get; set; }

        public HockeySeason(string oneYearName, string twoYearName)
        {
            this.OneYearName = oneYearName;
            this.TwoYearName = twoYearName;
        }
    }

    public partial class NhlStatsQuery
    {
        /// <summary>
        /// Parses the data out of an html tr row and saves it in NhlStatsContext db
        /// </summary>
        protected static void SaveRowToDb(HtmlNode tr, int gameType)
        {
            HtmlNodeCollection tds = tr.SelectNodes("./td");

            using (SportsDataContext db = new SportsDataContext())
            {
                NhlGameSummary gameSummary = new NhlGameSummary();
                gameSummary.Date = Convert.ToDateTime(tds[0].InnerText.Replace("'", "/"));
                gameSummary.Home = tds[3].InnerText;

                var result = from g in db.NhlGameSummaries
                             where g.Date == gameSummary.Date && g.Home == gameSummary.Home
                             select g;
                if (result.Any())
                {
                    // do not add the record if it already exists
                    return;
                }

                gameSummary.Season = NhlGameSummary.GetSeason(gameSummary.Date).Item2;
                gameSummary.GameType = gameType;
                gameSummary.Visitor = tds[1].InnerText;
                gameSummary.VisitorScore = ConvertStringToInt(tds[2].InnerText);
                gameSummary.HomeScore = ConvertStringToInt(tds[4].InnerText);
                gameSummary.OS = tds[5].InnerText;
                gameSummary.WGoalie = tds[6].InnerText;
                gameSummary.WGoal = tds[7].InnerText;
                gameSummary.VisitorShots = ConvertStringToInt(tds[8].InnerText);
                gameSummary.VisitorPPGF = ConvertStringToInt(tds[9].InnerText);
                gameSummary.VisitorPPOpp = ConvertStringToInt(tds[10].InnerText);
                gameSummary.VisitorPIM = ConvertStringToInt(tds[11].InnerText);
                gameSummary.HomeShots = ConvertStringToInt(tds[12].InnerText);
                gameSummary.HomePPGF = ConvertStringToInt(tds[13].InnerText);
                gameSummary.HomePPOpp = ConvertStringToInt(tds[14].InnerText);
                gameSummary.HomePIM = ConvertStringToInt(tds[15].InnerText);
                gameSummary.Att = ConvertStringToInt(tds[16].InnerText.Replace(",", String.Empty));

                db.NhlGameSummaries.Add(gameSummary);
                db.SaveChanges();
            }
        }

        private static int GetNumResultsInDb(int season, int gameType)
        {
            SportsDataContext db = new SportsDataContext();

            int intSeason = Convert.ToInt32(season);
            var results = (from g in db.NhlGameSummaries
                           where g.Season == intSeason && g.GameType == gameType
                           orderby g.Date descending
                           select g);

            return results.Count();
        }

        private static int ConvertStringToInt(string s)
        {
            int result;
            bool success = Int32.TryParse(s, out result);

            if (!success)
            {
                return 0;
            }
            else
            {
                return result;
            }
        }

        private static HtmlTableRow BuildRow(NhlGameSummary gameSummary)
        {
            HtmlTableRow htmlRow = new HtmlTableRow();

            HtmlTableCell cell;

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.Date.ToShortDateString()));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(Enum.GetName(typeof(NhlGameSummary.SeasonType), gameSummary.GameType)));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.Visitor));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.VisitorScore.ToString()));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.Home));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.HomeScore.ToString()));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.OS));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.WGoalie));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.WGoal));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.VisitorShots.ToString()));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.VisitorPPGF.ToString()));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.VisitorPPOpp.ToString()));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.VisitorPIM.ToString()));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.HomeShots.ToString()));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.HomePPGF.ToString()));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.HomePPOpp.ToString()));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.HomePIM.ToString()));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl(gameSummary.Att.ToString()));
            htmlRow.Cells.Add(cell);

            return htmlRow;
        }

        private static HtmlTableRow BuildTitleRow(int season, int numResults)
        {
            HtmlTableRow htmlRow = new HtmlTableRow();

            HtmlTableCell cell;

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Season: <br>" + (season-1) + "-" + season));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Results: " + numResults));
            htmlRow.Cells.Add(cell);

            return htmlRow;
        }



        private static HtmlTableRow BuildHeaderRow(int season)
        {
            HtmlTableRow htmlRow = new HtmlTableRow();

            HtmlTableCell cell;

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Date"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Game Type"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Visitor"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Visitor Score"));
            htmlRow.Cells.Add(cell);
            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Home"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Home Score"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("OT/SO"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Winning Goalie"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Winning Goal"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Visitor Shots"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Visitor PPGF"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Visitor PPOpp"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Visitor PIM"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Home Shots"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Home PPGF"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Home PPOpp"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Home PIM"));
            htmlRow.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.Controls.Add(new LiteralControl("Attendance"));
            htmlRow.Cells.Add(cell);

            return htmlRow;
        }

        private static string GetPage(string queryFormatString, int season, int gameType, int pageNum)
        {
            string uriString = String.Format(queryFormatString, season, gameType, pageNum);
            Uri uri = new Uri(uriString);

            HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Method = "GET";

            WebResponse response = request.GetResponse();

            string responseString;
            Stream s = response.GetResponseStream();
            StreamReader sr = new StreamReader(s);
            responseString = sr.ReadToEnd();

            return responseString;
        }

        protected static void SaveRowsToDb(HtmlNode node, int gameType)
        {
            // Verify that the tables contain rows
            if (null == node || null == node.SelectNodes("./tbody/tr"))
            {
                // This is unexpected
                return;
            }

            foreach (HtmlNode row in node.SelectNodes("./tbody/tr"))
            {
                NhlStatsQuery.SaveRowToDb(row, gameType);
            }
        }

        protected static int CountRows(HtmlNode table)
        {
            // Verify that the tables contain rows
            if (null == table.SelectNodes("./tbody/tr"))
            {
                // This is unexpected
                return 0;
            }

            HtmlNodeCollection rows = table.SelectNodes("./tbody/tr");
            return rows.Count;
        }

        /// <summary>
        /// Finds the stats table in the html document and returns it as an HTMLNode
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        protected static HtmlNode GetStatsTable(string htmlString)
        {
            HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(htmlString);

            foreach (HtmlNode table in htmlDocument.DocumentNode.SelectNodes("//table"))
            {
                HtmlAttribute classAttribute = table.Attributes["class"];
                if (null != classAttribute && classAttribute.Value.Equals("data stats", StringComparison.CurrentCultureIgnoreCase))
                {
                    return table;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the number of pages in a stats table
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        protected static int GetNumPages(HtmlNode tableNode)
        {
            string query = @"//div[@class = 'numRes']";
            HtmlNode numResults = tableNode.SelectSingleNode(query);

            if (null == numResults)
            {
                return 1;
            }

            string tempString = Regex.Match(numResults.InnerText, @"\d+ results").Value;
            tempString = Regex.Match(tempString, @"\d+").Value;

            var numPages = Math.Ceiling(Convert.ToDouble(tempString) / 30);// 30 results per page

            return Convert.ToInt32(numPages);
        }

        /// <summary>
        /// Finds the number of expected results in a stats table
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        protected static int GetNumExpectedResults(HtmlNode tableNode)
        {
            string query = @"//div[@class = 'numRes']";
            HtmlNode numResults = tableNode.SelectSingleNode(query);

            if (null == numResults)
            {
                return 0;
            }
            else
            {

                string tempString = Regex.Match(numResults.InnerText, @"\d+ results").Value;
                tempString = Regex.Match(tempString, @"\d+").Value;
                return Convert.ToInt32(tempString);
            }
        }

        /// <summary>
        /// Remove the footer from the stats table
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        protected static void RemoveFooter(HtmlNode tableNode)
        {
            HtmlNode footNode = tableNode.SelectSingleNode(@"//tfoot");
            tableNode.RemoveChild(footNode);
        }
    }
}
