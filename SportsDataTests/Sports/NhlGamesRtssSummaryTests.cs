using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HtmlAgilityPack;
using SportsData.Nhl;
using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class NhlGamesRtssSummaryTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void NhlGamesRtssSummary_ParseHtmlBlob()
        {

            string htmlBlob = HtmlBlob.RetrieveBlob(HtmlBlobType.NhlRoster, "1", new Uri("http://www.nhl.com/scores/htmlreports/20132014/GS030315.HTM"), true);
            Nhl_Games_Rtss_Summary report = NhlGamesRtssSummary.ParseHtmlBlob(1, htmlBlob);


        }
    }
}
