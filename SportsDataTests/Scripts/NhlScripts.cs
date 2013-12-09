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
    public class NhlScripts
    {
        [TestInitialize]
        public void TestInitialize()
        {
            //Database.SetInitializer<SportsDataContext>(new SportsDataContextDropCreateDatabaseAlways());
            Database.SetInitializer<SportsDataContext>(new SportsDataContextDropCreateDatabaseIfNotExists());
        }

        [TestMethod]
        public void NhlGetRtssReport()
        {
            NhlSeasonType nhlSeasonType = NhlSeasonType.RegularSeason;
            int year = 2011;

            NhlRtssReport nhlRtssReport = new NhlRtssReport();
            List<HtmlNode> pages = nhlRtssReport.GetStatPages(year, nhlSeasonType);
            Assert.AreEqual(41, pages.Count);

            //List<HtmlNode> rowNodes = NhlRtssReport.GetRowsFromTable(tableNode);
            //NhlRtssReportModel model = nhlRtssReport.MapHtmlRowToModel(rowNodes[0], nhlSeasonType);

        }


    }
}
