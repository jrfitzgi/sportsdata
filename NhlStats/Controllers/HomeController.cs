using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SportsData.Nhl.Query;

namespace NhlStats.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = String.Empty;

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
