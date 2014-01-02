using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
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
        public static void UpdateSeason(int year)
        {
            // Get the RtssReports for the specified year
            List<NhlRtssReportModel> models = NhlHtmlReportBase.GetRtssReports(year);

            // For each report, get the html blob from blob storage and parse the blob to a report
            List<NhlHtmlReportRosterModel> results = new List<NhlHtmlReportRosterModel>();
            foreach (NhlRtssReportModel model in models)
            {
                string htmlBlob = HtmlBlob.RetrieveBlob(HtmlBlobType.NhlRoster, model.Id.ToString(), new Uri(model.RosterLink));
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
            if (String.IsNullOrWhiteSpace(html)) { return null; }

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

            string x = "X";



            return model;
        }

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

    }
}

