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
    public class NhlGameStatsRtssReportTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void NhlGameStatsRtssReport_GetFullSeason()
        {
            int year = 2013;
            List<NhlRtssReportModel> results = NhlGameStatsRtssReport.GetFullSeason(year);
            Assert.AreEqual(806, results.Count);
        }
    }
}
