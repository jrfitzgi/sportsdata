using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace SportsData.Mlb
{
    public class MlbAttendanceQuery
    {
        private const string baseAddress = "http://espn.go.com/";
        private const string preSeasonFormatString = "/mlb/team/schedule/_/name/{0}/year/{1}/seasontype/1"; // team short name, year
        private const string regSeasonFormatString = "/mlb/team/schedule/_/name/{0}/year/{1}/half/{2}"; // team short name, year, half (1 or 2)
        private const string postSeasonFormatString = "/mlb/team/schedule/_/name/{0}/year/{1}/seasontype/3"; // team short name, year

        public static List<MlbGameSummary> GetSeason(MlbSeasonType mlbSeasonType, MlbTeamShortName mlbTeam, int seasonYear)
        {
            return MlbAttendanceQuery.GetPage(mlbSeasonType, mlbTeam, seasonYear);
        }

        private static List<MlbGameSummary> GetPage(MlbSeasonType mlbSeasonType, MlbTeamShortName mlbTeam, int seasonYear)
        {
            // Construct the url
            string urlString = String.Format(preSeasonFormatString, mlbTeam.ToString(), seasonYear);
            Uri url = new Uri(urlString, UriKind.Relative);

            // Make an http request
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(MlbAttendanceQuery.baseAddress);

            Task<string> httpResponseMessage = httpClient.GetStringAsync(url);
            string responseString = httpResponseMessage.Result;

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseString);

            HtmlNode tableNode = document.DocumentNode.SelectSingleNode(@"//table[@class='tablehead']");
            HtmlNodeCollection rows = tableNode.SelectNodes(@".//tr[contains(@class,'evenrow') or contains(@class,'oddrow')]");

            List<MlbGameSummary> games = MlbAttendanceQuery.ParseRows(rows, mlbTeam, seasonYear);

            return games;
        }

        private static List<MlbGameSummary> ParseRows(HtmlNodeCollection rows, MlbTeamShortName mlbTeam, int seasonYear)
        {
            List<MlbGameSummary> games = new List<MlbGameSummary>();
            foreach (HtmlNode row in rows)
            {
                HtmlNodeCollection columns = row.SelectNodes(@".//td");
                MlbGameSummary game = MlbAttendanceQuery.ParseRow(columns, mlbTeam, seasonYear);
                if (null != game)
                {
                    games.Add(game);
                }
            }

            return games;
        }

        private static MlbGameSummary ParseRow(HtmlNodeCollection columns, MlbTeamShortName mlbTeam, int seasonYear, bool includeHomeGamesOnly = true)
        {
            MlbGameSummary game = new MlbGameSummary();

            // Date
            HtmlNode dateNode = columns[0];
            DateTime date = Convert.ToDateTime(dateNode.InnerText + " " + seasonYear);
            game.Date = date;

            // Determine the Opponent
            // Only include home games if specified
            HtmlNode opponentNode = columns[1];

            HtmlNode opponentTeamNode = opponentNode.SelectSingleNode(@".//li[@class='team-name']");
            string opponentTeamCity = opponentTeamNode.InnerText;

            HtmlNode gameStatusNode = opponentNode.SelectSingleNode(@".//li[@class='game-status']");
            if (gameStatusNode.InnerText.Equals("@") && includeHomeGamesOnly)
            {
                return null;
            }

            // Determine home and away teams
            if (gameStatusNode.InnerText.Equals("@"))
            {
                // away game
                game.Home = MlbAttendanceQuery.LookupShortName(opponentTeamCity).ToString();
                game.Visitor = mlbTeam.ToString();
            }
            else
            {
                // home game
                game.Home = mlbTeam.ToString();
                game.Visitor = MlbAttendanceQuery.LookupShortName(opponentTeamCity).ToString();
            }




            return game;
        }

        private static MlbTeamShortName LookupShortName(string city)
        {
            SportsDataContext db = new SportsDataContext();
            MlbTeamShortName shortName = db.MlbTeams.Where(x => x.City.Equals(city, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().ShortNameId;
            return shortName;
        }

        //protected static void SaveRowsToDb(HtmlNode node, int gameType)
        //{
        //    // Verify that the tables contain rows
        //    if (null == node || null == node.SelectNodes("./tbody/tr"))
        //    {
        //        // This is unexpected
        //        return;
        //    }

        //    foreach (HtmlNode row in node.SelectNodes("./tbody/tr"))
        //    {
        //        NhlStatsQuery.SaveRowToDb(row, gameType);
        //    }
        //}



        ///// <summary>
        ///// Parses the data out of an html tr row and saves it in NhlStatsContext db
        ///// </summary>
        //protected static void SaveRowToDb(HtmlNode tr, int gameType)
        //{
        //    HtmlNodeCollection tds = tr.SelectNodes("./td");

        //    using (NhlAttendanceContext db = new NhlAttendanceContext())
        //    {
        //        NhlGameSummary gameSummary = new NhlGameSummary();
        //        gameSummary.Date = Convert.ToDateTime(tds[0].InnerText.Replace("'", "/"));
        //        gameSummary.Home = tds[3].InnerText;

        //        var result = from g in db.GameSummaries
        //                     where g.Date == gameSummary.Date && g.Home == gameSummary.Home
        //                     select g;
        //        if (result.Any())
        //        {
        //            // do not add the record if it already exists
        //            return;
        //        }

        //        gameSummary.Season = NhlGameSummary.GetSeason(gameSummary.Date).Item2;
        //        gameSummary.GameType = gameType;
        //        gameSummary.Visitor = tds[1].InnerText;
        //        gameSummary.VisitorScore = ConvertStringToInt(tds[2].InnerText);
        //        gameSummary.HomeScore = ConvertStringToInt(tds[4].InnerText);
        //        gameSummary.OS = tds[5].InnerText;
        //        gameSummary.WGoalie = tds[6].InnerText;
        //        gameSummary.WGoal = tds[7].InnerText;
        //        gameSummary.VisitorShots = ConvertStringToInt(tds[8].InnerText);
        //        gameSummary.VisitorPPGF = ConvertStringToInt(tds[9].InnerText);
        //        gameSummary.VisitorPPOpp = ConvertStringToInt(tds[10].InnerText);
        //        gameSummary.VisitorPIM = ConvertStringToInt(tds[11].InnerText);
        //        gameSummary.HomeShots = ConvertStringToInt(tds[12].InnerText);
        //        gameSummary.HomePPGF = ConvertStringToInt(tds[13].InnerText);
        //        gameSummary.HomePPOpp = ConvertStringToInt(tds[14].InnerText);
        //        gameSummary.HomePIM = ConvertStringToInt(tds[15].InnerText);
        //        gameSummary.Att = ConvertStringToInt(tds[16].InnerText.Replace(",", String.Empty));

        //        db.GameSummaries.Add(gameSummary);
        //        db.SaveChanges();
        //    }
        //}

        //private static int GetNumResultsInDb(int season, int gameType)
        //{
        //    NhlAttendanceContext db = new NhlAttendanceContext();

        //    int intSeason = Convert.ToInt32(season);
        //    var results = (from g in db.GameSummaries
        //                   where g.Season == intSeason && g.GameType == gameType
        //                   orderby g.Date descending
        //                   select g);

        //    return results.Count();
        //}

        //private static int ConvertStringToInt(string s)
        //{
        //    int result;
        //    bool success = Int32.TryParse(s, out result);

        //    if (!success)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return result;
        //    }
        //}

        //private static HtmlTableRow BuildRow(NhlGameSummary gameSummary)
        //{
        //    HtmlTableRow htmlRow = new HtmlTableRow();

        //    HtmlTableCell cell;

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.Date.ToShortDateString()));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(Enum.GetName(typeof(NhlGameSummary.GameTypes), gameSummary.GameType)));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.Visitor));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.VisitorScore.ToString()));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.Home));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.HomeScore.ToString()));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.OS));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.WGoalie));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.WGoal));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.VisitorShots.ToString()));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.VisitorPPGF.ToString()));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.VisitorPPOpp.ToString()));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.VisitorPIM.ToString()));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.HomeShots.ToString()));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.HomePPGF.ToString()));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.HomePPOpp.ToString()));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.HomePIM.ToString()));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl(gameSummary.Att.ToString()));
        //    htmlRow.Cells.Add(cell);

        //    return htmlRow;
        //}

        //private static HtmlTableRow BuildTitleRow(int season, int numResults)
        //{
        //    HtmlTableRow htmlRow = new HtmlTableRow();

        //    HtmlTableCell cell;

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Season: <br>" + (season-1) + "-" + season));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Results: " + numResults));
        //    htmlRow.Cells.Add(cell);

        //    return htmlRow;
        //}

        //private static HtmlTableRow BuildHeaderRow(int season)
        //{
        //    HtmlTableRow htmlRow = new HtmlTableRow();

        //    HtmlTableCell cell;

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Date"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Game Type"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Visitor"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Visitor Score"));
        //    htmlRow.Cells.Add(cell);
        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Home"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Home Score"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("OT/SO"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Winning Goalie"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Winning Goal"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Visitor Shots"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Visitor PPGF"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Visitor PPOpp"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Visitor PIM"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Home Shots"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Home PPGF"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Home PPOpp"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Home PIM"));
        //    htmlRow.Cells.Add(cell);

        //    cell = new HtmlTableCell();
        //    cell.Controls.Add(new LiteralControl("Attendance"));
        //    htmlRow.Cells.Add(cell);

        //    return htmlRow;
        //}


        //protected static int CountRows(HtmlNode table)
        //{
        //    // Verify that the tables contain rows
        //    if (null == table.SelectNodes("./tbody/tr"))
        //    {
        //        // This is unexpected
        //        return 0;
        //    }

        //    HtmlNodeCollection rows = table.SelectNodes("./tbody/tr");
        //    return rows.Count;
        //}

        ///// <summary>
        ///// Finds the stats table in the html document and returns it as an HTMLNode
        ///// </summary>
        ///// <param name="htmlString"></param>
        ///// <returns></returns>
        //protected static HtmlNode GetStatsTable(string htmlString)
        //{
        //    HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
        //    htmlDocument.LoadHtml(htmlString);

        //    foreach (HtmlNode table in htmlDocument.DocumentNode.SelectNodes("//table"))
        //    {
        //        HtmlAttribute classAttribute = table.Attributes["class"];
        //        if (null != classAttribute && classAttribute.Value.Equals("data stats", StringComparison.CurrentCultureIgnoreCase))
        //        {
        //            return table;
        //        }
        //    }

        //    return null;
        //}

        ///// <summary>
        ///// Finds the number of pages in a stats table
        ///// </summary>
        ///// <param name="htmlString"></param>
        ///// <returns></returns>
        //protected static int GetNumPages(HtmlNode tableNode)
        //{
        //    string query = @"//div[@class = 'numRes']";
        //    HtmlNode numResults = tableNode.SelectSingleNode(query);

        //    if (null == numResults)
        //    {
        //        return 1;
        //    }

        //    string tempString = Regex.Match(numResults.InnerText, @"\d+ results").Value;
        //    tempString = Regex.Match(tempString, @"\d+").Value;

        //    var numPages = Math.Ceiling(Convert.ToDouble(tempString) / 30);// 30 results per page

        //    return Convert.ToInt32(numPages);
        //}

        ///// <summary>
        ///// Finds the number of expected results in a stats table
        ///// </summary>
        ///// <param name="htmlString"></param>
        ///// <returns></returns>
        //protected static int GetNumExpectedResults(HtmlNode tableNode)
        //{
        //    string query = @"//div[@class = 'numRes']";
        //    HtmlNode numResults = tableNode.SelectSingleNode(query);

        //    if (null == numResults)
        //    {
        //        return 0;
        //    }
        //    else
        //    {

        //        string tempString = Regex.Match(numResults.InnerText, @"\d+ results").Value;
        //        tempString = Regex.Match(tempString, @"\d+").Value;
        //        return Convert.ToInt32(tempString);
        //    }
        //}

        ///// <summary>
        ///// Remove the footer from the stats table
        ///// </summary>
        ///// <param name="htmlString"></param>
        ///// <returns></returns>
        //protected static void RemoveFooter(HtmlNode tableNode)
        //{
        //    HtmlNode footNode = tableNode.SelectSingleNode(@"//tfoot");
        //    tableNode.RemoveChild(footNode);
        //}
    }
}
