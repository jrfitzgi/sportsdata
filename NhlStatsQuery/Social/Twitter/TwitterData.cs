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
    public class TwitterData
    {
        public static List<TwitterSnapshot> UpdateSnapshotsInDb(List<TwitterAccount> twitterAccountSnapshots)
        {
            // Get latest results
            List<TwitterSnapshot> snapshotsToAdd = TwitterQuery.GetTwitterSnapshots(twitterAccountSnapshots);

            // Remove existing results from DB from today and save new ones
            using (SportsDataContext db = new SportsDataContext())
            {
                IEnumerable<TwitterSnapshot> snapshotsToRemove = from s in db.TwitterSnapshots
                                                            where EntityFunctions.TruncateTime(s.DateOfSnapshot) == EntityFunctions.TruncateTime(DateTime.UtcNow)
                                                            select s;

                foreach (TwitterSnapshot snapshotToRemove in snapshotsToRemove)
                {
                    db.TwitterSnapshots.Remove(snapshotToRemove);
                }

                //db.SaveChanges();

                foreach (TwitterSnapshot snapshotToAdd in snapshotsToAdd)
                {
                    db.TwitterSnapshots.Add(snapshotToAdd);
                }

                db.SaveChanges();
            }

            return snapshotsToAdd;
        }

    }
}
