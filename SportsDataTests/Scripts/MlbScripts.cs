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
        //[Ignore]
        public void Script_Mlb_GetGameSummary()
        {
            int year = 2014;

            MlbAttendanceData.UpdateSeason(MlbSeasonType.Spring, year);
            MlbAttendanceData.UpdateSeason(MlbSeasonType.Regular, year);
            MlbAttendanceData.UpdateSeason(MlbSeasonType.PostSeason, year);
        }
    }
}
