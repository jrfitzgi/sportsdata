using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsData.Mlb;

namespace SportsDataTests
{
    [TestClass]
    public class MlbTests
    {
        [TestMethod]
        public void GetSpringSeasonForTeamTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.Spring;
            MlbTeamShortName teamName = MlbTeamShortName.NYY;
            int seasonYear = 2012;

            List<MlbGameSummary> result = MlbAttendanceQuery.GetSeasonForTeam(seasonType, teamName, seasonYear);
            Assert.AreEqual(12, result.Count, "Verify that there are 12 home games returned");

            Assert.AreEqual(new DateTime(2012, 3, 2), result[0].Date, "Verify that the date of the first game is 3/2/2012");
            Assert.AreEqual(MlbTeamShortName.NYY.ToString(), result[1].Home, "Verify that the home team is NYY");
            Assert.AreEqual(MlbTeamShortName.TB.ToString(), result[2].Visitor, "Verify that the away team is TB");
        }

        [TestMethod]
        public void GetSpringSeasonForAllTeamsTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.Spring;
            int seasonYear = 2011;

            List<MlbGameSummary> result = MlbAttendanceQuery.GetSeason(seasonType, seasonYear);
            
            Assert.AreEqual(491, result.Count, "Verify that there are 491 home games returned");
        }

        [TestMethod]
        public void GetRegularSeasonForTeamTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.Regular;
            int seasonYear = 2009;

            List<MlbGameSummary> result = MlbAttendanceQuery.GetSeason(seasonType, seasonYear);
            Assert.AreEqual(81, result, "Verify that there are 81 home games returned");

            Assert.AreEqual(new DateTime(2010, 10, 3), result[80].Date, "Verify that the date of the last game is 10/3/2010");
            Assert.AreEqual(MlbTeamShortName.SEA.ToString(), result[80].Home, "Verify that the home team is SEA");
            Assert.AreEqual(MlbTeamShortName.OAK.ToString(), result[80].Visitor, "Verify that the away team is OAK");
            Assert.AreEqual(3, result[80].HomeScore, "Verify that the home score is 3");
            Assert.AreEqual(4, result[80].VisitorScore, "Verify that visitor score is 4");
            Assert.AreEqual(61, result[80].WinsToDate, "Verify that the wins to date is 61");
            Assert.AreEqual(101, result[80].LossesToDate, "Verify that losses to date is 101");
            Assert.AreEqual("Braden", result[80].WPitcher, "Verify that the W Pitcher is Braden");
            Assert.AreEqual("Varvaro", result[80].LPitcher, "Verify that the L Pitcher is Varvaro");
            Assert.AreEqual("Breslow", result[80].SavePitcher, "Verify that the Save Pitcher is Breslow");
            Assert.AreEqual(23278, result[80].Attendance, "Verify that attendance is 23278");

        }

        [TestMethod]
        public void GetRegularSeasonForAllTeamsTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.Regular;
            int seasonYear = 2010;

            List<MlbGameSummary> result = MlbAttendanceQuery.GetSeason(seasonType, seasonYear);

            Assert.AreEqual(2460, result.Where(x => x.Postponed == false).Count(), "Verify that there are 2460 home games returned");
        }
    }
}
