using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace SportsData.Social
{
    public class FacebookData
    {
        public static List<FacebookSnapshot> UpdateAllSnapshotsInDb()
        {
            List<FacebookAccount> accounts = new List<FacebookAccount>();
            using (SportsDataContext db = new SportsDataContext())
            {
                accounts = db.FacebookAccountsToFollow.ToList();
            }

            return FacebookData.UpdateSnapshotsInDb(accounts);
        }

        public static List<FacebookSnapshot> UpdateSnapshotsInDb(List<FacebookAccount> accounts)
        {
            // Get latest results
            List<FacebookSnapshot> snapshots = FacebookQuery.GetFacebookSnapshots(accounts) ;

            // Remove existing results from DB from today and save new ones
            using (SportsDataContext db = new SportsDataContext())
            {
                IEnumerable<FacebookSnapshot> snapshotsToRemove = from s in db.FacebookSnapshots
                                                                        where EntityFunctions.TruncateTime(s.DateOfSnapshot) == EntityFunctions.TruncateTime(DateTime.UtcNow)
                                                                        select s;

                foreach (FacebookSnapshot snapshotToRemove in snapshotsToRemove)
                {
                    db.FacebookSnapshots.Remove(snapshotToRemove);
                }

                //db.SaveChanges();

                foreach (FacebookSnapshot snapshotToAdd in snapshots)
                {
                    db.FacebookSnapshots.Add(snapshotToAdd);
                }

                db.SaveChanges();
            }

            return snapshots;
        }

    }
}

