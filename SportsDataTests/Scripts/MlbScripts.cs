using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HtmlAgilityPack;
using SportsData.Mlb;
using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class MlbScripts : SportsDataScriptsBaseClass
    {

        [TestMethod]
        public void Script_MlbGetGameSummary()
        {
            int year = 2004;

            MlbAttendanceData.UpdateSeason(MlbSeasonType.Spring, year);
            MlbAttendanceData.UpdateSeason(MlbSeasonType.Regular, year);
            MlbAttendanceData.UpdateSeason(MlbSeasonType.PostSeason, year);
        }

        //[TestMethod]
        //public void Script_NhlGetAllGameSummary()
        //{
        //    // Create a list of years to collect data for
        //    List<int> years = new List<int>();
        //    for (int i = 2001; i >= 2000; i--)
        //    {
        //        years.Add(i);
        //    }

        //    foreach (int year in years)
        //    {
        //        NhlGameSummary.UpdateSeason(year);
        //    }
        //}
    }
}
