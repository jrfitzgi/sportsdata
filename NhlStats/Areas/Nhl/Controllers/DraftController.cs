using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SportsData.Models;

namespace SportsData.Areas.Nhl.Controllers
{
    public class DraftController : Controller
    {
        //
        // GET: /Nhl/Draft/

        public ActionResult Index()
        {
            List<NhlPlayerStatsBioSkaterModel> model;
            using (SportsDataContext db = new SportsDataContext())
            {
                model = (from m in db.NhlPlayerStatsBioSkaters
                        where m.Year == 2014 && m.NhlSeasonType == NhlSeasonType.RegularSeason
                        select m).ToList();
            }

            return View(model);
        }

        //
        // GET: /Nhl/Draft/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Nhl/Draft/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Nhl/Draft/Create

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

        //
        // GET: /Nhl/Draft/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Nhl/Draft/Edit/5

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

        //
        // GET: /Nhl/Draft/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Nhl/Draft/Delete/5

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
