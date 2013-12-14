using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class SportsDataScriptsBaseClass
    {

        [TestInitialize]
        public void TestInitialize()
        {
            Database.SetInitializer<SportsDataContext>(new SportsDataContextMigrateDatabaseToLatestVersion());
            //Database.SetInitializer<SportsDataContext>(new SportsDataContextDropCreateDatabaseAlways());
            //Database.SetInitializer<SportsDataContext>(new SportsDataContextDropCreateDatabaseIfNotExists());
        }

    }
}
