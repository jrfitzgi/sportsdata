using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NhlStatsQuery;

namespace NhlStats.Areas.Attendance.Controllers
{
    public class NhlController : Controller
    {
        public ActionResult Index(bool update=false)
        {
            if (update == true)
            {
                ViewBag.GetLatest = true;
            }

            string season = (string)Request.Form["SeasonList"];
            ViewBag.Season = season;

            if (String.IsNullOrEmpty(season))
            {
                ViewBag.Table = String.Empty;
            }
            else
            {
                bool performUpdate = false;

                if (this.Request["Update"] == "Update")
                {
                    performUpdate = true;
                }

                ViewBag.Table = NhlStatsQuery.NhlStatsQuery.GetStatsFromDb(season, performUpdate);
                ViewBag.PerformUpdate = false;
            }

            return View();
        }

        public ActionResult Update()
        {
            return RedirectToAction("Index", "Nhl", new { update = true });
        }

    }
}
