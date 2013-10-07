using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SportsData.Nhl;
using SportsData.Nhl.Query;

namespace SportsData.Areas.Attendance.Controllers
{
    public class NhlController : SportsDataController
    {
        public ActionResult Index(int seasonYear = 0, bool update = false)
        {
            ViewBag.GetLatest = update;
            ViewBag.SeasonYear = seasonYear;


            List<NhlGameSummary> games = new List<NhlGameSummary>();
            if (seasonYear >= 1998) // check if it is a valid season and if so get the results
            {
                using (SportsDataContext db = new SportsDataContext())
                {
                    IEnumerable<NhlGameSummary> results = from g in db.NhlGameSummaries
                                                          where g.Season == seasonYear
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
            string year = this.Request["SeasonList"];
            bool performUpdate = (this.Request["Update"] == "Get Latest");

            if (performUpdate)
            {
                NhlStatsQuery.GetAndStoreStats(year);
            }

            return RedirectToAction("Index", "Nhl", new { seasonYear = Convert.ToInt32(year), update = performUpdate });
        }

        public ActionResult Update()
        {
            return RedirectToAction("Index", "Nhl", new { update = true });
        }

    }
}
