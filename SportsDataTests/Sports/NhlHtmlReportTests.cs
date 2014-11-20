using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HtmlAgilityPack;
using SportsData;
using SportsData.Nhl;
using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class NhlHtmlReportTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void NhlHtmlReport_HtmlBlobTest()
        {
            Uri url = new Uri("http://www.nhl.com/scores/htmlreports/20132014/RO020507.HTM ");
            //Uri url = new Uri("http://www.nhl.com/scores/htmlreports/20132014/RO020502.HTM");

            string id = (new Guid()).ToString();
            string result = HtmlBlob.GetHtmlPage(url);

            Assert.IsFalse(HtmlBlob.BlobExists(HtmlBlobType.NhlRoster, id, url));

            HtmlBlob.SaveBlob(HtmlBlobType.NhlRoster, id, url, result);
            string downloadedBlob = HtmlBlob.RetrieveBlob(HtmlBlobType.NhlRoster, id, url);

            Assert.IsTrue(HtmlBlob.BlobExists(HtmlBlobType.NhlRoster, id, url));

        }

        [TestMethod]
        public void NhlHtmlReport_ParseSummaryFrenchRegSeason()
        {
            string path = @"C:\Users\jordanf\Google Drive\Coding\Sportsdata\TestData\FrenchRegSeasonRoster_formatted.htm";
            string html = File.ReadAllText(path);

            Nhl_Games_Rtss_Summary model = NhlHtmlReportSummary.ParseHtmlBlob(-1, html);
        }

        [TestMethod]
        public void NhlHtmlReport_PareSummaryEnglishInProgressRegSeason()
        {
            string path = @"C:\Users\jordanf\Google Drive\Coding\Sportsdata\TestData\EnglishInProgressRoster.htm";
            string html = File.ReadAllText(path);

            Nhl_Games_Rtss_Summary model = NhlHtmlReportSummary.ParseHtmlBlob(-1, html);
        }

        [TestMethod]
        public void NhlHtmlReport_ParseSummaryCapsThrashers2007()
        {
            string path = @"C:\Users\jordanf\Google Drive\Coding\Sportsdata\TestData\CapsThrashers_10.05.2007.htm";
            string html = File.ReadAllText(path);

            Nhl_Games_Rtss_Summary model = NhlHtmlReportSummary.ParseHtmlBlob(-1, html);
        }
        
        [TestMethod]
        public void NhlHtmlReport_ParseMultipleSummaries()
        {
            NhlHtmlReportSummary.UpdateSeason(2014);
        }

        [TestMethod]
        public void NhlHtmlReport_ParseRosterFrenchRegSeason()
        {
            string path = @"C:\Users\jordanf\Google Drive\Coding\Sportsdata\TestData\FrenchRegSeasonRoster_formatted.htm";
            string html = File.ReadAllText(path);

            Nhl_Games_Rtss_Roster model = NhlHtmlReportRoster.ParseHtmlBlob(-1, html);
        }

        [TestMethod]
        public void NhlHtmlReport_ParseFrenchRegSeasonAndPersist()
        {
            string path = @"C:\Users\jordanf\Google Drive\Coding\Sportsdata\TestData\FrenchRegSeasonRoster_formatted.htm";
            string html = File.ReadAllText(path);

            Nhl_Games_Rtss_Roster model = NhlHtmlReportRoster.ParseHtmlBlob(-1, html);

            using (SportsDataContext db = new SportsDataContext())
            {
                db.NhlHtmlReportRosters.AddOrUpdate(
                    m => m.Id,
                    model);
                db.SaveChanges();
                
                model = db.NhlHtmlReportRosters.FirstOrDefault(m => m.Id == model.Id);
                model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "visitor player 1" });
                model.VisitorRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "visitor player 2" });
                model.VisitorScratches.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "visitor scratch 1" });
                model.VisitorScratches.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "visitor scratch 2" });
                model.VisitorHeadCoach.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "visitor head coach 1" });

                model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "home player 1" });
                model.HomeRoster.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "home player 2" });
                model.HomeScratches.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "home scratch 1" });
                model.HomeScratches.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "home scratch 2" });
                model.HomeHeadCoach.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "home head coach 1" });

                model.Referees.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "referee 1" });
                model.Referees.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "referee 2" });
                model.Linesman.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "linesman 1" });
                model.Linesman.Add(new Nhl_Games_Rtss_RosterParticipantItem { Name = "linesman 2" });

                db.NhlHtmlReportRosters.AddOrUpdate(
                    m => m.Id,
                    model);

                db.SaveChanges();
            }

            List<Nhl_Games_Rtss_Roster> models;
            using (SportsDataContext db = new SportsDataContext())
            {
                models = (from m in db.NhlHtmlReportRosters
                              .Include(x => x.VisitorRoster)
                              .Include(x => x.VisitorScratches)
                              .Include(x => x.VisitorHeadCoach)
                              .Include(x => x.HomeRoster)
                              .Include(x => x.HomeScratches)
                              .Include(x => x.HomeHeadCoach)
                              .Include(x => x.Referees)
                              .Include(x => x.Linesman)
                          select m).ToList();
                
            }
        }

    }
}
