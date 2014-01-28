using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsData;
using SportsData.Nhl;
using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class HtmlBlobScripts : SportsDataTestsBaseClass
    {
        /// <summary>
        /// Get the html blobs for rosters
        /// </summary>
        [TestMethod]
        public void Script_HtmlBlobs_Roster()
        {
            HtmlBlob.UpdateSeason();
        }
    }
}
