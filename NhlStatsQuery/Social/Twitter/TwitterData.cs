using System;
using System.Collections.Generic;
using System.Data.Entity;
//using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;
using SportsData.Models;

namespace SportsData.Social
{
    public class TwitterData
    {
        public static List<TwitterSnapshot> UpdateAllSnapshotsInDb()
        {
            List<TwitterAccount> accounts = new List<TwitterAccount>();
            using (SportsDataContext db = new SportsDataContext())
            {
                accounts = db.TwitterAccount_DbSet.ToList();
            }

            return TwitterData.UpdateSnapshotsInDb(accounts);
        }
        
        public static List<TwitterSnapshot> UpdateSnapshotsInDb(List<TwitterAccount> accounts)
        {
            // Get latest results
            List<TwitterSnapshot> snapshots = TwitterQuery.GetTwitterSnapshots(accounts);

            // Remove existing results from DB from today and save new ones
            using (SportsDataContext db = new SportsDataContext())
            {
                IEnumerable<TwitterSnapshot> snapshotsToRemove = from s in db.TwitterSnapshot_DbSet
                                                                 where DbFunctions.TruncateTime(s.DateOfSnapshot) == DbFunctions.TruncateTime(DateTime.UtcNow)
                                                            select s;

                foreach (TwitterSnapshot snapshotToRemove in snapshotsToRemove)
                {
                    db.TwitterSnapshot_DbSet.Remove(snapshotToRemove);
                }

                //db.SaveChanges();

                foreach (TwitterSnapshot snapshotToAdd in snapshots)
                {
                    db.TwitterSnapshot_DbSet.Add(snapshotToAdd);
                }

                db.SaveChanges();
            }

            return snapshots;
        }

    }
}
