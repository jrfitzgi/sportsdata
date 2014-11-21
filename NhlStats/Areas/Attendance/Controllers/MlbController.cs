using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using SportsData;
using SportsData.Mlb;
using SportsData.Models;

using SportsData.Controllers;

namespace SportsData.Areas.Attendance.Controllers
{
    public class MlbController : SportsDataController
    {
        public ActionResult Index(int seasonYear = 0, bool update = false)
        {
            ViewBag.SeasonYear = seasonYear;
            ViewBag.GetLatest = update; // specifies if we should show the update button

            List<MlbGameSummaryModel> games = new List<MlbGameSummaryModel>();
            if (seasonYear >= 2002) // check if it is a valid season and if so get the results
            {
                using (SportsDataContext db = new SportsDataContext())
                {
                    IEnumerable<MlbGameSummaryModel> results = from g in db.MlbGameSummaryModel_DbSet
                                                          where g.Year == seasonYear
                                                          orderby g.Date
                                                          select g;
                    games = results.ToList();
                }
            }

            return Result(games);
        }

        [HttpPost]
        public ActionResult Index()
        {
            int year = Convert.ToInt32(this.Request["SeasonList"]);
            bool performUpdate = (this.Request["Update"] == "Get Latest");

            if (performUpdate)
            {
                //MlbAttendanceData.UpdateSeasonForTeam(MlbSeasonType.Spring, MlbTeamShortName.TOR, year);
                MlbAttendanceData.UpdateSeason(MlbSeasonType.Spring, year);
                MlbAttendanceData.UpdateSeason(MlbSeasonType.Regular, year);
                MlbAttendanceData.UpdateSeason(MlbSeasonType.PostSeason, year);
            }

            return RedirectToAction("Index", "Mlb", new { seasonYear = year, update = performUpdate });
        }

        public ActionResult Update()
        {
            return RedirectToAction("Index", "Mlb", new { seasonYear = 0, update = true });
        }

    }
}
