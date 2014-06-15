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
    public class NhlPlayerStatsRtssTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void NhlPlayerStatsRtss_GetFullSeason()
        {
            int year = 2013;
            List<NhlPlayerStatsRtssModel> results = NhlPlayerStatsRtss.GetFullSeason(year, false);
            Assert.AreEqual(1188, results.Count);
        }
    }
}
