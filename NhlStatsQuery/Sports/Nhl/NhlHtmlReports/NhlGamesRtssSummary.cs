﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using EntityFramework.BulkInsert.Extensions;
using HtmlAgilityPack;
using SportsData;
using SportsData.Models;

namespace SportsData.Nhl
{
    public class NhlGamesRtssSummary : NhlHtmlReportBase
    {
        public static void UpdateSeason([Optional] int year, [Optional] DateTime fromDate, [Optional] bool forceOverwrite)
        {
            // Get the RtssReports for the specified year. Exclude recent games that may not be done (UtcNow - 1)
            List<Nhl_Games_Rtss> models = NhlHtmlReportBase.GetRtssReports(year, fromDate).Where(m=>m.Date < DateTime.UtcNow.AddDays(-1).Date).ToList();
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
                Console.WriteLine("Start saving {0} to {1}", results.Count, db.Database.Connection.ConnectionString);

                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.ValidateOnSaveEnabled = false;

                int counter = 0;
                int totalCounter = 0;
                int batchSize = 10;
                foreach (var model in results)
                {
                    counter++;
                    totalCounter++;

                    if (model.Id != 0)
                    {
                        db.Nhl_Games_Rtss_Summary_DbSet.Attach(model);
                        db.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        db.Entry(model).State = EntityState.Added;
                    }

                    if (counter >= batchSize)
                    {
                        db.SaveChanges();
                        counter = 0;

                        Console.WriteLine("Saved {0} of {1}", totalCounter, results.Count);
                    }
                }

                db.SaveChanges();
                Console.WriteLine("Saved {0} of {1}", totalCounter, results.Count);
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

            HtmlNode mainTableNode = documentNode.SelectSingleNode(@".//table[@id='MainTable']");
            HtmlNode gameSummaryTableNode = documentNode.SelectSingleNode(@".//table[.//table[@id='GameInfo']]");

            if (null == gameSummaryTableNode) { return null; }

            #region Get team names, score, game numbers

            HtmlNode visitorTableNode = gameSummaryTableNode.SelectSingleNode(@".//table[@id='Visitor']");
            HtmlNode homeTableNode = gameSummaryTableNode.SelectSingleNode(@".//table[@id='Home']");

            HtmlNode visitorScoreNode = visitorTableNode.SelectNodes(@".//tr").ElementAt(1).SelectNodes(@".//tr/td").ElementAt(1);
            HtmlNode homeScoreNode = homeTableNode.SelectNodes(@".//tr").ElementAt(1).SelectNodes(@".//tr/td").ElementAt(1);

            model.VisitorScore = Convert.ToInt32(visitorScoreNode.InnerText);
            model.HomeScore = Convert.ToInt32(homeScoreNode.InnerText);

            HtmlNode visitorNameNode = visitorTableNode.SelectNodes(String.Format(@".{0}/tr", TBODY)).ElementAt(2).SelectSingleNode(@"./td");
            HtmlNode homeNameNode = homeTableNode.SelectNodes(String.Format(@".{0}/tr", TBODY)).ElementAt(2).SelectSingleNode(@"./td");

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

            HtmlNode gameInfoTableNode = gameSummaryTableNode.SelectSingleNode(@".//table[@id='GameInfo']");
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

            #region Scoring Summary

            //model.ScoringSummary = new List<Nhl_Games_Rtss_Summary_ScoringSummary_Item>();

            HtmlNode scoringSummaryTableNode = mainTableNode.SelectSingleNode(@".//table[.//td[text()[contains(.,'SCORING SUMMARY')]]]/../..").NextSibling.NextSibling.SelectSingleNode(@".//table");
            HtmlNodeCollection scoringSummaryTableRows = scoringSummaryTableNode.SelectNodes(@".//tr");

            if (scoringSummaryTableRows != null && scoringSummaryTableRows.Count > 0)
            {
                HtmlNode scoringSummaryTableTeam1Node = scoringSummaryTableRows[0].SelectNodes(@".//td")[8];
                HtmlNode scoringSummaryTableTeam2Node = scoringSummaryTableRows[0].SelectNodes(@".//td")[9];

                string scoringSummaryOnIceTeam1 = scoringSummaryTableTeam1Node.InnerText.Substring(0, scoringSummaryTableTeam1Node.InnerText.IndexOf(' '));
                string scoringSummaryOnIceTeam2 = scoringSummaryTableTeam2Node.InnerText.Substring(0, scoringSummaryTableTeam2Node.InnerText.IndexOf(' '));

                for (int i = 1; i < scoringSummaryTableRows.Count; i++)
                {
                    HtmlNodeCollection scoringSummaryRowFields = scoringSummaryTableRows[i].SelectNodes(@".//td");
                    Nhl_Games_Rtss_Summary_ScoringSummary_Item scoringSummaryItem = new Nhl_Games_Rtss_Summary_ScoringSummary_Item();
                    scoringSummaryItem.Date = model.Date;
                    scoringSummaryItem.GoalNumber = NhlBaseClass.ConvertStringToInt(scoringSummaryRowFields[0].InnerText);
                    scoringSummaryItem.Period = scoringSummaryRowFields[1].InnerText;
                    scoringSummaryItem.TimeInSeconds = NhlBaseClass.ConvertMinutesToSeconds(scoringSummaryRowFields[2].InnerText);
                    scoringSummaryItem.Strength = scoringSummaryRowFields[3].InnerText;
                    scoringSummaryItem.Team = scoringSummaryRowFields[4].InnerText;

                    string goalScorerText = scoringSummaryRowFields[5].InnerText;
                    int index1 = goalScorerText.IndexOf(' ');
                    int index2 = goalScorerText.IndexOf('(');
                    int index3 = goalScorerText.IndexOf(')');
                    if (index1 >= 0)
                    {
                        scoringSummaryItem.GoalScorerPlayerNumber = NhlBaseClass.ConvertStringToInt(goalScorerText.Substring(0, index1));
                    }

                    if (index2 >= index1 + 1)
                    {
                        scoringSummaryItem.GoalScorer = goalScorerText.Substring(index1 + 1, index2 - index1 - 1);
                    }
                    else
                    {
                        scoringSummaryItem.GoalScorer = goalScorerText;
                    }

                    if (index3 >= index2 + 1)
                    {
                        scoringSummaryItem.GoalScorerGoalNumber = NhlBaseClass.ConvertStringToInt(goalScorerText.Substring(index2 + 1, index3 - index2 - 1));
                    }


                    // If this was a penalty shot, there is one less column after Assist1
                    int penaltyShotOffset = 0;

                    string assist1Text = scoringSummaryRowFields[6].InnerText;

                    if (assist1Text.IndexOf("Penalty Shot", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        penaltyShotOffset = -1;
                    }

                    index1 = assist1Text.IndexOf(' ');
                    index2 = assist1Text.IndexOf('(');
                    index3 = assist1Text.IndexOf(')');
                    if (index1 >= 0)
                    {
                        scoringSummaryItem.Assist1PlayerNumber = NhlBaseClass.ConvertStringToInt(assist1Text.Substring(0, index1));
                    }
                    if (index2 >= index1 + 1)
                    {
                        scoringSummaryItem.Assist1 = assist1Text.Substring(index1 + 1, index2 - index1 - 1);
                    }
                    else
                    {
                        scoringSummaryItem.Assist1 = assist1Text;
                    }

                    if (index3 >= index2 + 1)
                    {
                        scoringSummaryItem.Assist1AssistNumber = NhlBaseClass.ConvertStringToInt(assist1Text.Substring(index2 + 1, index3 - index2 - 1));
                    }

                    string assist2Text = scoringSummaryRowFields[7 + penaltyShotOffset].InnerText;
                    index1 = assist2Text.IndexOf(' ');
                    index2 = assist2Text.IndexOf('(');
                    index3 = assist2Text.IndexOf(')');
                    if (index1 >= 0)
                    {
                        scoringSummaryItem.Assist2PlayerNumber = NhlBaseClass.ConvertStringToInt(assist2Text.Substring(0, index1));
                    }

                    if (index2 >= index1 + 1)
                    {
                        scoringSummaryItem.Assist2 = assist2Text.Substring(index1 + 1, index2 - index1 - 1);
                    }
                    else
                    {
                        scoringSummaryItem.Assist2 = assist2Text;
                    }

                    if (index3 >= index2 + 1)
                    {
                        scoringSummaryItem.Assist2AssistNumber = NhlBaseClass.ConvertStringToInt(assist2Text.Substring(index2 + 1, index3 - index2 - 1));
                    }

                    scoringSummaryItem.VisitorOnIce = NhlBaseClass.RemoveAllWhitespace(scoringSummaryRowFields[8 + penaltyShotOffset].InnerText);
                    scoringSummaryItem.HomeOnIce = NhlBaseClass.RemoveAllWhitespace(scoringSummaryRowFields[9 + penaltyShotOffset].InnerText);

                    model.ScoringSummary.Add(scoringSummaryItem);

                }
            }

            #endregion

            #region Penalty Summary

            // Get the 4 child tables that have width=50%
            HtmlNodeCollection penaltySummaryTableNodes = mainTableNode.SelectNodes(@".//table[@id='PenaltySummary']//table//table//td[@width='50%']/table");

            HtmlNodeCollection visitorPenaltySummaryNodes = penaltySummaryTableNodes[0].SelectNodes(String.Format(@".{0}/tr", TBODY));
            HtmlNodeCollection homePenaltySummaryNodes = penaltySummaryTableNodes[1].SelectNodes(String.Format(@".{0}/tr", TBODY));

            for (int i = 1; i < visitorPenaltySummaryNodes.Count; i++)
            {
                Nhl_Games_Rtss_Summary_PenaltySummary_Item penaltySummaryItem = new Nhl_Games_Rtss_Summary_PenaltySummary_Item();
                penaltySummaryItem.Team = model.Visitor;
                penaltySummaryItem.Date = model.Date;

                HtmlNodeCollection penaltySummaryRowFields = visitorPenaltySummaryNodes[i].SelectNodes(@"./td");

                penaltySummaryItem.PenaltyNumber = NhlBaseClass.ConvertStringToInt(penaltySummaryRowFields[0].InnerText);
                penaltySummaryItem.Period = penaltySummaryRowFields[1].InnerText;
                penaltySummaryItem.TimeInSeconds = NhlBaseClass.ConvertMinutesToSeconds(penaltySummaryRowFields[2].InnerText);

                HtmlNodeCollection penaltySummaryPlayerTableRows = penaltySummaryRowFields[3].SelectNodes(@".//td");
                penaltySummaryItem.PlayerNumber = NhlBaseClass.ConvertStringToInt(penaltySummaryPlayerTableRows[0].InnerText);
                penaltySummaryItem.Name = penaltySummaryPlayerTableRows[3].InnerText;
                Debug.Assert(!String.IsNullOrWhiteSpace(penaltySummaryItem.Name));

                penaltySummaryItem.PIM = NhlBaseClass.ConvertStringToInt(penaltySummaryRowFields[4].InnerText);
                penaltySummaryItem.Penalty = penaltySummaryRowFields[5].InnerText;

                model.PenaltySummary.Add(penaltySummaryItem);

            }

            for (int i = 1; i < homePenaltySummaryNodes.Count; i++)
            {
                Nhl_Games_Rtss_Summary_PenaltySummary_Item penaltySummaryItem = new Nhl_Games_Rtss_Summary_PenaltySummary_Item();

                HtmlNodeCollection penaltySummaryRowFields = homePenaltySummaryNodes[i].SelectNodes(@"./td");
                penaltySummaryItem.Team = model.Home;
                penaltySummaryItem.Date = model.Date;

                penaltySummaryItem.PenaltyNumber = NhlBaseClass.ConvertStringToInt(penaltySummaryRowFields[0].InnerText);
                penaltySummaryItem.Period = penaltySummaryRowFields[1].InnerText;
                penaltySummaryItem.TimeInSeconds = NhlBaseClass.ConvertMinutesToSeconds(penaltySummaryRowFields[2].InnerText);

                HtmlNodeCollection penaltySummaryPlayerTableRows = penaltySummaryRowFields[3].SelectNodes(@".//td");
                penaltySummaryItem.PlayerNumber = NhlBaseClass.ConvertStringToInt(penaltySummaryPlayerTableRows[0].InnerText);
                penaltySummaryItem.Name = penaltySummaryPlayerTableRows[3].InnerText;
                Debug.Assert(!String.IsNullOrWhiteSpace(penaltySummaryItem.Name));

                penaltySummaryItem.PIM = NhlBaseClass.ConvertStringToInt(penaltySummaryRowFields[4].InnerText);
                penaltySummaryItem.Penalty = penaltySummaryRowFields[5].InnerText;

                model.PenaltySummary.Add(penaltySummaryItem);

            }

            // Ignore these sections. They can be calculated by Penalty Summary and Power Play sections.
            //HtmlNodeCollection visitorPenaltySummaryTotalsNodes = penaltySummaryTableNodes[2].SelectNodes(String.Format(@".{0}/tr", TBODY));
            //HtmlNodeCollection homePenaltySummaryTotalsNodes = penaltySummaryTableNodes[3].SelectNodes(String.Format(@".{0}/tr", TBODY)); 

            #endregion

            #region By Period Summary

            HtmlNodeCollection periodSummaryTableNodes = mainTableNode.SelectNodes(@".//td[text()[contains(.,'BY PERIOD')]]/../..//td[@width='50%']/table");

            HtmlNodeCollection periodSummaryVisitorRows = periodSummaryTableNodes[0].SelectNodes(@".//tr");
            for (int i = 1; i < periodSummaryVisitorRows.Count - 1; i++)
            {
                HtmlNodeCollection periodSummaryVisitorRowFields = periodSummaryVisitorRows[i].SelectNodes(@".//td");
                Nhl_Games_Rtss_Summary_PeriodSummary_Item periodSummaryItem = new Nhl_Games_Rtss_Summary_PeriodSummary_Item();
                periodSummaryItem.Team = model.Visitor;
                periodSummaryItem.Date = model.Date;

                periodSummaryItem.Period = periodSummaryVisitorRowFields[0].InnerText;
                periodSummaryItem.Goals = NhlBaseClass.ConvertStringToInt(periodSummaryVisitorRowFields[1].InnerText);
                periodSummaryItem.Shots = NhlBaseClass.ConvertStringToInt(periodSummaryVisitorRowFields[2].InnerText);
                periodSummaryItem.Penalties = NhlBaseClass.ConvertStringToInt(periodSummaryVisitorRowFields[3].InnerText);
                periodSummaryItem.PIM = NhlBaseClass.ConvertStringToInt(periodSummaryVisitorRowFields[4].InnerText);
                model.PeriodSummary.Add(periodSummaryItem);
            }

            HtmlNodeCollection periodSummaryHomeRows = periodSummaryTableNodes[1].SelectNodes(@".//tr");
            for (int i = 1; i < periodSummaryHomeRows.Count - 1; i++)
            {
                HtmlNodeCollection periodSummaryHomeRowFields = periodSummaryHomeRows[i].SelectNodes(@".//td");
                Nhl_Games_Rtss_Summary_PeriodSummary_Item periodSummaryItem = new Nhl_Games_Rtss_Summary_PeriodSummary_Item();
                periodSummaryItem.Team = model.Home;
                periodSummaryItem.Date = model.Date;

                periodSummaryItem.Period = periodSummaryHomeRowFields[0].InnerText;
                periodSummaryItem.Goals = NhlBaseClass.ConvertStringToInt(periodSummaryHomeRowFields[1].InnerText);
                periodSummaryItem.Shots = NhlBaseClass.ConvertStringToInt(periodSummaryHomeRowFields[2].InnerText);
                periodSummaryItem.Penalties = NhlBaseClass.ConvertStringToInt(periodSummaryHomeRowFields[3].InnerText);
                periodSummaryItem.PIM = NhlBaseClass.ConvertStringToInt(periodSummaryHomeRowFields[4].InnerText);
                model.PeriodSummary.Add(periodSummaryItem);
            }

            #endregion

            #region Power Play and Even Strength Summary

            //model.PowerPlaySummary.Team = model.Visitor;
            //model.PowerPlaySummary.Date = model.Date;
            //model.HomePowerPlaySummary.Team = model.Home;
            //model.HomePowerPlaySummary.Date = model.Date;

            HtmlNodeCollection powerPlaySummaryTableNodes = mainTableNode.SelectNodes(@".//td[text()[contains(.,'POWER PLAY')]]/../..//td[@width='50%']/table");
            HtmlNodeCollection powerPlaySummaryVisitorRows = powerPlaySummaryTableNodes[0].SelectNodes(@".//tr");
            HtmlNodeCollection powerPlaySummaryHomeRows = powerPlaySummaryTableNodes[1].SelectNodes(@".//tr");

            Nhl_Games_Rtss_Summary_PowerPlaySummary_Item powerPlayVisitorItem = new Nhl_Games_Rtss_Summary_PowerPlaySummary_Item();
            powerPlayVisitorItem.Team = model.Visitor;
            powerPlayVisitorItem.Date = model.Date;
            
            Nhl_Games_Rtss_Summary_PowerPlaySummary_Item powerPlayHomeItem = new Nhl_Games_Rtss_Summary_PowerPlaySummary_Item();
            powerPlayHomeItem.Team = model.Home;
            powerPlayHomeItem.Date = model.Date;

            string powerPlayText = String.Empty;

            // Power Play Visitor
            if (powerPlaySummaryVisitorRows != null & powerPlaySummaryVisitorRows.Count >= 2)
            {
                HtmlNodeCollection powerPlaySummaryVisitorRowFields = powerPlaySummaryVisitorRows[1].SelectNodes(@".//td");

                powerPlayText = powerPlaySummaryVisitorRowFields[0].InnerText;
                if (powerPlayText.IndexOf('-') >= 0 && powerPlayText.IndexOf('/') >= 0)
                {
                    int dash = powerPlayText.IndexOf('-');
                    int slash = powerPlayText.IndexOf('/');
                    powerPlayVisitorItem.PowerPlay5v4Goals = NhlBaseClass.ConvertStringToInt(powerPlayText.Substring(0, dash));
                    powerPlayVisitorItem.PowerPlay5v4Occurrences = NhlBaseClass.ConvertStringToInt(powerPlayText.Substring(dash + 1, slash - dash - 1));
                    powerPlayVisitorItem.PowerPlay5v4ToiSeconds = NhlBaseClass.ConvertMinutesToSeconds(powerPlayText.Substring(slash + 1, powerPlayText.Length - slash - 1));
                }
                powerPlayText = powerPlaySummaryVisitorRowFields[1].InnerText;
                if (powerPlayText.IndexOf('-') >= 0 && powerPlayText.IndexOf('/') >= 0)
                {
                    int dash = powerPlayText.IndexOf('-');
                    int slash = powerPlayText.IndexOf('/');
                    powerPlayVisitorItem.PowerPlay5v3Goals = NhlBaseClass.ConvertStringToInt(powerPlayText.Substring(0, dash));
                    powerPlayVisitorItem.PowerPlay5v3Occurrences = NhlBaseClass.ConvertStringToInt(powerPlayText.Substring(dash + 1, slash - dash - 1));
                    powerPlayVisitorItem.PowerPlay5v3ToiSeconds = NhlBaseClass.ConvertMinutesToSeconds(powerPlayText.Substring(slash + 1, powerPlayText.Length - slash - 1));
                }
                powerPlayText = powerPlaySummaryVisitorRowFields[2].InnerText;
                if (powerPlayText.IndexOf('-') >= 0 && powerPlayText.IndexOf('/') >= 0)
                {
                    int dash = powerPlayText.IndexOf('-');
                    int slash = powerPlayText.IndexOf('/');
                    powerPlayVisitorItem.PowerPlay4v3Goals = NhlBaseClass.ConvertStringToInt(powerPlayText.Substring(0, dash));
                    powerPlayVisitorItem.PowerPlay4v3Occurrences = NhlBaseClass.ConvertStringToInt(powerPlayText.Substring(dash + 1, slash - dash - 1));
                    powerPlayVisitorItem.PowerPlay4v3ToiSeconds = NhlBaseClass.ConvertMinutesToSeconds(powerPlayText.Substring(slash + 1, powerPlayText.Length - slash - 1));
                }
            }

            // Power Play Home
            if (powerPlaySummaryHomeRows != null & powerPlaySummaryHomeRows.Count >= 2)
            {
                HtmlNodeCollection powerPlaySummaryHomeRowFields = powerPlaySummaryHomeRows[1].SelectNodes(@".//td");

                powerPlayText = powerPlaySummaryHomeRowFields[0].InnerText;
                if (powerPlayText.IndexOf('-') >= 0 && powerPlayText.IndexOf('/') >= 0)
                {
                    int dash = powerPlayText.IndexOf('-');
                    int slash = powerPlayText.IndexOf('/');
                    powerPlayHomeItem.PowerPlay5v4Goals = NhlBaseClass.ConvertStringToInt(powerPlayText.Substring(0, dash));
                    powerPlayHomeItem.PowerPlay5v4Occurrences = NhlBaseClass.ConvertStringToInt(powerPlayText.Substring(dash + 1, slash - dash - 1));
                    powerPlayHomeItem.PowerPlay5v4ToiSeconds = NhlBaseClass.ConvertMinutesToSeconds(powerPlayText.Substring(slash + 1, powerPlayText.Length - slash - 1));
                }
                powerPlayText = powerPlaySummaryHomeRowFields[1].InnerText;
                if (powerPlayText.IndexOf('-') >= 0 && powerPlayText.IndexOf('/') >= 0)
                {
                    int dash = powerPlayText.IndexOf('-');
                    int slash = powerPlayText.IndexOf('/');
                    powerPlayHomeItem.PowerPlay5v3Goals = NhlBaseClass.ConvertStringToInt(powerPlayText.Substring(0, dash));
                    powerPlayHomeItem.PowerPlay5v3Occurrences = NhlBaseClass.ConvertStringToInt(powerPlayText.Substring(dash + 1, slash - dash - 1));
                    powerPlayHomeItem.PowerPlay5v3ToiSeconds = NhlBaseClass.ConvertMinutesToSeconds(powerPlayText.Substring(slash + 1, powerPlayText.Length - slash - 1));
                }
                powerPlayText = powerPlaySummaryHomeRowFields[2].InnerText;
                if (powerPlayText.IndexOf('-') >= 0 && powerPlayText.IndexOf('/') >= 0)
                {
                    int dash = powerPlayText.IndexOf('-');
                    int slash = powerPlayText.IndexOf('/');
                    powerPlayHomeItem.PowerPlay4v3Goals = NhlBaseClass.ConvertStringToInt(powerPlayText.Substring(0, dash));
                    powerPlayHomeItem.PowerPlay4v3Occurrences = NhlBaseClass.ConvertStringToInt(powerPlayText.Substring(dash + 1, slash - dash - 1));
                    powerPlayHomeItem.PowerPlay4v3ToiSeconds = NhlBaseClass.ConvertMinutesToSeconds(powerPlayText.Substring(slash + 1, powerPlayText.Length - slash - 1));
                }
            }

            HtmlNodeCollection evenStrengthSummaryTableNodes = mainTableNode.SelectNodes(@".//td[text()[contains(.,'EVEN STRENGTH')]]/../..//td[@width='50%']/table");

            HtmlNodeCollection evenStrengthSummaryVisitorRows = evenStrengthSummaryTableNodes[0].SelectNodes(@".//tr");
            HtmlNodeCollection evenStrengthSummaryHomeRows = evenStrengthSummaryTableNodes[1].SelectNodes(@".//tr");

            string evenStrengthText = String.Empty;

            // Even Strength Visitor
            if (evenStrengthSummaryVisitorRows != null & evenStrengthSummaryVisitorRows.Count >= 2)
            {
                HtmlNodeCollection evenStrengthSummaryVisitorRowFields = evenStrengthSummaryVisitorRows[1].SelectNodes(@".//td");

                evenStrengthText = evenStrengthSummaryVisitorRowFields[0].InnerText;
                if (evenStrengthText.IndexOf('-') >= 0 && evenStrengthText.IndexOf('/') >= 0)
                {
                    int dash = evenStrengthText.IndexOf('-');
                    int slash = evenStrengthText.IndexOf('/');
                    powerPlayVisitorItem.EvenStrength5v5Goals = NhlBaseClass.ConvertStringToInt(evenStrengthText.Substring(0, dash));
                    powerPlayVisitorItem.EvenStrength5v5Occurrences = NhlBaseClass.ConvertStringToInt(evenStrengthText.Substring(dash + 1, slash - dash - 1));
                    powerPlayVisitorItem.EvenStrength5v5ToiSeconds = NhlBaseClass.ConvertMinutesToSeconds(evenStrengthText.Substring(slash + 1, evenStrengthText.Length - slash - 1));
                }
                evenStrengthText = evenStrengthSummaryVisitorRowFields[1].InnerText;
                if (evenStrengthText.IndexOf('-') >= 0 && evenStrengthText.IndexOf('/') >= 0)
                {
                    int dash = evenStrengthText.IndexOf('-');
                    int slash = evenStrengthText.IndexOf('/');
                    powerPlayVisitorItem.EvenStrength4v4Goals = NhlBaseClass.ConvertStringToInt(evenStrengthText.Substring(0, dash));
                    powerPlayVisitorItem.EvenStrength4v4Occurrences = NhlBaseClass.ConvertStringToInt(evenStrengthText.Substring(dash + 1, slash - dash - 1));
                    powerPlayVisitorItem.EvenStrength4v4ToiSeconds = NhlBaseClass.ConvertMinutesToSeconds(evenStrengthText.Substring(slash + 1, evenStrengthText.Length - slash - 1));
                }
                evenStrengthText = evenStrengthSummaryVisitorRowFields[2].InnerText;
                if (evenStrengthText.IndexOf('-') >= 0 && evenStrengthText.IndexOf('/') >= 0)
                {
                    int dash = evenStrengthText.IndexOf('-');
                    int slash = evenStrengthText.IndexOf('/');
                    powerPlayVisitorItem.EvenStrength3v3Goals = NhlBaseClass.ConvertStringToInt(evenStrengthText.Substring(0, dash));
                    powerPlayVisitorItem.EvenStrength3v3Occurrences = NhlBaseClass.ConvertStringToInt(evenStrengthText.Substring(dash + 1, slash - dash - 1));
                    powerPlayVisitorItem.EvenStrength3v3ToiSeconds = NhlBaseClass.ConvertMinutesToSeconds(evenStrengthText.Substring(slash + 1, evenStrengthText.Length - slash - 1));
                }

            }

            // Even Strength Home
            if (evenStrengthSummaryHomeRows != null & evenStrengthSummaryHomeRows.Count >= 2)
            {
                HtmlNodeCollection evenStrengthSummaryHomeRowFields = evenStrengthSummaryHomeRows[1].SelectNodes(@".//td");

                evenStrengthText = evenStrengthSummaryHomeRowFields[0].InnerText;
                if (evenStrengthText.IndexOf('-') >= 0 && evenStrengthText.IndexOf('/') >= 0)
                {
                    int dash = evenStrengthText.IndexOf('-');
                    int slash = evenStrengthText.IndexOf('/');
                    powerPlayHomeItem.EvenStrength5v5Goals = NhlBaseClass.ConvertStringToInt(evenStrengthText.Substring(0, dash));
                    powerPlayHomeItem.EvenStrength5v5Occurrences = NhlBaseClass.ConvertStringToInt(evenStrengthText.Substring(dash + 1, slash - dash - 1));
                    powerPlayHomeItem.EvenStrength5v5ToiSeconds = NhlBaseClass.ConvertMinutesToSeconds(evenStrengthText.Substring(slash + 1, evenStrengthText.Length - slash - 1));
                }
                evenStrengthText = evenStrengthSummaryHomeRowFields[1].InnerText;
                if (evenStrengthText.IndexOf('-') >= 0 && evenStrengthText.IndexOf('/') >= 0)
                {
                    int dash = evenStrengthText.IndexOf('-');
                    int slash = evenStrengthText.IndexOf('/');
                    powerPlayHomeItem.EvenStrength4v4Goals = NhlBaseClass.ConvertStringToInt(evenStrengthText.Substring(0, dash));
                    powerPlayHomeItem.EvenStrength4v4Occurrences = NhlBaseClass.ConvertStringToInt(evenStrengthText.Substring(dash + 1, slash - dash - 1));
                    powerPlayHomeItem.EvenStrength4v4ToiSeconds = NhlBaseClass.ConvertMinutesToSeconds(evenStrengthText.Substring(slash + 1, evenStrengthText.Length - slash - 1));
                }
                evenStrengthText = evenStrengthSummaryHomeRowFields[2].InnerText;
                if (evenStrengthText.IndexOf('-') >= 0 && evenStrengthText.IndexOf('/') >= 0)
                {
                    int dash = evenStrengthText.IndexOf('-');
                    int slash = evenStrengthText.IndexOf('/');
                    powerPlayHomeItem.EvenStrength3v3Goals = NhlBaseClass.ConvertStringToInt(evenStrengthText.Substring(0, dash));
                    powerPlayHomeItem.EvenStrength3v3Occurrences = NhlBaseClass.ConvertStringToInt(evenStrengthText.Substring(dash + 1, slash - dash - 1));
                    powerPlayHomeItem.EvenStrength3v3ToiSeconds = NhlBaseClass.ConvertMinutesToSeconds(evenStrengthText.Substring(slash + 1, evenStrengthText.Length - slash - 1));
                }

            }

            model.PowerPlaySummary.Add(powerPlayVisitorItem);
            model.PowerPlaySummary.Add(powerPlayHomeItem);

            #endregion

            #region Goaltender Summary

            // First go through all visitor rows until we encounter end of visitor rows
            string team = model.Visitor;

            HtmlNodeCollection goaltenderSummaryTableNodes = mainTableNode.SelectSingleNode(@".//td[text()[contains(.,'GOALTENDER SUMMARY')]]/..").NextSibling.NextSibling.SelectNodes(@".//table//tr");

            for (int j = 2; j < goaltenderSummaryTableNodes.Count - 1; j++)
            {
                HtmlNodeCollection goaltenderSummaryRowFields = goaltenderSummaryTableNodes[j].SelectNodes(@".//td");

                if (goaltenderSummaryRowFields[0].InnerText.IndexOf("team totals", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    // Switch the team to the Home team and continue
                    team = model.Home;
                    j = j + 3;
                    continue;
                }

                Nhl_Games_Rtss_Summary_GoalieSummary_Item goalieItem = new Nhl_Games_Rtss_Summary_GoalieSummary_Item();
                goalieItem.Team = team;
                goalieItem.Date = model.Date;

                // If there was an empty net, there is one fewer columns so need to change the base index
                int offset = 0;
                if (goaltenderSummaryRowFields[1].InnerText.IndexOf("empty net", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    offset = -1;
                }
                else
                {
                    goalieItem.Number = NhlBaseClass.ConvertStringToInt(goaltenderSummaryRowFields[0].InnerText);

                }

                string goalieName = goaltenderSummaryRowFields[2 + offset].InnerHtml;

                string[] goalieOutcomes = { "W", "L", "OT" };

                foreach (string goalieOutcome in goalieOutcomes)
                {
                    if (goalieName.IndexOf("(" + goalieOutcome + ")", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        goalieItem.WinOrLoss = goalieOutcome;
                        goalieName = goalieName.Replace("(" + goalieOutcome.ToUpper() + ")", String.Empty);
                        goalieName = goalieName.Replace("(" + goalieOutcome.ToLower() + ")", String.Empty);
                        goalieName = goalieName.Trim();
                    }
                }

                goalieItem.Name = goalieName;

                goalieItem.ToiInSecondsEvenStrength = NhlBaseClass.ConvertMinutesToSeconds(goaltenderSummaryRowFields[3 + offset].InnerText);
                goalieItem.ToiInSecondsPowerPlay = NhlBaseClass.ConvertMinutesToSeconds(goaltenderSummaryRowFields[4 + offset].InnerText);
                goalieItem.ToiInSecondsShortHanded = NhlBaseClass.ConvertMinutesToSeconds(goaltenderSummaryRowFields[5 + offset].InnerText);
                goalieItem.ToiInSecondsTotal = NhlBaseClass.ConvertMinutesToSeconds(goaltenderSummaryRowFields[6 + offset].InnerText);

                goalieItem.GoaliePeriodSummary = new List<Nhl_Games_Rtss_Summary_GoalieSummary_Item.Nhl_Games_Rtss_Summary_GoaliePeriodSummary_Item>();

                int period = 1;
                for (int i = 7; i < goaltenderSummaryRowFields.Count - 1; i++)
                {
                    Nhl_Games_Rtss_Summary_GoalieSummary_Item.Nhl_Games_Rtss_Summary_GoaliePeriodSummary_Item goaliePeriodItem = new Nhl_Games_Rtss_Summary_GoalieSummary_Item.Nhl_Games_Rtss_Summary_GoaliePeriodSummary_Item();
                    goaliePeriodItem.Team = team;
                    goaliePeriodItem.Date = model.Date;

                    goaliePeriodItem.Period = period.ToString();
                    period++;

                    string goaliePeriodItemText = goaltenderSummaryRowFields[i + offset].InnerText;
                    if (goaliePeriodItemText.IndexOf('-') >= 0)
                    {
                        goaliePeriodItem.GoalsAgainst = NhlBaseClass.ConvertStringToInt(goaliePeriodItemText.Split('-')[0]);
                        goaliePeriodItem.ShotsAgainst = NhlBaseClass.ConvertStringToInt(goaliePeriodItemText.Split('-')[1]);
                    }

                    goalieItem.GoaliePeriodSummary.Add(goaliePeriodItem);

                }

                model.GoalieSummary.Add(goalieItem);
            }

            #endregion

            HtmlNodeCollection officialsAndStarsTableNodes = mainTableNode.SelectSingleNode(@".//td[text()[contains(.,'OFFICIALS')]]/..").NextSibling.NextSibling.SelectNodes(@"./td/table");

            #region Officials

            Nhl_Games_Rtss_Summary_Officials_Item officialsItem;

            HtmlNodeCollection officialsTableRows = officialsAndStarsTableNodes[0].SelectNodes(String.Format(@".{0}/tr", TBODY));
            HtmlNodeCollection refereesRows = officialsTableRows[1].SelectNodes(@"./td")[0].SelectNodes(String.Format(@"./table{0}/tr", TBODY));
            HtmlNodeCollection linesmenRows = officialsTableRows[1].SelectNodes(@"./td")[1].SelectNodes(String.Format(@"./table{0}/tr", TBODY));

            for (int i = 0; i < (refereesRows == null ? 0 : refereesRows.Count); i++)
            {
                officialsItem = NhlGamesRtssSummary.ParseOfficialsItem(refereesRows[i].InnerText);
                officialsItem.VisitorTeam = model.Visitor;
                officialsItem.HomeTeam = model.Home;
                officialsItem.Date = model.Date;
                officialsItem.OfficialType = Designation.Referee;
                model.OfficialsSummary.Add(officialsItem);

            }

            for (int i = 0; i < (linesmenRows == null ? 0 : linesmenRows.Count); i++)
            {
                officialsItem = NhlGamesRtssSummary.ParseOfficialsItem(linesmenRows[i].InnerText);
                officialsItem.VisitorTeam = model.Visitor;
                officialsItem.HomeTeam = model.Home;
                officialsItem.Date = model.Date;
                officialsItem.OfficialType = Designation.Linesman;
                model.OfficialsSummary.Add(officialsItem);
            }

            if (officialsTableRows.Count >= 4)
            {
                HtmlNodeCollection standbyRefereesRows = officialsTableRows[3].SelectNodes(@"./td")[0].SelectNodes(String.Format(@"./table{0}/tr", TBODY));
                HtmlNodeCollection standbyLinesmenRows = officialsTableRows[3].SelectNodes(@"./td")[1].SelectNodes(String.Format(@"./table{0}/tr", TBODY));

                if (null != standbyRefereesRows)
                {
                    for (int i = 0; i < standbyRefereesRows.Count; i++)
                    {
                        officialsItem = NhlGamesRtssSummary.ParseOfficialsItem(standbyRefereesRows[i].InnerText);
                        officialsItem.VisitorTeam = model.Visitor;
                        officialsItem.HomeTeam = model.Home;
                        officialsItem.Date = model.Date;
                        officialsItem.OfficialType = Designation.StandbyReferee;
                        model.OfficialsSummary.Add(officialsItem);
                    }
                }

                if (null != standbyLinesmenRows)
                {
                    for (int i = 0; i < standbyLinesmenRows.Count; i++)
                    {
                        officialsItem = NhlGamesRtssSummary.ParseOfficialsItem(standbyLinesmenRows[i].InnerText);
                        officialsItem.VisitorTeam = model.Visitor;
                        officialsItem.HomeTeam = model.Home;
                        officialsItem.Date = model.Date;
                        officialsItem.OfficialType = Designation.StandbyLinesman;
                        model.OfficialsSummary.Add(officialsItem);
                    }
                }
            }

            #endregion

            #region Stars

            HtmlNodeCollection starsRows = officialsAndStarsTableNodes[1].SelectNodes(@".//table//tr");

            if (null != starsRows)
            {
                for (int i = 0; i < (starsRows == null ? 0 : starsRows.Count); i++)
                {
                    Nhl_Games_Rtss_Summary_Stars_Item starsItem = new Nhl_Games_Rtss_Summary_Stars_Item();
                    starsItem.Date = model.Date;

                    HtmlNodeCollection starsRowFields = starsRows[i].SelectNodes(@"./td");
                    starsItem.StarNumber = i + 1;
                    starsItem.Team = starsRowFields[1].InnerText;
                    starsItem.Position = starsRowFields[2].InnerText;

                    int playerNumber = 0;
                    string playerName = String.Empty;

                    NhlBaseClass.ParseNameText(starsRowFields[3].InnerText, out playerNumber, out playerName);

                    starsItem.PlayerNumber = playerNumber;
                    starsItem.Name = playerName;

                    model.StarsSummary.Add(starsItem);
                }
            }

            #endregion

            //if (model.StarsSummary.ElementAt(0).Date < new DateTime(1990, 1, 1))
            //{
            //    Console.WriteLine("");
            //}

            return model;
        }

        /// <summary>
        /// The nameText should be the officials number and name. Eg. #40 Steve Kozari
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static Nhl_Games_Rtss_Summary_Officials_Item ParseOfficialsItem(string nameText)
        {
            Nhl_Games_Rtss_Summary_Officials_Item officialsItem = new Nhl_Games_Rtss_Summary_Officials_Item();
            officialsItem.OfficialType = Designation.None;

            int officialsNumber = 0;
            string officialsName = String.Empty;

            NhlBaseClass.ParseNameText(nameText, out officialsNumber, out officialsName);
            officialsItem.Number = officialsNumber;
            officialsItem.Name = officialsName;

            return officialsItem;
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
                existingModels = (from m in db.Nhl_Games_Rtss_Summary_DbSet
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
