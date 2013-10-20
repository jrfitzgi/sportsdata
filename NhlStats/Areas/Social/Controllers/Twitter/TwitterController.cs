using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using SportsData;
using SportsData.Twitter;

using SportsData.Controllers;

namespace SportsData.Areas.Social.Controllers
{
    public class TwitterController : SportsDataController
    {
        public ActionResult Index(bool update = false)
        {
            ViewBag.GetLatest = update; // specifies if we should show the update button

            List<TwitterAccountSnapshot> latestSnapshots = new List<TwitterAccountSnapshot>();

            using (SportsDataContext db = new SportsDataContext())
            {
                TwitterAccountSnapshot latestSnapshot = (from d in db.TwitterSnapshots
                                  orderby d.DateOfSnapshot descending
                                  select d).FirstOrDefault();

                if (null != latestSnapshot)
                {
                    DateTime latestDate = latestSnapshot.DateOfSnapshot;

                    IEnumerable<TwitterAccountSnapshot> results = from s in db.TwitterSnapshots.Include(x => x.TwitterAccount)
                                                                  where EntityFunctions.TruncateTime(s.DateOfSnapshot) == EntityFunctions.TruncateTime(latestDate)
                                                                  && !s.TwitterAccountId.Equals("NhlToSeattle", StringComparison.InvariantCultureIgnoreCase)
                                                                  && !s.TwitterAccountId.Equals("Nhl", StringComparison.InvariantCultureIgnoreCase)
                                                                  orderby s.TwitterAccount.FriendlyName
                                                                  select s;

                    latestSnapshots = results.ToList();
                }
            }

            return Result(latestSnapshots);
        }

        [HttpPost]
        public ActionResult Index()
        {
            bool performUpdate = (this.Request["Update"] == "Get Latest");

            if (performUpdate)
            {
                List<TwitterAccount> twitterAccounts = new List<TwitterAccount>();
                using (SportsDataContext db = new SportsDataContext())
                {
                    twitterAccounts = db.TwitterAccountsToFollow.ToList();
                }

                TwitterData.UpdateSnapshotsInDb(twitterAccounts);
            }

            return RedirectToAction("Index", "Twitter", new { update = performUpdate });
        }

        public ActionResult Update()
        {
            return RedirectToAction("Index", "Twitter", new { update = true });
        }

    }
}
