using System;
using System.Collections.Generic;
using System.Data.Entity;
//using System.Data.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using SportsData;
using SportsData.Models;
using SportsData.Social;

using SportsData.Controllers;

namespace SportsData.Areas.Social.Controllers
{
    public class TwitterController : SocialBaseController
    {
        public ActionResult Index(bool update = false)
        {
            ViewBag.GetLatest = update; // specifies if we should show the update button

            List<TwitterSnapshot> latestSnapshots = new List<TwitterSnapshot>();

            using (SportsDataContext db = new SportsDataContext())
            {
                TwitterSnapshot latestSnapshot = (from d in db.TwitterSnapshot_DbSet
                                  orderby d.DateOfSnapshot descending
                                  select d).FirstOrDefault();

                if (null != latestSnapshot)
                {
                    DateTime latestDate = latestSnapshot.DateOfSnapshot;

                    IEnumerable<TwitterSnapshot> results = from s in db.TwitterSnapshot_DbSet.Include(x => x.TwitterAccount)
                                                           where DbFunctions.TruncateTime(s.DateOfSnapshot) == DbFunctions.TruncateTime(latestDate)
                                                                  //&& !s.TwitterAccountId.Equals("NhlToSeattle", StringComparison.InvariantCultureIgnoreCase)
                                                                  //&& !s.TwitterAccountId.Equals("Nhl", StringComparison.InvariantCultureIgnoreCase)
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
            return this.IndexPost<TwitterData>("Twitter");
        }

        public ActionResult Update()
        {
            return this.UpdateGet("Twitter");
        }

    }
}
