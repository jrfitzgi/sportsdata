using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsData;
using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class UpdateControllerTests
    {
        [TestMethod]
        public void UpdateControllerTest()
        {
            bool useLocalhost = true;
            string result = WebUpdate.Update("NhlGamesSummary", useLocalhost);
        }
    }
}
