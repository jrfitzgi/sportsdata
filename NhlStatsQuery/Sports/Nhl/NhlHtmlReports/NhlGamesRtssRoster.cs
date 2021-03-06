﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
//using System.Data.Objects;
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
    public class NhlGamesRtssRoster : NhlHtmlReportBase
    {

        public static void UpdateSeason([Optional] int year, [Optional] DateTime fromDate, [Optional] bool forceOverwrite)
        {
            // Initialize the rtss reports that we are going to read and parse
            List<Nhl_Games_Rtss> models = NhlHtmlReportBase.GetRtssReports(year, fromDate);
            List<Nhl_Games_Rtss_Roster> existingModels = null;
            if (forceOverwrite == false)
            {
                // Only query for existing if we are not going to force overwrite all
                existingModels = NhlGamesRtssRoster.GetHtmlRosterReports(year, fromDate);
            }

            // For each report, get the html blob from blob storage and parse the blob to a report
            List<Nhl_Games_Rtss_Roster> results = new List<Nhl_Games_Rtss_Roster>();
            foreach (Nhl_Games_Rtss model in models)
            {
                if (forceOverwrite == false && existingModels.Exists(m => m.NhlRtssReportModelId == model.Id))
                {
                    // In this case, only get data if it is not already populated
                    continue;
                }

                Nhl_Games_Rtss_Roster report = null;
                if (!model.GameLink.Equals("#"))
                {
                    string htmlBlob = HtmlBlob.RetrieveBlob(HtmlBlobType.NhlRoster, model.Id.ToString(), new Uri(model.RosterLink), true);
                    report = NhlGamesRtssRoster.ParseHtmlBlob(model.Id, htmlBlob);
                }

                if (null != report)
                {
                    results.Add(report);
                }
            }

            // Save the reports to the db 100 records at a time
            using (SportsDataContext db = new SportsDataContext())
            {
                int counter = 0;
                int totalCounter = 0;
                int batchSize = 10;
                foreach (Nhl_Games_Rtss_Roster model in results)
                {
                    Console.WriteLine("Start saving {0} to {1}", results.Count, db.Database.Connection.ConnectionString);

                    db.Configuration.AutoDetectChangesEnabled = false;
                    db.Configuration.ValidateOnSaveEnabled = false;

                    counter++;
                    totalCounter++;

                    if (model.Id != 0)
                    {
                        db.Nhl_Games_Rtss_Roster_DbSet.Attach(model);
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

        public static Nhl_Games_Rtss_Roster ParseHtmlBlob(int rtssReportId, string html)
        {
            if (String.IsNullOrWhiteSpace(html) || html.Equals("404")) { return null; }

            Nhl_Games_Rtss_Roster model = new Nhl_Games_Rtss_Roster();
            model.NhlRtssReportModelId = rtssReportId;

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNode documentNode = htmlDocument.DocumentNode;

            // Special Case
            // The html for this game doesn't follow the same format as the other games 
            if (null != documentNode.SelectSingleNode(@"./html/head/link[@href='RO010002_files/editdata.mso']"))
            {
                return NhlGamesRtssRoster.BruinsRangersSpecialCase(rtssReportId);
            }

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
            HtmlNodeCollection team1RosterNodes = null;
            HtmlNodeCollection team2RosterNodes = null;
            if (null != rosterTables && rosterTables.Count >= 2)
            {
                team1RosterNodes = rosterTables[0].SelectNodes(@".//tr[position() > 1]");
                team2RosterNodes = rosterTables[1].SelectNodes(@".//tr[position() > 1]");
                Assert.IsTrue(team1RosterNodes.Count > 10, "team1Roster count");
                Assert.IsTrue(team2RosterNodes.Count > 10, "team2Roster count");
            }

            HtmlNodeCollection scratchesNodes = documentNode.SelectNodes(@".//tr[@id='Scratches']/td");
            HtmlNodeCollection team1ScratchesNodes = null;
            HtmlNodeCollection team2ScratchesNodes = null;
            if (null != scratchesNodes)
            {
                team1ScratchesNodes = scratchesNodes[0].SelectNodes(@"./table/tr[position() > 1] | ./table/tbody/tr[position() > 1]");
                team2ScratchesNodes = scratchesNodes[1].SelectNodes(@"./table/tr[position() > 1] | ./table/tbody/tr[position() > 1]");
            }

            // Parse the players out of the lists
            List<Nhl_Games_Rtss_RosterParticipantItem> team1Roster = NhlGamesRtssRoster.ParsePlayers(team1RosterNodes);
            List<Nhl_Games_Rtss_RosterParticipantItem> team2Roster = NhlGamesRtssRoster.ParsePlayers(team2RosterNodes);
            List<Nhl_Games_Rtss_RosterParticipantItem> team1Scratches = NhlGamesRtssRoster.ParsePlayers(team1ScratchesNodes);
            List<Nhl_Games_Rtss_RosterParticipantItem> team2Scratches = NhlGamesRtssRoster.ParsePlayers(team2ScratchesNodes);

            // Find the head coaches
            HtmlNodeCollection coachNodes = documentNode.SelectNodes(@".//tr[@id='HeadCoaches']/td/table/tbody/tr | .//tr[@id='HeadCoaches']/td/table/tr");
            Nhl_Games_Rtss_RosterParticipantItem coach1 = NhlGamesRtssRoster.ParseCoach(coachNodes[0]);
            Nhl_Games_Rtss_RosterParticipantItem coach2 = NhlGamesRtssRoster.ParseCoach(coachNodes[1]);

            // Find the officials
            HtmlNode officialsTableNode = documentNode.SelectSingleNode(@".//table[tbody/tr/td[contains(text(),'Referee')] or tr/td[contains(text(),'Referee')]]");
            HtmlNodeCollection officialsSubTableNodes = officialsTableNode.SelectNodes(@".//table");
            HtmlNodeCollection refereesNodes = officialsSubTableNodes[0].SelectNodes(@".//tr");
            HtmlNodeCollection linesmenNodes = officialsSubTableNodes[1].SelectNodes(@".//tr");

            Nhl_Games_Rtss_RosterParticipantItem referee1 = null;
            if (refereesNodes != null && refereesNodes.Count >= 1) { referee1 = NhlGamesRtssRoster.ParseReferee(refereesNodes[0]); }

            Nhl_Games_Rtss_RosterParticipantItem referee2 = null;
            if (refereesNodes != null && refereesNodes.Count >= 2) { referee2 = NhlGamesRtssRoster.ParseReferee(refereesNodes[1]); }

            Nhl_Games_Rtss_RosterParticipantItem linesman1 = null;
            if (linesmenNodes != null && linesmenNodes.Count >= 1) { linesman1 = NhlGamesRtssRoster.ParseLinesman(linesmenNodes[0]); }

            Nhl_Games_Rtss_RosterParticipantItem linesman2 = null;
            if (linesmenNodes != null && linesmenNodes.Count >= 2) { linesman2 = NhlGamesRtssRoster.ParseLinesman(linesmenNodes[1]); }

            // Check for standby officials
            HtmlNodeCollection standbyOfficialsNodes1 = officialsSubTableNodes[2].SelectNodes(@".//tr");
            HtmlNodeCollection standbyOfficialsNodes2 = officialsSubTableNodes[3].SelectNodes(@".//tr");
            if (null != standbyOfficialsNodes1 || null != standbyOfficialsNodes2)
            {
                Console.WriteLine("Encountered potential standby officials in RTSS report {0}", rtssReportId);
            }


            // Fill out the model
            model.VisitorHeadCoach = new List<Nhl_Games_Rtss_RosterParticipantItem> { coach1 };
            model.HomeHeadCoach = new List<Nhl_Games_Rtss_RosterParticipantItem> { coach2 };
            model.VisitorRoster = team1Roster;
            model.HomeRoster = team2Roster;
            model.VisitorScratches = team1Scratches;
            model.HomeScratches = team2Scratches;
            model.Linesman = new List<Nhl_Games_Rtss_RosterParticipantItem> { linesman1, linesman2 };
            model.Referees = new List<Nhl_Games_Rtss_RosterParticipantItem> { referee1, referee2 };

            return model;
        }

        #region Players

        private static List<Nhl_Games_Rtss_RosterParticipantItem> ParsePlayers(HtmlNodeCollection rows)
        {
            if (null == rows) { return null; }

            List<Nhl_Games_Rtss_RosterParticipantItem> players = new List<Nhl_Games_Rtss_RosterParticipantItem>();

            foreach (HtmlNode row in rows)
            {
                Nhl_Games_Rtss_RosterParticipantItem player = NhlGamesRtssRoster.ParsePlayer(row);
                if (null != player)
                {
                    players.Add(player);
                }
            }

            return players;
        }

        private static Nhl_Games_Rtss_RosterParticipantItem ParsePlayer(HtmlNode row)
        {
            Nhl_Games_Rtss_RosterParticipantItem player = new Nhl_Games_Rtss_RosterParticipantItem();

            // Assume it is a Player
            player.ParticipantType = ParticipantType.Player;

            HtmlNodeCollection columnNodes = row.SelectNodes(@"./td");

            string numberText = columnNodes[0].InnerText;
            if (!String.IsNullOrWhiteSpace(numberText))
            {
                player.Number = Convert.ToInt32(numberText);
            }
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

        private static Nhl_Games_Rtss_RosterParticipantItem ParseCoach(HtmlNode row)
        {
            HtmlNode columnNode = row.SelectSingleNode(@"./td");

            Nhl_Games_Rtss_RosterParticipantItem coach = new Nhl_Games_Rtss_RosterParticipantItem();
            coach.ParticipantType = ParticipantType.Coach;
            coach.Designation = Designation.HeadCoach;
            coach.Name = columnNode.InnerText.Trim();
            return coach;
        }

        #endregion

        #region Officials

        private static Nhl_Games_Rtss_RosterParticipantItem ParseReferee(HtmlNode row)
        {
            Nhl_Games_Rtss_RosterParticipantItem official = NhlGamesRtssRoster.ParseOfficial(row);
            official.Designation = Designation.Referee;
            return official;
        }

        private static Nhl_Games_Rtss_RosterParticipantItem ParseLinesman(HtmlNode row)
        {
            Nhl_Games_Rtss_RosterParticipantItem official = NhlGamesRtssRoster.ParseOfficial(row);
            official.Designation = Designation.Linesman;
            return official;
        }

        private static Nhl_Games_Rtss_RosterParticipantItem ParseOfficial(HtmlNode row)
        {
            HtmlNode columnNode = row.SelectSingleNode(@"./td");
            string nameText = columnNode.InnerText.Trim();

            Nhl_Games_Rtss_RosterParticipantItem official = new Nhl_Games_Rtss_RosterParticipantItem();
            official.ParticipantType = ParticipantType.Official;

            int officialsNumber = 0;
            string officialsName = String.Empty;
            NhlBaseClass.ParseNameText(nameText, out officialsNumber, out officialsName);

            official.Number = officialsNumber;
            official.Name = officialsName;

            return official;
        }

        #endregion

        /// <summary>
        /// Get the NhlHtmlReportRosterModel for the specified year
        /// </summary>
        private static List<Nhl_Games_Rtss_Roster> GetHtmlRosterReports([Optional] int year, [Optional] DateTime fromDate)
        {
            year = NhlModelHelper.SetDefaultYear(year);

            List<Nhl_Games_Rtss_Roster> existingModels = new List<Nhl_Games_Rtss_Roster>();
            using (SportsDataContext db = new SportsDataContext())
            {
                existingModels = (from m in db.Nhl_Games_Rtss_Roster_DbSet
                                  where
                                      m.NhlRtssReportModel.Year == year &&
                                      m.NhlRtssReportModel.Date >= fromDate
                                  select m).ToList();
            }

            return existingModels;
        }

        private static Nhl_Games_Rtss_Roster BruinsRangersSpecialCase(int rtssReportId)
        {
            Nhl_Games_Rtss_Roster model = new Nhl_Games_Rtss_Roster();
            model.NhlRtssReportModelId = rtssReportId;

            model.VisitorRoster = new List<Nhl_Games_Rtss_RosterParticipantItem>();
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 12, Position = "R", Name = "CHUCK KOBASEW", Designation = Designation.AssistantCaptain });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 16, Position = "L", Name = "MARCO STRUM", Designation = Designation.AssistantCaptain });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 17, Position = "L", Name = "MILAN LUCIC" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 26, Position = "R", Name = "BLAKE WHEELER" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 33, Position = "D", Name = "ZDENO CHARA", Designation = Designation.Captain });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 42, Position = "C", Name = "TRENT WHITFIELD" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 48, Position = "D", Name = "MATT HUNWICK" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 52, Position = "C", Name = "ZACH HAMILL" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 54, Position = "D", Name = "ADAM MCQUAID" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 55, Position = "D", Name = "JOHNNY BOYCHUK" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 60, Position = "C", Name = "VLADIMIR SOBOTKA" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 61, Position = "R", Name = "BYRON BITZ" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 62, Position = "D", Name = "JEFFREY PENNER" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 63, Position = "C", Name = "BRAD MARCHAND" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 65, Position = "D", Name = "ANDREW BODNARCHUK" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 68, Position = "R", Name = "MIKKO LEHTONEN" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 72, Position = "C", Name = "JAMIE ARNIEL" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 74, Position = "C", Name = "MAX SAUVE" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 32, Position = "G", Name = "DANY SABOURIN" });
            model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 40, Position = "G", Name = "TUUKKA RASK" });
            model.VisitorRoster.ToList().ForEach(m => m.ParticipantType = ParticipantType.Player);

            model.HomeRoster = new List<Nhl_Games_Rtss_RosterParticipantItem>();
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 5, Position = "D", Name = "DAN GIRARDI" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 12, Position = "R", Name = "ALES KOTALIK" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 18, Position = "D", Name = "MARC STAAL" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 21, Position = "L", Name = "CHRISTOPHER HIGGINS" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 22, Position = "C", Name = "BRIAN BOYLE" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 23, Position = "C", Name = "CHRIS DRURY", Designation = Designation.Captain });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 24, Position = "R", Name = "RYAN CALLAHAN" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 25, Position = "D", Name = "ALEXEI SEMENOV" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 32, Position = "D", Name = "MICHAEL SAUER" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 36, Position = "L", Name = "DANE BYERS" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 38, Position = "R", Name = "PIERRE PARENTEAU" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 42, Position = "C", Name = "ARTEM ANISIMOV" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 43, Position = "D", Name = "MICHAEL DEL ZOTTO" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 59, Position = "C", Name = "EVGENY GRACHEV" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 81, Position = "R", Name = "ENVER LISIN" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 85, Position = "L", Name = "MATT MACCARONE" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 87, Position = "L", Name = "DONALD BRASHEAR" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 97, Position = "D", Name = "MATT GILROY" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 31, Position = "G", Name = "MATT ZABA" });
            model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 40, Position = "G", Name = "STEPHEN VALIQUETTE" });
            model.HomeRoster.ToList().ForEach(m => m.ParticipantType = ParticipantType.Player);

            // Since this is a preseason game, don't bother filling in the scratches. The data is available here http://www.nhl.com/scores/htmlreports/20092010/RO010002.HTM
            model.VisitorScratches = new List<Nhl_Games_Rtss_RosterParticipantItem>();
            model.HomeScratches = new List<Nhl_Games_Rtss_RosterParticipantItem>();

            model.VisitorHeadCoach = new List<Nhl_Games_Rtss_RosterParticipantItem>();
            model.VisitorHeadCoach.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "CLAUDE JULIEN", Designation = Designation.HeadCoach, ParticipantType = ParticipantType.Coach });

            model.HomeHeadCoach = new List<Nhl_Games_Rtss_RosterParticipantItem>();
            model.HomeHeadCoach.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "JOHN TORTORELLA", Designation = Designation.HeadCoach, ParticipantType = ParticipantType.Coach });

            model.Referees = new List<Nhl_Games_Rtss_RosterParticipantItem>();
            model.Referees.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 8, Name = "Dave Jackson", Designation = Designation.Referee, ParticipantType = ParticipantType.Official });
            model.Referees.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 41, Name = "Chris Ciamaga", Designation = Designation.Referee, ParticipantType = ParticipantType.Official });

            model.Linesman = new List<Nhl_Games_Rtss_RosterParticipantItem>();
            model.Referees.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 84, Name = "Tony Sericolo", Designation = Designation.Linesman, ParticipantType = ParticipantType.Official });
            model.Referees.Add(new Nhl_Games_Rtss_RosterParticipantItem { Number = 77, Name = "Tim Nowak", Designation = Designation.Linesman, ParticipantType = ParticipantType.Official });

            return model;
        }
    }
}

