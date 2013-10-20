using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsData;
using SportsData.Twitter;

namespace TwitterTests
{
    [TestClass]
    public class TwitterTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Database.SetInitializer<SportsDataContext>(new SportsDataContextDropCreateDatabaseAlways());
            //Database.SetInitializer<SportsDataContext>(new SportsDataContextDropCreateDatabaseIfNotExists());
        }

        [TestMethod]
        public void TwitterAccountsToFollowTest()
        {
            using (SportsDataContext db = new SportsDataContext())
            {
                Assert.AreEqual(3, db.TwitterAccountsToFollow.Count(), "There are 3 twitter accounts seeded");
            }
        }

        [TestMethod]
        public void TwitterGetSnapshot()
        {
            TwitterAccount twitterAccount = new TwitterAccount { Id = "sanjosesharks", FriendlyName = "San Jose Sharks" };
            TwitterAccountSnapshot twitterAccountSnapshot = TwitterQuery.GetTwitterSnapshot(twitterAccount);

            Assert.AreEqual(DateTime.UtcNow.Date, twitterAccountSnapshot.DateOfSnapshot.Date, "The snapshot is from today");
            Assert.IsTrue(twitterAccountSnapshot.Tweets > 0, "There are more than 0 tweets");
            Assert.IsTrue(twitterAccountSnapshot.Following > 0, "There are more than 0 following");
            Assert.IsTrue(twitterAccountSnapshot.Followers > 0, "There are more than 0 followers");
            Assert.AreEqual(twitterAccount.Id, twitterAccountSnapshot.TwitterAccountId, "There account name is correct in the snapshot");

            Assert.AreEqual("sanjosesharks", twitterAccountSnapshot.TwitterAccountId, "There account name is sanjosesharks");
        }

        [TestMethod]
        public void TwitterGetSnapshots()
        {
            List<TwitterAccount> twitterAccounts = new List<TwitterAccount>();
            twitterAccounts.Add(new TwitterAccount { Id = "MapleLeafs", FriendlyName = "Toronto Maple Leafs" });
            twitterAccounts.Add(new TwitterAccount { Id = "phoenixcoyotes", FriendlyName = "Phoenix Coyotes" });

            List<TwitterAccountSnapshot> twitterAccountSnapshots = TwitterQuery.GetTwitterSnapshots(twitterAccounts);

            Assert.AreEqual(2, twitterAccountSnapshots.Count, "There are 2 snapshots");

            Assert.AreEqual(DateTime.UtcNow.Date, twitterAccountSnapshots[0].DateOfSnapshot.Date, "The snapshots are from today");
            Assert.AreEqual(twitterAccountSnapshots[0].DateOfSnapshot.Date, twitterAccountSnapshots[1].DateOfSnapshot.Date, "The snapshots are equal");
            Assert.AreEqual("MapleLeafs", twitterAccountSnapshots[0].TwitterAccountId, "The first snapshot is from MapleLeafs");
            Assert.AreEqual("phoenixcoyotes", twitterAccountSnapshots[1].TwitterAccountId, "The first snapshot is from phoenixcoyotes");
        }

        [TestMethod]
        public void TwitterUpdateSnapshotsInDb()
        {
            List<TwitterAccount> twitterAccounts = new List<TwitterAccount>();
            twitterAccounts.Add(new TwitterAccount { Id = "MapleLeafs", FriendlyName = "Toronto Maple Leafs" });
            twitterAccounts.Add(new TwitterAccount { Id = "phoenixcoyotes", FriendlyName = "Phoenix Coyotes" });

            List<TwitterAccountSnapshot> twitterAccountSnapshots = TwitterData.UpdateSnapshotsInDb(twitterAccounts);
            Assert.AreEqual(2, twitterAccountSnapshots.Count, "There are 2 snapshots");

            // Call Update again to make sure dupes are added
            twitterAccountSnapshots = TwitterData.UpdateSnapshotsInDb(twitterAccounts);

            // Make sure we have the right items in the db
            using (SportsDataContext db = new SportsDataContext())
            {
                List<TwitterAccountSnapshot> snapshotsFromToday = (from s in db.TwitterSnapshots.Include(x => x.TwitterAccount)
                                                                  where EntityFunctions.TruncateTime(s.DateOfSnapshot) == EntityFunctions.TruncateTime(DateTime.UtcNow)
                                                                  orderby s.TwitterAccount.Id
                                                                  select s).ToList();


                Assert.AreEqual(2, snapshotsFromToday.Count, "There are 2 snapshots, not 4");
                Assert.AreEqual(DateTime.UtcNow.Date, snapshotsFromToday[0].DateOfSnapshot.Date, "The snapshots are from today");
                Assert.AreEqual(snapshotsFromToday[0].DateOfSnapshot.Date, snapshotsFromToday[1].DateOfSnapshot.Date, "The snapshots are equal");
                Assert.AreEqual("MapleLeafs", snapshotsFromToday[0].TwitterAccount.Id, "The first snapshot is from MapleLeafs");
                Assert.AreEqual("phoenixcoyotes", snapshotsFromToday[1].TwitterAccount.Id, "The first snapshot is from phoenixcoyotes");
            }
        }
    }
}
