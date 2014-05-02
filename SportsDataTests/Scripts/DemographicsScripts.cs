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
    public class DemographicsScripts : SportsDataScriptsBaseClass
    {
        [TestMethod]
        public void DemographicsScript()
        {
            string zipCsv = "98103, 98052, 98107";

            List<int> zipCodes = zipCsv.Replace(" ", String.Empty).Split(',').ToList().ConvertAll<int>(x => Convert.ToInt32(x));

            List<DemographicsModel> results = DemographicsQuery.GetDemographics(zipCodes);

            Assert.AreEqual(zipCodes.Count, results.Count);

            DemographicsData.UpdateDatabase(results);
        }
    }
}
