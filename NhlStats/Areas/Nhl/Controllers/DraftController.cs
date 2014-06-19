using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SportsData.Areas.Nhl.Models;
using SportsData.Models;

namespace SportsData.Areas.Nhl.Controllers
{
    public class DraftController : Controller
    {
        //
        // GET: /Nhl/Draft/

        public ActionResult Index()
        {
            int draftYear = 2008;

            List<NhlPlayerStatsBioSkaterViewModel> viewModel;
            using (SportsDataContext db = new SportsDataContext())
            {
                IEnumerable<NhlPlayerStatsBioSkaterViewModel> queryResult =
                        from player in db.NhlPlayerStatsBioSkaters
                        where player.NhlSeasonType == NhlSeasonType.RegularSeason
                        group player by new { player.Name, player.DraftYear } into playerGroup
                        select new NhlPlayerStatsBioSkaterViewModel
                        {
                            Name = playerGroup.Key.Name,
                            DraftYear = playerGroup.Key.DraftYear,
                            SeasonsPlayed = playerGroup.Count(),
                            AvgGamesPerSeason = Math.Round(playerGroup.Average(p => p.GamesPlayed), 1),
                            TotalGamesPlayed = playerGroup.Sum(p => p.GamesPlayed)
                        };

                viewModel = queryResult.ToList();
            }

            return View(viewModel);
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
