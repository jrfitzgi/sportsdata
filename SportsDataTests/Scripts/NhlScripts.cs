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
    public class NhlScripts : SportsDataScriptsBaseClass
    {
        [TestMethod]
        public void Script_NhlGetGameSummary()
        {
            NhlGameSummary.UpdateSeason(2014);
        }

        [TestMethod]
        public void Script_NhlGetRtssReport()
        {
            NhlRtssReport.UpdateSeason(2014);
        }

        [TestMethod]
        public void Script_NhlGetHtmlReportSummary()
        {
            NhlHtmlReportSummary.UpdateSeason(2014);
        }
    }
}
