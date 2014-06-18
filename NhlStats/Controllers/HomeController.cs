using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SportsData.Nhl.Query;

namespace SportsData.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("index", "draft", new { area = "nhl" });
        }
        
        public ActionResult OldIndex()
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
