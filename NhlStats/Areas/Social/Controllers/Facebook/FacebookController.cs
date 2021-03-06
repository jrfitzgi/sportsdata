﻿using System;
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
    public class FacebookController : SocialBaseController
    {
        public ActionResult Index(bool update = false)
        {
            ViewBag.GetLatest = update; // specifies if we should show the update button

            List<FacebookSnapshot> latestSnapshots = new List<FacebookSnapshot>();

            using (SportsDataContext db = new SportsDataContext())
            {
                FacebookSnapshot latestSnapshot = (from d in db.FacebookSnapshot_DbSet
                                  orderby d.DateOfSnapshot descending
                                  select d).FirstOrDefault();

                if (null != latestSnapshot)
                {
                    DateTime latestDate = latestSnapshot.DateOfSnapshot;

                    IEnumerable<FacebookSnapshot> results = from s in db.FacebookSnapshot_DbSet.Include(x => x.FacebookAccount)
                                                            where DbFunctions.TruncateTime(s.DateOfSnapshot) == DbFunctions.TruncateTime(latestDate)
                                                                  //&& !s.FacebookAccountId.Equals("NhlToSeattle", StringComparison.InvariantCultureIgnoreCase)
                                                                  //&& !s.FacebookAccountId.Equals("Nhl", StringComparison.InvariantCultureIgnoreCase)
                                                                  orderby s.FacebookAccount.FriendlyName
                                                                  select s;

                    latestSnapshots = results.ToList();
                }
            }

            return Result(latestSnapshots);
        }

        [HttpPost]
        public ActionResult Index()
        {
            return this.IndexPost<FacebookData>("Facebook");
        }

        public ActionResult Update()
        {
            return this.UpdateGet("Facebook");
        }

    }
}
