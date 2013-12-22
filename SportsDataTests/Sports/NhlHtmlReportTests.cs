using System;
using System.Collections.Generic;
using System.Data.Entity;
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

            NhlHtmlReportSummaryModel model = NhlHtmlReportSummary.ParseHtmlBlob(html);
        }

        [TestMethod]
        public void ParseHtmlReportSummaryEnglishInProgressRegSeason()
        {
            string path = @"C:\Users\jordanf\Google Drive\Coding\Sportsdata\TestData\EnglishInProgressRoster.htm";
            string html = File.ReadAllText(path);

            NhlHtmlReportSummaryModel model = NhlHtmlReportSummary.ParseHtmlBlob(html);
        }

        [TestMethod]
        public void ParseMultipleHtmlGameSummaries()
        {
            List<NhlRtssReportModel> models;
            using (SportsDataContext db = new SportsDataContext())
            {
                models = (from m in db.NhlRtssReports
                          where
                             m.NhlSeasonType == NhlSeasonType.RegularSeason &&
                             m.Year == 2014
                          select m).ToList();
            }

            foreach (NhlRtssReportModel model in models)
            {
                string htmlBlob = HtmlBlob.RetrieveBlob(HtmlBlobType.NhlRoster, model.Id.ToString(), new Uri(model.RosterLink));
                NhlHtmlReportSummaryModel report = NhlHtmlReportSummary.ParseHtmlBlob(htmlBlob);
            }

        }

        [TestMethod]
        public void ParseHtmlReportRosterFrenchRegSeason()
        {
            string path = @"C:\Users\jordanf\Google Drive\Coding\Sportsdata\TestData\FrenchRegSeasonRoster_formatted.htm";
            string html = File.ReadAllText(path);

            NhlHtmlReportRosterModel model = NhlHtmlReportRoster.ParseHtmlBlob(html);
        }

    }
}
