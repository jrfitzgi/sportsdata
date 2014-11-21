using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;
using SportsData.Models;

namespace SportsData.Mlb
{
    public class MlbAttendanceQuery
    {
        private const string baseAddress = "http://espn.go.com/";
        private const string preSeasonFormatString = "/mlb/team/schedule/_/name/{0}/year/{1}/seasontype/1"; // team short name, year
        private const string regSeasonFormatString = "/mlb/team/schedule/_/name/{0}/year/{1}/seasontype/2/half/{2}"; // team short name, year, half (1 or 2)
        private const string postSeasonFormatString = "/mlb/team/schedule/_/name/{0}/year/{1}/seasontype/3"; // team short name, year

        public static List<MlbGameSummaryModel> GetSeason(MlbSeasonType mlbSeasonType, int seasonYear)
        {
            List<MlbGameSummaryModel> results = new List<MlbGameSummaryModel>();

            foreach (MlbTeamShortName mlbTeam in Enum.GetValues(typeof(MlbTeamShortName)))
            {
                List<MlbGameSummaryModel> teamResults = MlbAttendanceQuery.GetSeasonForTeam(mlbSeasonType, mlbTeam, seasonYear);
                if (null != teamResults)
                {
                    results.AddRange(teamResults);
                }
            }

            return results;
        }

        public static List<MlbGameSummaryModel> GetSeasonForTeam(MlbSeasonType mlbSeasonType, MlbTeamShortName mlbTeam, int seasonYear)
        {
            string relativeUrl;
            List<MlbGameSummaryModel> results;

            switch (mlbSeasonType)
            {
                case MlbSeasonType.Spring:
                    relativeUrl = String.Format(MlbAttendanceQuery.preSeasonFormatString, mlbTeam.ToString(), seasonYear);
                    results = MlbAttendanceQuery.GetPage(relativeUrl, mlbTeam, seasonYear);
                    break;
                case MlbSeasonType.Regular:

                    // Collect first half of the season
                    relativeUrl = String.Format(MlbAttendanceQuery.regSeasonFormatString, mlbTeam.ToString(), seasonYear, 1);
                    List<MlbGameSummaryModel> firstHalf = MlbAttendanceQuery.GetPage(relativeUrl, mlbTeam, seasonYear);

                    // Collect second half of the season
                    relativeUrl = String.Format(MlbAttendanceQuery.regSeasonFormatString, mlbTeam.ToString(), seasonYear, 2);
                    List<MlbGameSummaryModel> secondHalf = MlbAttendanceQuery.GetPage(relativeUrl, mlbTeam, seasonYear);

                    // Merge them together
                    results = new List<MlbGameSummaryModel>();
                    results.AddRange(firstHalf);
                    results.AddRange(secondHalf);
                    break;
                case MlbSeasonType.PostSeason:
                    relativeUrl = String.Format(MlbAttendanceQuery.postSeasonFormatString, mlbTeam.ToString(), seasonYear);
                    results = MlbAttendanceQuery.GetPage(relativeUrl, mlbTeam, seasonYear);
                    break;
                default:
                    throw new ArgumentException(String.Format("Unrecognized season type {0}", mlbSeasonType.ToString()));
            }

            // Add the season type and year to every item
            if (null != results)
            {
                results.ForEach(x =>
                    {
                        x.MlbSeasonType = mlbSeasonType;
                        x.Year = seasonYear;
                    });
            }

            return results;
        }

        private static List<MlbGameSummaryModel> GetPage(string relativeUrl, MlbTeamShortName mlbTeam, int seasonYear)
        {
            // Construct the url
            Uri url = new Uri(relativeUrl, UriKind.Relative);

            // Make an http request
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(MlbAttendanceQuery.baseAddress);

            Task<string> httpResponseMessage = httpClient.GetStringAsync(url);
            string responseString = null;
            int numRetries = 5;
            for (int i = 0; i < numRetries; i++)
            {
                try
                {
                    httpResponseMessage = httpClient.GetStringAsync(url);
                    responseString = httpResponseMessage.Result;
                    break;
                }
                catch (Exception e)
                {
                    if (i == numRetries - 1)
                    {
                        throw;
                    }
                }
            }

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(responseString);

            HtmlNode tableNode = document.DocumentNode.SelectSingleNode(@"//table[@class='tablehead']");
            HtmlNodeCollection rows = tableNode.SelectNodes(@".//tr[contains(@class,'evenrow') or contains(@class,'oddrow')]");

            List<MlbGameSummaryModel> games = MlbAttendanceQuery.ParseRows(rows, mlbTeam, seasonYear);

            return games;
        }

        private static List<MlbGameSummaryModel> ParseRows(HtmlNodeCollection rows, MlbTeamShortName mlbTeam, int seasonYear)
        {
            // error checks
            if (null == rows || rows.Count == 0)
            {
                return null;
            }

            HtmlNode firstRow = rows[0];
            HtmlNode firstColumn = firstRow.SelectSingleNode(@".//td");
            if (firstColumn.InnerText.IndexOf("no schedule", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                // There are no games in this page
                return null;
            }

            List<MlbGameSummaryModel> games = new List<MlbGameSummaryModel>();
            foreach (HtmlNode row in rows)
            {
                HtmlNodeCollection columns = row.SelectNodes(@".//td");
                MlbGameSummaryModel game = MlbAttendanceQuery.ParseRow(columns, mlbTeam, seasonYear, true);
                if (null != game)
                {
                    games.Add(game);
                }
            }

            return games;
        }

        private static MlbGameSummaryModel ParseRow(HtmlNodeCollection columns, MlbTeamShortName mlbTeam, int seasonYear, bool includeHomeGamesOnly = true)
        {
            MlbGameSummaryModel game = new MlbGameSummaryModel();

            // Check if this is a home or away game and if we should proceed parsing
            HtmlNode gameStatusNode = columns[1].SelectSingleNode(@".//li[@class='game-status']");
            if (gameStatusNode.InnerText.Equals("@") && includeHomeGamesOnly)
            {
                // This is an away game and we don't want to include away games so return null
                return null;
            }

            // Game Date
            HtmlNode dateNode = columns[0];
            DateTime date = Convert.ToDateTime(dateNode.InnerText + " " + seasonYear);
            game.Date = date;

            if (date >= DateTime.Now.Date) // Don't retrieve games from today or future
            {
                return null;
            }

            // Determine the Opponent
            HtmlNode opponentNode = columns[1];
            HtmlNode opponentTeamNode = opponentNode.SelectSingleNode(@".//li[@class='team-name']");
            string opponentTeamCity = opponentTeamNode.InnerText;

            // Check if the game was postponed
            if (columns[2].InnerText.Equals("postponed", StringComparison.InvariantCultureIgnoreCase) || columns[3].InnerText.Equals("delayed", StringComparison.InvariantCultureIgnoreCase) || columns[3].InnerText.Equals("suspended", StringComparison.InvariantCultureIgnoreCase))
            {
                // The game was postponed. Figure out home/away teams and then return
                game.Postponed = true;
                MlbAttendanceQuery.DetermineHomeAndAway(gameStatusNode, mlbTeam, opponentTeamCity, ref game);
                return game;
            }
            else
            {
                game.Postponed = false;
            }

            // Game Result
            HtmlNode gameResultNode = columns[2];
            HtmlNode scoreNode = gameResultNode.SelectSingleNode(@".//li[contains(@class,'score')]");

            // Check if the score can be parsed out and if not then return
            if (null == scoreNode)
            {
                MlbAttendanceQuery.DetermineHomeAndAway(gameStatusNode, mlbTeam, opponentTeamCity, ref game);
                return game;
            }

            // Check if there were extra innings
            string score = scoreNode.InnerText;
            string fToken = "f/"; // This string implies there were extra innings
            int fIndex = score.IndexOf(fToken, StringComparison.InvariantCultureIgnoreCase);
            if (fIndex >= 0)
            {
                game.Innings = Convert.ToInt32(score.Substring(fIndex + fToken.Length));
                score = score.Substring(0, fIndex).Trim();
            }
            else
            {
                game.Innings = 9;
            }

            int winningScore;
            int losingScore;
            try
            {
                winningScore = score.Split('-').Max(x => Convert.ToInt32(x));
                losingScore = score.Split('-').Min(x => Convert.ToInt32(x));
            }
            catch (FormatException)
            {
                winningScore = 0;
                losingScore = 0;
                MlbAttendanceQuery.DetermineHomeAndAway(gameStatusNode, mlbTeam, opponentTeamCity, ref game);
                return game;
            }

            // Figure out if the home team won or lost
            HtmlNode winLossNode = gameResultNode.SelectSingleNode(@".//li[contains(@class,'game-status')]");
            int mlbTeamScore;
            int opponentScore;
            if (winLossNode.Attributes["class"].Value.IndexOf("win", StringComparison.InvariantCultureIgnoreCase) >= 0) // case-insensitive 'contains'
            {
                mlbTeamScore = winningScore;
                opponentScore = losingScore;
            }
            else if (winLossNode.Attributes["class"].Value.IndexOf("loss", StringComparison.InvariantCultureIgnoreCase) >= 0) // case-insensitive 'contains'
            {
                mlbTeamScore = losingScore;
                opponentScore = winningScore;
            }
            else if (winLossNode.Attributes["class"].Value.IndexOf("tie", StringComparison.InvariantCultureIgnoreCase) >= 0) // case-insensitive 'contains'
            {
                mlbTeamScore = winningScore;
                opponentScore = winningScore;
            }
            else
            {
                throw new Exception("Could not determine win or loss");
            }

            // Get Win-Loss record for mlbTeam
            HtmlNode winLossRecordNode = columns[3];
            string[] winLossRecord = winLossRecordNode.InnerText.Split('-');
            if (null != winLossRecord && winLossRecord.Length == 2)
            {
                game.WinsToDate = Convert.ToInt32(winLossRecord[0]);
                game.LossesToDate = Convert.ToInt32(winLossRecord[1]);
            }
            else
            {
                // do not set any values since we couldn't parse out the win-loss record
            }

            // Get pitchers
            HtmlNode winningPitcherNode = columns[4].SelectSingleNode(@".//a");
            game.WPitcher = winningPitcherNode != null ? winningPitcherNode.InnerText : null;
            HtmlNode losingPitcherNode = columns[5].SelectSingleNode(@".//a");
            game.LPitcher = losingPitcherNode != null ? losingPitcherNode.InnerText : null;
            HtmlNode savingPitcherNode = columns[6].SelectSingleNode(@".//a");
            game.SavePitcher = savingPitcherNode != null ? savingPitcherNode.InnerText : null;

            // Determine home and away teams and which was the winner or loser
            // Note: gameStatusNode was initialized at the start of the method
            if (gameStatusNode.InnerText.Equals("vs", StringComparison.InvariantCultureIgnoreCase))
            {
                // home game for mlbTeam 

                game.Home = mlbTeam.ToString();
                game.HomeScore = mlbTeamScore;

                string shortName = MlbAttendanceQuery.LookupShortName(opponentTeamCity);
                game.Visitor = String.IsNullOrWhiteSpace(shortName) ? opponentTeamCity : shortName;
                game.VisitorScore = opponentScore;
            }
            else if (gameStatusNode.InnerText.Equals("@", StringComparison.InvariantCultureIgnoreCase))
            {
                // away game for mlbTeam

                string shortName = MlbAttendanceQuery.LookupShortName(opponentTeamCity);
                game.Home = String.IsNullOrWhiteSpace(shortName) ? opponentTeamCity : shortName;
                game.HomeScore = opponentScore;

                game.Visitor = mlbTeam.ToString();
                game.VisitorScore = mlbTeamScore;
            }

            // Get Attendance Data
            HtmlNode attendanceNode = columns[7];
            int attendance = 0;
            Int32.TryParse(attendanceNode.InnerText, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out attendance);
            game.Attendance = attendance;

            return game;
        }

        private static void DetermineHomeAndAway(HtmlNode gameStatusNode, MlbTeamShortName mlbTeam, string opponentTeamCity, ref MlbGameSummaryModel game)
        {
            //Figure out home/away teams and then return
            if (gameStatusNode.InnerText.Equals("vs", StringComparison.InvariantCultureIgnoreCase))
            {
                // home game for mlbTeam 
                game.Home = mlbTeam.ToString();
                string shortName = MlbAttendanceQuery.LookupShortName(opponentTeamCity);
                game.Visitor = String.IsNullOrWhiteSpace(shortName) ? opponentTeamCity : shortName;
            }
            else if (gameStatusNode.InnerText.Equals("@", StringComparison.InvariantCultureIgnoreCase))
            {
                // away game for mlbTeam
                string shortName = MlbAttendanceQuery.LookupShortName(opponentTeamCity);
                game.Home = String.IsNullOrWhiteSpace(shortName) ? opponentTeamCity : shortName;
                game.Visitor = mlbTeam.ToString();
            }
        }

        private static string LookupShortName(string espnOpponentName)
        {

            MlbTeam mlbTeam = null;
            using (SportsDataContext db = new SportsDataContext())
            {
                mlbTeam = (from t in db.MlbTeam_DbSet.ToList()   // We need to use ToList() so we don't get 'ObjectContext instance has been disposed' error.
                           where t.EspnOpponentName.Equals(espnOpponentName, StringComparison.InvariantCulture)
                           select t).FirstOrDefault();
            }

            if (null == mlbTeam)
            {
                return null;
            }
            else
            {
                return mlbTeam.ShortNameId.ToString();
            }
        }

    }
}
