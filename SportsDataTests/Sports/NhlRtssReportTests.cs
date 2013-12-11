using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HtmlAgilityPack;
using SportsData.Nhl;
using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class NhlRtssReportTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            //Database.SetInitializer<SportsDataContext>(new SportsDataContextDropCreateDatabaseAlways());
            Database.SetInitializer<SportsDataContext>(new SportsDataContextDropCreateDatabaseIfNotExists());
        }

        [TestMethod]
        public void NhlRtssGetSeason()
        {
            int year = 2013;

            NhlRtssReport nhlRtssReport = new NhlRtssReport();
            List<NhlGameStatsBaseModel> results = nhlRtssReport.GetSeason(year);

            Assert.AreEqual(806, results.Count);
        }

        [TestMethod]
        public void NhlRtssGetSeasonAndUpdateDb()
        {
            int year = 2013;

            NhlRtssReport.UpdateSeason(year);

            // Need to add verification here

        }

        // Used for debugging. Need to change some protected methods to public for this to run.
        //[TestMethod]
        //public void NhlParseRtssReportRow()
        //{
        //    NhlSeasonType nhlSeasonType = NhlSeasonType.RegularSeason;
        //    int year = 2014;

        //    NhlRtssReport nhlRtssReport = new NhlRtssReport();
        //    HtmlNode tableNode = nhlRtssReport.ParseHtmlTableFromPage(year, nhlSeasonType, 1);

        //    List<HtmlNode> rowNodes = NhlRtssReport.ParseRowsFromTable(tableNode);
        //    Assert.AreEqual(30, rowNodes.Count);

        //    NhlRtssReportModel model = nhlRtssReport.MapHtmlRowToModel(rowNodes[0], nhlSeasonType) as NhlRtssReportModel;
        //}
    }
}
