using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SportsData.Models;

namespace ClarkWebRole.Areas.Hockey.Controllers
{
    public class PenaltiesController : Controller
    {
        // GET: Hockey/Penalties
        public ActionResult Index()
        {
            return View();
        }

        // GET: Hockey/Penalties/Details/5
        public ActionResult Details(int year)
        {
            List<Nhl_Games_Rtss_Summary> model;
            using (SportsDataContext db = new SportsDataContext())
            {
                model = (from game in db.Nhl_Games_Rtss_Summary_DbSet
                        where game.Date.Year == year
                        select game).ToList();
            }
            return View(model);
        }

        // GET: Hockey/Penalties/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hockey/Penalties/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Hockey/Penalties/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Hockey/Penalties/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Hockey/Penalties/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Hockey/Penalties/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
