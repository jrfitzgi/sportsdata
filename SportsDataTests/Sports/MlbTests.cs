using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsData.Mlb;
using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class MlbTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void Mlb_GetSpringSeasonForTeamTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.Spring;
            MlbTeamShortName teamName = MlbTeamShortName.NYY;
            int seasonYear = 2012;

            List<MlbGameSummaryModel> result = MlbAttendanceQuery.GetSeasonForTeam(seasonType, teamName, seasonYear);

            Assert.AreEqual(12, result.Count, "Verify that there are 12 home games returned");
            Assert.AreEqual(MlbSeasonType.Spring, result[0].MlbSeasonType, "Verify that the game is a Spring season game");
            Assert.AreEqual(new DateTime(2012, 3, 2), result[0].Date, "Verify that the date of the first game is 3/2/2012");
            Assert.AreEqual(2012, result[0].Year, "Verify that the Season is 2012");
            Assert.AreEqual(MlbTeamShortName.NYY.ToString(), result[1].Home, "Verify that the home team is NYY");
            Assert.AreEqual(MlbTeamShortName.TB.ToString(), result[2].Visitor, "Verify that the away team is TB");
        }

        [TestMethod]
        public void Mlb_UpdateSpringSeasonForTeamTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.Spring;
            MlbTeamShortName teamName = MlbTeamShortName.TOR;
            int seasonYear = 2006;

            List<MlbGameSummaryModel> result = MlbAttendanceData.UpdateSeasonForTeam(seasonType, teamName, seasonYear);

            Assert.AreEqual(15, result.Count, "Verify that there are 15 home games returned");

            using (SportsDataContext db = new SportsDataContext())
            {
                string teamNameString = teamName.ToString(); // http://stackoverflow.com/questions/5899683/linq-to-entities-does-not-recognize-the-method-system-string-tostring-method
                Assert.AreEqual(15, db.MlbGameSummaryModel_DbSet.Where(g => g.MlbSeasonType == seasonType &&
                                                                   g.Home.Equals(teamNameString, StringComparison.InvariantCultureIgnoreCase) &&
                                                                   g.Year == seasonYear).Count(),
                                "Verify that there are 15 games in the db");
            }
        }

        [TestMethod]
        public void Mlb_GetSpringSeasonForAllTeamsTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.Spring;
            int seasonYear = 2011;

            List<MlbGameSummaryModel> result = MlbAttendanceQuery.GetSeason(seasonType, seasonYear);

            Assert.AreEqual(MlbSeasonType.Spring, result[0].MlbSeasonType, "Verify that the game is a Spring season game");
            Assert.AreEqual(491, result.Count, "Verify that there are 491 home games returned");
        }

        [TestMethod]
        public void Mlb_GetRegularSeasonForTeamTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.Regular;
            MlbTeamShortName teamName = MlbTeamShortName.SEA;
            int seasonYear = 2010;

            List<MlbGameSummaryModel> result = MlbAttendanceQuery.GetSeasonForTeam(seasonType, teamName, seasonYear);
            Assert.AreEqual(81, result.Count, "Verify that there are 81 home games returned");

            Assert.AreEqual(MlbSeasonType.Regular, result[0].MlbSeasonType, "Verify that the game is a Regular season game");
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
        public void Mlb_UpdateRegularSeasonForTeamTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.Regular;
            MlbTeamShortName teamName = MlbTeamShortName.SD;
            int seasonYear = 2005;

            List<MlbGameSummaryModel> result = MlbAttendanceData.UpdateSeasonForTeam(seasonType, teamName, seasonYear);

            Assert.AreEqual(81, result.Count, "Verify that there are 81 home games returned");

            using (SportsDataContext db = new SportsDataContext())
            {
                string teamNameString = teamName.ToString(); // http://stackoverflow.com/questions/5899683/linq-to-entities-does-not-recognize-the-method-system-string-tostring-method
                Assert.AreEqual(81, db.MlbGameSummaryModel_DbSet.Where(g => g.MlbSeasonType == seasonType &&
                                                                   g.Home.Equals(teamNameString, StringComparison.InvariantCultureIgnoreCase) &&
                                                                   g.Year == seasonYear).Count(),
                                "Verify that there are 81 games in the db");
            }
        }

        [TestMethod]
        public void Mlb_GetRegularSeasonForAllTeamsTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.Regular;
            int seasonYear = 2010;

            List<MlbGameSummaryModel> result = MlbAttendanceQuery.GetSeason(seasonType, seasonYear);

            Assert.AreEqual(MlbSeasonType.Regular, result[0].MlbSeasonType, "Verify that the game is a Regular season game");
            Assert.AreEqual(2460, result.Where(x => x.Postponed == false).Count(), "Verify that there are 2460 home games returned");
        }

        [TestMethod]
        public void Mlb_GetPostSeasonForTeamTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.PostSeason;
            MlbTeamShortName teamName = MlbTeamShortName.BOS;
            int seasonYear = 2008;

            List<MlbGameSummaryModel> result = MlbAttendanceQuery.GetSeasonForTeam(seasonType, teamName, seasonYear);

            Assert.AreEqual(5, result.Count, "Verify that there are 5 home games returned");
            Assert.AreEqual(2008, result[0].Year, "Verify that the Season is 2008");
            Assert.AreEqual(MlbSeasonType.PostSeason, result[0].MlbSeasonType, "Verify that the game is a Postseason game");
            Assert.AreEqual(new DateTime(2008, 10, 6), result[1].Date, "Verify that the date of the last game is 10/6/2008");
            Assert.AreEqual(MlbTeamShortName.BOS.ToString(), result[1].Home, "Verify that the home team is BOS");
            Assert.AreEqual(MlbTeamShortName.LAA.ToString(), result[1].Visitor, "Verify that the away team is LAA");
            Assert.AreEqual(3, result[1].HomeScore, "Verify that the home score is 3");
            Assert.AreEqual(2, result[1].VisitorScore, "Verify that visitor score is 2");
            Assert.AreEqual(3, result[1].WinsToDate, "Verify that the wins to date is 3");
            Assert.AreEqual(1, result[1].LossesToDate, "Verify that losses to date is 1");
            Assert.AreEqual("Delcarmen", result[1].WPitcher, "Verify that the W Pitcher is Delcarmen");
            Assert.AreEqual("Shields", result[1].LPitcher, "Verify that the L Pitcher is Shields");
            Assert.AreEqual(null, result[1].SavePitcher, "Verify that the Save Pitcher is empty");
            Assert.AreEqual(38785, result[1].Attendance, "Verify that attendance is 38785");
        }

        [TestMethod]
        public void Mlb_GetPostSeasonForTeamWithNoPostSeasonTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.PostSeason;
            MlbTeamShortName teamName = MlbTeamShortName.MIA;
            int seasonYear = 2007;

            List<MlbGameSummaryModel> result = MlbAttendanceQuery.GetSeasonForTeam(seasonType, teamName, seasonYear);

            Assert.AreEqual(MlbSeasonType.PostSeason, result[0].MlbSeasonType, "Verify that the game is a Postseason game");
            Assert.AreEqual(null, result, "Verify that there are 0 games returned");
        }

        [TestMethod]
        public void Mlb_UpdatePostSeasonForTeamWithNoPostSeasonTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.PostSeason;
            MlbTeamShortName teamName = MlbTeamShortName.MIA;
            int seasonYear = 2004;

            List<MlbGameSummaryModel> result = MlbAttendanceData.UpdateSeasonForTeam(seasonType, teamName, seasonYear);

            Assert.AreEqual(null, result, "Verify that there are null home games returned");

            using (SportsDataContext db = new SportsDataContext())
            {
                string teamNameString = teamName.ToString(); // http://stackoverflow.com/questions/5899683/linq-to-entities-does-not-recognize-the-method-system-string-tostring-method
                Assert.AreEqual(0, db.MlbGameSummaryModel_DbSet.Where(g => g.MlbSeasonType == seasonType &&
                                                                   g.Home.Equals(teamNameString, StringComparison.InvariantCultureIgnoreCase) &&
                                                                   g.Year == seasonYear).Count(),
                                "Verify that there are 0 games in the db");
            }
        }

        [TestMethod]
        public void Mlb_GetPostSeasonForAllTeamsTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.PostSeason;
            int seasonYear = 2007;

            List<MlbGameSummaryModel> result = MlbAttendanceQuery.GetSeason(seasonType, seasonYear);

            Assert.AreEqual(MlbSeasonType.PostSeason, result[0].MlbSeasonType, "Verify that the game is a Postseason game");
            Assert.AreEqual(28, result.Count, "Verify that there are 28 games returned");
        }

        [TestMethod]
        public void Mlb_UpdatePostSeasonForAllTeamsTest()
        {
            MlbSeasonType seasonType = MlbSeasonType.PostSeason;
            int seasonYear = 2002;

            List<MlbGameSummaryModel> result = MlbAttendanceData.UpdateSeason(seasonType, seasonYear);

            // For some reason, the final game of the world series is not included
            Assert.AreEqual(33, result.Count, "Verify that there were 33 games");

            using (SportsDataContext db = new SportsDataContext())
            {
                Assert.AreEqual(33, db.MlbGameSummaryModel_DbSet.Where(g => g.MlbSeasonType == seasonType &&
                                                                   g.Year == seasonYear).Count(),
                                "Verify that there are 33 games in the db");
            }
        }
    }
}
