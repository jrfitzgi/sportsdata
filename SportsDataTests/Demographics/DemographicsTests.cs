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
            List<int> zipCodes = new List<int> {98103};

            List<DemographicsModel> results = DemographicsQuery.GetDemographics(zipCodes);

            Assert.AreEqual(1, results.Count);
        }
    }
}
