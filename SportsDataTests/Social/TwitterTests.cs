using System;
using System.Collections.Generic;
using System.Data.Entity;
//using System.Data.Objects;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsData;
using SportsData.Models;
using SportsData.Social;

namespace SportsDataTests
{
    [TestClass]
    public class TwitterTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void TwitterAccountsSeededTest()
        {
            using (SportsDataContext db = new SportsDataContext())
            {
                Assert.IsTrue(db.TwitterAccount_DbSet.Count() > 0, "There more than 0 twitter accounts seeded");
            }
        }

        [TestMethod]
        public void TwitterGetSnapshot()
        {
            TwitterAccount twitterAccount = new TwitterAccount { Id = "sanjosesharks", FriendlyName = "San Jose Sharks" };
            TwitterSnapshot twitterAccountSnapshot = TwitterQuery.GetTwitterSnapshot(twitterAccount);

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

            List<TwitterSnapshot> twitterAccountSnapshots = TwitterQuery.GetTwitterSnapshots(twitterAccounts);

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

            List<TwitterSnapshot> twitterAccountSnapshots = TwitterData.UpdateSnapshotsInDb(twitterAccounts);
            Assert.AreEqual(2, twitterAccountSnapshots.Count, "There are 2 snapshots");

            // Call Update again to make sure dupes are added
            twitterAccountSnapshots = TwitterData.UpdateSnapshotsInDb(twitterAccounts);

            // Make sure we have the right items in the db
            using (SportsDataContext db = new SportsDataContext())
            {
                List<TwitterSnapshot> snapshotsFromToday = (from s in db.TwitterSnapshot_DbSet.Include(x => x.TwitterAccount)
                                                            where DbFunctions.TruncateTime(s.DateOfSnapshot) == DbFunctions.TruncateTime(DateTime.UtcNow)
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
