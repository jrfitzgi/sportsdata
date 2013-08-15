using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NhlStatsQuery;

namespace NhlStats.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(bool GetLatest=false)
        {
            //if (this.HttpContext.Session["GetLatest"] == "true")
            if (GetLatest == true)
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

        public ActionResult About()
        {
            ViewBag.Message = String.Empty;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = String.Empty;

            return View();
        }
    }
}
