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
    public class NhlScripts : SportsDataScriptsBaseClass
    {
        [TestMethod]
        public void Script_NhlGetGameSummary()
        {
            NhlGameSummary.UpdateSeason(2014);
        }

        [TestMethod]
        public void Script_NhlGetAllGameSummary()
        {
            // Create a list of years to collect data for
            List<int> years = new List<int>();
            for (int i = 2001; i >= 2000; i--)
            {
                years.Add(i);
            }

            foreach (int year in years)
            {
                NhlGameSummary.UpdateSeason(year);
            }
        }

        [TestMethod]
        public void Script_NhlGetRtssReport()
        {
            NhlRtssReport.UpdateSeason(2014);
        }

        [TestMethod]
        public void Script_NhlGetAllRtssReport()
        {
            // Create a list of years to collect data for
            List<int> years = new List<int>();
            for (int i = 2014; i >= 1998; i--)
            {
                years.Add(i);
            }

            foreach (int year in years)
            {
                NhlRtssReport.UpdateSeason(year);
            }
        }
    }
}
