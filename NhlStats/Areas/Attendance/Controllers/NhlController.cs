using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SportsData.Models;
using SportsData.Nhl;
using SportsData.Nhl.Query;

using SportsData.Controllers;

namespace SportsData.Areas.Attendance.Controllers
{
    public class NhlController : SportsDataController
    {
        public ActionResult Index(int seasonYear = 0, bool update = false)
        {
            ViewBag.GetLatest = update;
            ViewBag.SeasonYear = seasonYear;

            List<Nhl_Games_Summary> games = new List<Nhl_Games_Summary>();
            if (seasonYear >= 1998) // check if it is a valid season and if so get the results
            {
                using (SportsDataContext db = new SportsDataContext())
                {
                    IEnumerable<Nhl_Games_Summary> results = from g in db.NhlGameStatsSummaries
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
            string year = this.Request["SeasonList"];
            bool performUpdate = (this.Request["Update"] == "Get Latest");

            if (performUpdate)
            {
                NhlGamesSummary.GetFullSeason(year:Convert.ToInt32(year), saveToDb:true);
            }

            return RedirectToAction("Index", "Nhl", new { seasonYear = Convert.ToInt32(year), update = performUpdate });
        }

        public ActionResult Update()
        {
            return RedirectToAction("Index", "Nhl", new { update = true });
        }

    }
}
