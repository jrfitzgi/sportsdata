using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsData.Demographics;
using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class DemographicsTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void DemographicsTest()
        {
            List<int> zipCodes = new List<int> {98103, 98052, 98007};

            List<DemographicsModel> results = DemographicsQuery.GetDemographics(zipCodes);

            Assert.AreEqual(3, results.Count);

            DemographicsModel fremont = results.Single(x => x.Zip == 98103);
            Assert.AreEqual(49044, fremont.MedianIncome);
            Assert.AreEqual(73, fremont.MedianIncomeRank);

            Assert.AreEqual(43.9, fremont.OwnerOccupiedHomesPercent);
            Assert.AreEqual(5, fremont.OwnerOccupiedHomesRank);

            Assert.AreEqual(36.5, fremont.MarriedPercent);
            Assert.AreEqual(2, fremont.MarriedRank);

            Assert.AreEqual("Seattle", fremont.City);
            Assert.AreEqual("WA", fremont.State);

        }
    }
}
