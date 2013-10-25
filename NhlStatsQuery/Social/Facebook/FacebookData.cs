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
        public static List<FacebookSnapshot> UpdateSnapshotsInDb(List<FacebookAccount> accountsToQuery)
        {
            // Get latest results
            List<FacebookSnapshot> snapshotsToAdd = (new FacebookQuery()).GetSnapshots<FacebookSnapshot,FacebookAccount>(accountsToQuery) ;

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

                foreach (FacebookSnapshot snapshotToAdd in snapshotsToAdd)
                {
                    db.FacebookSnapshots.Add(snapshotToAdd);
                }

                db.SaveChanges();
            }

            return snapshotsToAdd;
        }

    }
}

