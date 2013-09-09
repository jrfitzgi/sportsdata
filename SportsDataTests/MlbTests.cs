using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsData.Mlb;

namespace SportsDataTests
{
    [TestClass]
    public class MlbTests
    {
        [TestMethod]
        public void GetSeasonTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.Spring;
            MlbTeamShortName teamName = MlbTeamShortName.NYY;
            int seasonYear = 2012;

            List<MlbGameSummary> result = MlbAttendanceQuery.GetSeason(seasonType, teamName, seasonYear);

        }
    }
}
