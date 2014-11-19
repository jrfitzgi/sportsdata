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
    public class NhlGameStatsSummaryTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void NhlGameStatsSummary_GetNewResultsOnly()
        {
            int year = 2012;
            NhlGameStatsSummary.GetNewResultsOnly(year);

        }

        [TestMethod]
        public void NhlGameStatsSummary_GetPartialSeason()
        {
            int year = 2012;
            List<Nhl_Games_Summary> results = NhlGameStatsSummary.GetFullSeason(year, new DateTime(2012, 4, 6));
            Assert.AreEqual(16 + 86, results.Count);
        }

        [TestMethod]
        public void NhlGameStatsSummary_GetFullSeason()
        {
            int year = 2013;
            List<Nhl_Games_Rtss> results = NhlGameStatsRtssReport.GetFullSeason(year);
            Assert.AreEqual(806, results.Count);
        }
    }
}
