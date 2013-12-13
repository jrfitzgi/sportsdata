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
    public class NhlGameSummaryTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void NhlGameSummaryUpdateLatestOnly()
        {
            int year = 2012;
            NhlGameSummary.UpdateSeasonWithLatestOnly(year);

        }

        [TestMethod]
        public void NhlGameSummaryGetPartialSeason()
        {
            int year = 2012;

            NhlRtssReport nhlRtssReport = new NhlRtssReport();
            List<NhlGameStatsBaseModel> results = nhlRtssReport.GetSeason(year, new DateTime(2012, 4, 6));
            Assert.AreEqual(16 + 86, results.Count);
        }

        [TestMethod]
        public void NhlGameSummaryGetFullSeason()
        {
            int year = 2013;

            NhlRtssReport nhlRtssReport = new NhlRtssReport();
            List<NhlGameStatsBaseModel> results = nhlRtssReport.GetSeason(year);
            Assert.AreEqual(806, results.Count);
        }
    }
}
