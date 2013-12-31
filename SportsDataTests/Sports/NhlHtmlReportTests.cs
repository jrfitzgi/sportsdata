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
        public void HtmlBlobTest()
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
        public void ParseHtmlReportSummaryFrenchRegSeason()
        {
            string path = @"C:\Users\jordanf\Google Drive\Coding\Sportsdata\TestData\FrenchRegSeasonRoster_formatted.htm";
            string html = File.ReadAllText(path);

            NhlHtmlReportSummaryModel model = NhlHtmlReportSummary.ParseHtmlBlob(-1, html);
        }

        [TestMethod]
        public void ParseHtmlReportSummaryEnglishInProgressRegSeason()
        {
            string path = @"C:\Users\jordanf\Google Drive\Coding\Sportsdata\TestData\EnglishInProgressRoster.htm";
            string html = File.ReadAllText(path);

            NhlHtmlReportSummaryModel model = NhlHtmlReportSummary.ParseHtmlBlob(-1, html);
        }

        [TestMethod]
        public void ParseMultipleHtmlGameSummaries()
        {
            NhlHtmlReportSummary.UpdateSeason(2014);
        }

        [TestMethod]
        public void ParseHtmlReportRosterFrenchRegSeason()
        {
            string path = @"C:\Users\jordanf\Google Drive\Coding\Sportsdata\TestData\FrenchRegSeasonRoster_formatted.htm";
            string html = File.ReadAllText(path);

            NhlHtmlReportRosterModel model = NhlHtmlReportRoster.ParseHtmlBlob(-1, html);
        }

        [TestMethod]
        public void ParseHtmlReportRosterFrenchRegSeason_AndPersist()
        {
            string path = @"C:\Users\jordanf\Google Drive\Coding\Sportsdata\TestData\FrenchRegSeasonRoster_formatted.htm";
            string html = File.ReadAllText(path);

            NhlHtmlReportRosterModel model = NhlHtmlReportRoster.ParseHtmlBlob(-1, html);

            using (SportsDataContext db = new SportsDataContext())
            {
                db.NhlHtmlReportRosters.AddOrUpdate(
                    m => m.Id,
                    model);
                db.SaveChanges();
                
                model = db.NhlHtmlReportRosters.FirstOrDefault(m => m.Id == model.Id);
                model.VisitorRoster.Add(new NhlHtmlReportRosterEntryModel("visitor player 1", model.Id));
                model.VisitorRoster.Add(new NhlHtmlReportRosterEntryModel("visitor player 2", model.Id));
                model.VisitorScratches.Add(new NhlHtmlReportRosterEntryModel("visitor scratch 1", model.Id));
                model.VisitorScratches.Add(new NhlHtmlReportRosterEntryModel("visitor scratch 2", model.Id));
                model.VisitorHeadCoach.Add(new NhlHtmlReportRosterEntryModel("visitor head coach 1", model.Id));
 
                model.HomeRoster.Add(new NhlHtmlReportRosterEntryModel("home player 1", model.Id));
                model.HomeRoster.Add(new NhlHtmlReportRosterEntryModel("home player 2", model.Id));
                model.HomeScratches.Add(new NhlHtmlReportRosterEntryModel("home scratch 1", model.Id));
                model.HomeScratches.Add(new NhlHtmlReportRosterEntryModel("home scratch 2", model.Id));
                model.HomeHeadCoach.Add(new NhlHtmlReportRosterEntryModel("home head coach 1", model.Id));

                model.Referees.Add(new NhlHtmlReportRosterEntryModel("referee 1", model.Id));
                model.Referees.Add(new NhlHtmlReportRosterEntryModel("referee 2", model.Id));
                model.Linesman.Add(new NhlHtmlReportRosterEntryModel("linesman 1", model.Id));
                model.Linesman.Add(new NhlHtmlReportRosterEntryModel("linesman 2", model.Id));

                db.NhlHtmlReportRosters.AddOrUpdate(
                    m => m.Id,
                    model);

                db.SaveChanges();
            }

            List<NhlHtmlReportRosterModel> models;
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
