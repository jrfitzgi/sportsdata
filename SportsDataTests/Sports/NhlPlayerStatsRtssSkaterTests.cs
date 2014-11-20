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
    public class NhlPlayerStatsRtssSkaterTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void NhlPlayerStatsRtssSkater_GetFullSeason()
        {
            int year = 2013;
            List<Nhl_Players_Rtss_Skater> results = NhlPlayerStatsRtssSkater.GetFullSeason(year, false);
            Assert.AreEqual(1188, results.Count);
        }
    }
}
