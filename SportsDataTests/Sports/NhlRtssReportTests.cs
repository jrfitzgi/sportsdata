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
    public class NhlRtssReportTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void NhlRtssGetSeason()
        {
            int year = 2013;

            NhlRtssReport nhlRtssReport = new NhlRtssReport();
            List<NhlGameStatsBaseModel> results = nhlRtssReport.GetSeason(year);

            Assert.AreEqual(806, results.Count);
        }
    }
}
