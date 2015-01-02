using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsData.Areas.Nhl.Controllers
{
    public class FightingController : Controller
    {
        //
        // GET: /Nhl/Fighting/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Nhl/Fighting/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Nhl/Fighting/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Nhl/Fighting/Create

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
        // GET: /Nhl/Fighting/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Nhl/Fighting/Edit/5

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
        // GET: /Nhl/Fighting/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Nhl/Fighting/Delete/5

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
