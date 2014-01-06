﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HtmlAgilityPack;
using SportsData;
using SportsData.Models;

namespace SportsData.Nhl
{
    public class NhlHtmlReportRoster : NhlHtmlReportBase
    {

        public static void UpdateSeason([Optional] int year, [Optional] DateTime fromDate, [Optional] bool forceOverwrite)
        {
            // Initialize the rtss reports that we are going to read and parse
            List<NhlRtssReportModel> models = NhlHtmlReportBase.GetRtssReports(year, fromDate);
            List<NhlHtmlReportRosterModel> existingModels = null;
            if (forceOverwrite == false)
            {
                // Only query for existing if we are not going to force overwrite all
                existingModels = NhlHtmlReportRoster.GetHtmlRtssReports(year, fromDate);
            }

            // For each report, get the html blob from blob storage and parse the blob to a report
            List<NhlHtmlReportRosterModel> results = new List<NhlHtmlReportRosterModel>();
            foreach (NhlRtssReportModel model in models)
            {
                if (forceOverwrite == false && existingModels.Exists(m => m.NhlRtssReportModelId == model.Id))
                {
                    // In this case, only get data if it is not already populated
                    continue;
                }

                string htmlBlob = HtmlBlob.RetrieveBlob(HtmlBlobType.NhlRoster, model.Id.ToString(), new Uri(model.RosterLink), true);
                NhlHtmlReportRosterModel report = NhlHtmlReportRoster.ParseHtmlBlob(model.Id, htmlBlob);

                if (null != report)
                {
                    results.Add(report);
                }
            }

            // Save the reports to the db
            using (SportsDataContext db = new SportsDataContext())
            {
                db.NhlHtmlReportRosters.AddOrUpdate<NhlHtmlReportRosterModel>(
                    m => m.NhlRtssReportModelId,
                    results.ToArray());
                db.SaveChanges();
            }
        }

        public static NhlHtmlReportRosterModel ParseHtmlBlob(int rtssReportId, string html)
        {
            if (String.IsNullOrWhiteSpace(html) || html.Equals("404")) { return null; }

            NhlHtmlReportRosterModel model = new NhlHtmlReportRosterModel();
            model.NhlRtssReportModelId = rtssReportId;

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNode documentNode = htmlDocument.DocumentNode;

            // Get the teams
            HtmlNodeCollection teamNodes = documentNode.SelectNodes(@".//tr/td[contains(@class,'teamHeading + border')]");
            string team1 = teamNodes[0].InnerText;
            string team2 = teamNodes[1].InnerText;

            // Get the tables that contain a header with '#'
            HtmlNodeCollection rosterTables = documentNode.SelectNodes(@".//table[tbody/tr/td[text()='#'] or tr/td[text()='#']]");

            // Pull out the rows for rosters and scratches. Assume the order of:
            // 1. visitor roster
            // 2. home roster
            // 3. visitor scratches
            // 4. home scratches
            // Also, ignore the first rows because they are header fields.
            HtmlNodeCollection team1RosterNodes = rosterTables[0].SelectNodes(@".//tr[position() > 1]");
            HtmlNodeCollection team2RosterNodes = rosterTables[1].SelectNodes(@".//tr[position() > 1]");
            HtmlNodeCollection team1ScratchesNodes = rosterTables[2].SelectNodes(@".//tr[position() > 1]");
            HtmlNodeCollection team2ScratchesNodes = rosterTables[3].SelectNodes(@".//tr[position() > 1]");
            Assert.IsTrue(team1RosterNodes.Count > 10, "team1Roster count");
            Assert.IsTrue(team2RosterNodes.Count > 10, "team2Roster count");
            Assert.IsTrue(team1ScratchesNodes.Count >= 0, "team1Scratches count");
            Assert.IsTrue(team2ScratchesNodes.Count >= 0, "team2Scratches count");

            // Parse the players out of the lists
            List<NhlHtmlReportRosterParticipantModel> team1Roster = NhlHtmlReportRoster.ParsePlayers(team1RosterNodes);
            List<NhlHtmlReportRosterParticipantModel> team2Roster = NhlHtmlReportRoster.ParsePlayers(team2RosterNodes);
            List<NhlHtmlReportRosterParticipantModel> team1Scratches = NhlHtmlReportRoster.ParsePlayers(team1ScratchesNodes);
            List<NhlHtmlReportRosterParticipantModel> team2Scratches = NhlHtmlReportRoster.ParsePlayers(team2ScratchesNodes);

            // Find the head coaches
            HtmlNodeCollection coachNodes = documentNode.SelectNodes(@".//tr[@id='HeadCoaches']/td/table/tbody/tr | .//tr[@id='HeadCoaches']/td/table/tr");
            NhlHtmlReportRosterParticipantModel coach1 = NhlHtmlReportRoster.ParseCoach(coachNodes[0]);
            NhlHtmlReportRosterParticipantModel coach2 = NhlHtmlReportRoster.ParseCoach(coachNodes[1]);

            // Find the officials
            HtmlNode officialsTableNode = documentNode.SelectSingleNode(@".//table[tbody/tr/td[contains(text(),'Referee')] or tr/td[contains(text(),'Referee')]]");
            HtmlNodeCollection officialsSubTableNodes = officialsTableNode.SelectNodes(@".//table");
            HtmlNodeCollection refereesNodes = officialsSubTableNodes[0].SelectNodes(@".//tr");
            HtmlNodeCollection linesmenNodes = officialsSubTableNodes[1].SelectNodes(@".//tr");

            NhlHtmlReportRosterParticipantModel referee1 = NhlHtmlReportRoster.ParseReferee(refereesNodes[0]);
            NhlHtmlReportRosterParticipantModel referee2 = NhlHtmlReportRoster.ParseReferee(refereesNodes[1]);
            NhlHtmlReportRosterParticipantModel linesman1 = NhlHtmlReportRoster.ParseReferee(linesmenNodes[0]);
            NhlHtmlReportRosterParticipantModel linesman2 = NhlHtmlReportRoster.ParseReferee(linesmenNodes[1]);

            // Check for standby officials
            HtmlNodeCollection standbyOfficialsNodes1 = officialsSubTableNodes[2].SelectNodes(@".//tr");
            HtmlNodeCollection standbyOfficialsNodes2 = officialsSubTableNodes[3].SelectNodes(@".//tr");
            if (null != standbyOfficialsNodes1 || null != standbyOfficialsNodes2)
            {
                Console.WriteLine("Encountered potential standby officials in RTSS report {0}", rtssReportId);
            }


            // Fill out the model
            model.VisitorHeadCoach = new List<NhlHtmlReportRosterParticipantModel> {coach1};
            model.HomeHeadCoach = new List<NhlHtmlReportRosterParticipantModel> { coach2 };
            model.VisitorRoster = team1Roster;
            model.HomeRoster = team2Roster;
            model.VisitorScratches = team1Scratches;
            model.HomeScratches = team2Scratches;
            model.Linesman = new List<NhlHtmlReportRosterParticipantModel> { linesman1, linesman2 };
            model.Referees = new List<NhlHtmlReportRosterParticipantModel> { referee1, referee2 };

            return model;
        }

        #region Players

        private static List<NhlHtmlReportRosterParticipantModel> ParsePlayers(HtmlNodeCollection rows)
        {
            List<NhlHtmlReportRosterParticipantModel> players = new List<NhlHtmlReportRosterParticipantModel>();

            foreach (HtmlNode row in rows)
            {
                NhlHtmlReportRosterParticipantModel player = NhlHtmlReportRoster.ParsePlayer(row);
                if (null != player)
                {
                    players.Add(player);
                }
            }

            return players;
        }

        private static NhlHtmlReportRosterParticipantModel ParsePlayer(HtmlNode row)
        {
            NhlHtmlReportRosterParticipantModel player = new NhlHtmlReportRosterParticipantModel();

            // Assume it is a Player
            player.ParticipantType = ParticipantType.Player;

            HtmlNodeCollection columnNodes = row.SelectNodes(@"./td");
            player.Number = Convert.ToInt32(columnNodes[0].InnerText);
            player.Position = columnNodes[1].InnerText;

            // Parse out the name, captaincy, starting lineup
            string nameText = columnNodes[2].InnerText;
            if (columnNodes[2].Attributes["class"].Value.IndexOf("bold", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                player.StartingLineup = true;
            }

            if (nameText.IndexOf("(C)", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                player.Designation = Designation.Captain;
                player.Name = nameText.Replace("(C)", String.Empty).Trim();
            }
            else if (nameText.IndexOf("(A)", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                player.Designation = Designation.AssistantCaptain;
                player.Name = nameText.Replace("(A)", String.Empty).Trim();
            }
            else
            {
                player.Name = nameText.Trim();
            }

            return player;
        }

        #endregion

        #region Coaches

        private static NhlHtmlReportRosterParticipantModel ParseCoach(HtmlNode row)
        {
            HtmlNode columnNode = row.SelectSingleNode(@"./td");

            NhlHtmlReportRosterParticipantModel coach = new NhlHtmlReportRosterParticipantModel();
            coach.ParticipantType = ParticipantType.Coach;
            coach.Name = columnNode.InnerText.Trim();
            return coach;
        }

        #endregion

        #region Officials

        private static NhlHtmlReportRosterParticipantModel ParseReferee(HtmlNode row)
        {
            NhlHtmlReportRosterParticipantModel official = NhlHtmlReportRoster.ParseOfficial(row);
            official.Designation = Designation.Referee;
            return official;
        }

        private static NhlHtmlReportRosterParticipantModel ParseLinesman(HtmlNode row)
        {
            NhlHtmlReportRosterParticipantModel official = NhlHtmlReportRoster.ParseOfficial(row);
            official.Designation = Designation.Linesman;
            return official;
        }

        private static NhlHtmlReportRosterParticipantModel ParseOfficial(HtmlNode row)
        {
            HtmlNode columnNode = row.SelectSingleNode(@"./td");

            NhlHtmlReportRosterParticipantModel official = new NhlHtmlReportRosterParticipantModel();
            official.ParticipantType = ParticipantType.Official;

            string nameText = columnNode.InnerText.Trim();
            Regex regex = new Regex(@"(?<number>\d+)(?<name>.*)");

            string number = regex.Match(nameText).Groups["number"].Value;
            if (String.IsNullOrWhiteSpace(number))
            {
                official.Number = 0;
                official.Name = nameText.Trim();
            }
            else
            {
                official.Number = Convert.ToInt32(number);
                official.Name = regex.Match(nameText).Groups["name"].Value.Trim();
            }

           

            return official;
        }

        #endregion

        /// <summary>
        /// Get the NhlHtmlReportRosterModel for the specified year
        /// </summary>
        private static List<NhlHtmlReportRosterModel> GetHtmlRtssReports([Optional] int year, [Optional] DateTime fromDate)
        {
            year = NhlGameStatsBaseModel.SetDefaultYear(year);

            List<NhlHtmlReportRosterModel> existingModels = new List<NhlHtmlReportRosterModel>();
            using (SportsDataContext db = new SportsDataContext())
            {
                existingModels = (from m in db.NhlHtmlReportRosters
                                  where
                                      m.NhlRtssReportModel.Year == year &&
                                      m.NhlRtssReportModel.Date >= fromDate
                                  select m).ToList();
            }

            return existingModels;
        }
    }
}

