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
    public class NhlPlayerStatsBioTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void NhlPlayerStatsBio_GetFullSeason()
        {
            int year = 2013;
            List<NhlPlayerStatsBioSkaterModel> results = NhlPlayerStatsBio.GetFullSeason(year, false);
            Assert.AreEqual(1188, results.Count);
        }
    }
}
