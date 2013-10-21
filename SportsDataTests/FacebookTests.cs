using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsData;
using SportsData.Facebook;

namespace FacebookTests
{
    [TestClass]
    public class FacebookTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Database.SetInitializer<SportsDataContext>(new SportsDataContextDropCreateDatabaseAlways());
            //Database.SetInitializer<SportsDataContext>(new SportsDataContextDropCreateDatabaseIfNotExists());
        }

        [TestMethod]
        public void FacebookAccountsToFollowTest()
        {
            using (SportsDataContext db = new SportsDataContext())
            {
                Assert.IsTrue(db.TwitterAccountsToFollow.Count() > 0, "There are more than 0 accounts seeded");
            }
        }

        [TestMethod]
        public void FacebookGetSnapshot()
        {
            FacebookAccount account = new FacebookAccount { Id = "NHLBruins", FriendlyName = "Boston Bruins" };
            FacebookAccountSnapshot accountSnapshot = FacebookQuery.GetSnapshot(account);

            Assert.AreEqual(DateTime.UtcNow.Date, accountSnapshot.DateOfSnapshot.Date, "The snapshot is from today");
            Assert.IsTrue(accountSnapshot.TotalLikes > 0, "There are more than 0 tweets");
            //Assert.IsTrue(accountSnapshot.PeopleTalkingAboutThis > 0, "There are more than 0 people talking about this");
            //Assert.IsTrue(accountSnapshot.MostPopularWeek > DateTime.MinValue, "The most popular week is not the default DateTime.MinValue");
            //Assert.AreNotEqual(String.Empty, accountSnapshot.MostPopularCity, "The most popular city is not empty");
            //Assert.AreNotEqual(String.Empty, accountSnapshot.MostPopularAgeGroup, "The most popular age group is not empty");

            Assert.AreEqual(account.Id, accountSnapshot.FacebookAccountId, "There account name is correct in the snapshot");
            Assert.AreEqual("NHLBruins", accountSnapshot.FacebookAccountId, "There account name is NHLBruins");
        }

        //[TestMethod]
        //public void TwitterGetSnapshots()
        //{
        //    List<TwitterAccount> twitterAccounts = new List<TwitterAccount>();
        //    twitterAccounts.Add(new TwitterAccount { Id = "MapleLeafs", FriendlyName = "Toronto Maple Leafs" });
        //    twitterAccounts.Add(new TwitterAccount { Id = "phoenixcoyotes", FriendlyName = "Phoenix Coyotes" });

        //    List<TwitterAccountSnapshot> twitterAccountSnapshots = TwitterQuery.GetTwitterSnapshots(twitterAccounts);

        //    Assert.AreEqual(2, twitterAccountSnapshots.Count, "There are 2 snapshots");

        //    Assert.AreEqual(DateTime.UtcNow.Date, twitterAccountSnapshots[0].DateOfSnapshot.Date, "The snapshots are from today");
        //    Assert.AreEqual(twitterAccountSnapshots[0].DateOfSnapshot.Date, twitterAccountSnapshots[1].DateOfSnapshot.Date, "The snapshots are equal");
        //    Assert.AreEqual("MapleLeafs", twitterAccountSnapshots[0].TwitterAccountId, "The first snapshot is from MapleLeafs");
        //    Assert.AreEqual("phoenixcoyotes", twitterAccountSnapshots[1].TwitterAccountId, "The first snapshot is from phoenixcoyotes");
        //}

        //[TestMethod]
        //public void TwitterUpdateSnapshotsInDb()
        //{
        //    List<TwitterAccount> twitterAccounts = new List<TwitterAccount>();
        //    twitterAccounts.Add(new TwitterAccount { Id = "MapleLeafs", FriendlyName = "Toronto Maple Leafs" });
        //    twitterAccounts.Add(new TwitterAccount { Id = "phoenixcoyotes", FriendlyName = "Phoenix Coyotes" });

        //    List<TwitterAccountSnapshot> twitterAccountSnapshots = TwitterData.UpdateSnapshotsInDb(twitterAccounts);
        //    Assert.AreEqual(2, twitterAccountSnapshots.Count, "There are 2 snapshots");

        //    // Call Update again to make sure dupes are added
        //    twitterAccountSnapshots = TwitterData.UpdateSnapshotsInDb(twitterAccounts);

        //    // Make sure we have the right items in the db
        //    using (SportsDataContext db = new SportsDataContext())
        //    {
        //        List<TwitterAccountSnapshot> snapshotsFromToday = (from s in db.TwitterSnapshots.Include(x => x.TwitterAccount)
        //                                                          where EntityFunctions.TruncateTime(s.DateOfSnapshot) == EntityFunctions.TruncateTime(DateTime.UtcNow)
        //                                                          orderby s.TwitterAccount.Id
        //                                                          select s).ToList();


        //        Assert.AreEqual(2, snapshotsFromToday.Count, "There are 2 snapshots, not 4");
        //        Assert.AreEqual(DateTime.UtcNow.Date, snapshotsFromToday[0].DateOfSnapshot.Date, "The snapshots are from today");
        //        Assert.AreEqual(snapshotsFromToday[0].DateOfSnapshot.Date, snapshotsFromToday[1].DateOfSnapshot.Date, "The snapshots are equal");
        //        Assert.AreEqual("MapleLeafs", snapshotsFromToday[0].TwitterAccount.Id, "The first snapshot is from MapleLeafs");
        //        Assert.AreEqual("phoenixcoyotes", snapshotsFromToday[1].TwitterAccount.Id, "The first snapshot is from phoenixcoyotes");
        //    }
        //}
    }
}
