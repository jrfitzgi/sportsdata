using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class SportsDataTestsBaseClass
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Database.SetInitializer<SportsDataContext>(new SportsDataContextMigrateDatabaseToLatestVersion());
        }
    }
}
