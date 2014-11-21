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
    public class FacebookTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void FacebookAccountsSeededTest()
        {
            using (SportsDataContext db = new SportsDataContext())
            {
                Assert.IsTrue(db.TwitterAccount_DbSet.Count() > 0, "There are more than 0 accounts seeded");
            }
        }

        [TestMethod]
        public void FacebookGetSnapshot()
        {
            FacebookAccount account = new FacebookAccount { Id = "NHLBruins", FriendlyName = "Boston Bruins" };
           
            FacebookSnapshot accountSnapshot = FacebookQuery.GetFacebookSnapshot(account);

            Assert.AreEqual(DateTime.UtcNow.Date, accountSnapshot.DateOfSnapshot.Date, "The snapshot is from today");
            Assert.IsTrue(accountSnapshot.TotalLikes > 0, "There are more than 0 likes");
            Assert.IsTrue(accountSnapshot.PeopleTalkingAboutThis > 0, "There are more than 0 people talking about this");
            Assert.IsTrue(accountSnapshot.MostPopularWeek > DateTime.MinValue, "The most popular week is not the default DateTime.MinValue");
            Assert.AreNotEqual(String.Empty, accountSnapshot.MostPopularCity, "The most popular city is not empty");
            Assert.AreNotEqual(String.Empty, accountSnapshot.MostPopularAgeGroup, "The most popular age group is not empty");

            Assert.AreEqual(account.Id, accountSnapshot.FacebookAccountId, "There account name is correct in the snapshot");
            Assert.AreEqual("NHLBruins", accountSnapshot.FacebookAccountId, "There account name is NHLBruins");
        }

        [TestMethod]
        public void FacebookGetSnapshots()
        {
            List<FacebookAccount> accounts = new List<FacebookAccount>();
            accounts.Add(new FacebookAccount { Id = "NHLBruins", FriendlyName = "Boston Bruins" });
            accounts.Add(new FacebookAccount { Id = "torontomapleleafs", FriendlyName = "Toronto Maple Leafs" });

            List<FacebookSnapshot> snapshots = FacebookQuery.GetFacebookSnapshots(accounts);

            Assert.AreEqual(2, snapshots.Count, "There are 2 snapshots");

            Assert.AreEqual(DateTime.UtcNow.Date, snapshots[0].DateOfSnapshot.Date, "The snapshots are from today");
            Assert.AreEqual(snapshots[0].DateOfSnapshot.Date, snapshots[1].DateOfSnapshot.Date, "The snapshots are equal");
            Assert.AreEqual("NHLBruins", snapshots[0].FacebookAccountId, "The first snapshot is from NHLBruins");
            Assert.AreEqual("torontomapleleafs", snapshots[1].FacebookAccountId, "The first snapshot is from torontomapleleafs");
        }

        [TestMethod]
        public void FacebookUpdateSnapshotsInDb()
        {
            List<FacebookAccount> accounts = new List<FacebookAccount>();
            accounts.Add(new FacebookAccount { Id = "NHLBruins", FriendlyName = "Boston Bruins" });
            accounts.Add(new FacebookAccount { Id = "torontomapleleafs", FriendlyName = "Toronto Maple Leafs" });

            List<FacebookSnapshot> snapshots = FacebookData.UpdateSnapshotsInDb(accounts);
            Assert.AreEqual(2, snapshots.Count, "There are 2 snapshots");

            // Call Update again to make sure dupes are added
            snapshots = FacebookData.UpdateSnapshotsInDb(accounts);

            // Make sure we have the right items in the db
            using (SportsDataContext db = new SportsDataContext())
            {
                List<FacebookSnapshot> snapshotsFromToday = (from s in db.FacebookSnapshot_DbSet.Include(x => x.FacebookAccount)
                                                             where DbFunctions.TruncateTime(s.DateOfSnapshot) == DbFunctions.TruncateTime(DateTime.UtcNow)
                                                                   orderby s.FacebookAccount.Id
                                                                   select s).ToList();


                Assert.AreEqual(2, snapshotsFromToday.Count, "There are 2 snapshots, not 4");
                Assert.AreEqual(DateTime.UtcNow.Date, snapshotsFromToday[0].DateOfSnapshot.Date, "The snapshots are from today");
                Assert.AreEqual(snapshotsFromToday[0].DateOfSnapshot.Date, snapshotsFromToday[1].DateOfSnapshot.Date, "The snapshots are equal");
                Assert.AreEqual("NHLBruins", snapshotsFromToday[0].FacebookAccount.Id, "The first snapshot is from NHLBruins");
                Assert.AreEqual("torontomapleleafs", snapshotsFromToday[1].FacebookAccount.Id, "The first snapshot is from torontomapleleafs");
            }
        }
    }
}
