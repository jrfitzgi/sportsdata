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
        /// <summary>
        /// Get the game summaries for a season. No pre-reqs in the DB.
        /// </summary>
        [TestMethod]
        public void Script_NhlGameStatsSummary()
        {
            NhlGameStatsSummary.GetFullSeason(year: 2014, saveToDb:true);
        }

        /// <summary>
        /// Get the RTSS Reports for a season. No pre-reqs in the DB.
        /// </summary>
        [TestMethod]
        public void Script_NhlGameStatsRtssReport()
        {
            NhlGameStatsRtssReport.GetFullSeason(year:2013, saveToDb:true);
        }

        /// <summary>
        /// Get the game summaries for a season from the existing HTML blobs in blob storage.
        /// </summary>
        [TestMethod]
        public void Script_NhlHtmlReportSummary()
        {
            NhlHtmlReportSummary.UpdateSeason(year: 2014, forceOverwrite: true);
        }

        /// <summary>
        /// Get the rosters for a season from the existing HTML blobs in blob storage.
        /// </summary>
        [TestMethod]
        public void Script_NhlHtmlReportRoster()
        {
            //NhlHtmlReportRoster.UpdateSeason(year: 2008, forceOverwrite: true);
            NhlHtmlReportRoster.UpdateSeason();
        }

        /// <summary>
        /// Get the Player Stats Bios for a season
        /// </summary>
        [TestMethod]
        public void Script_NhlPlayerStatsBio()
        {
            for (int y = 2004; y >=1998; y--)
            {
                NhlPlayerStatsBio.GetFullSeason(year: y, saveToDb: true);
            }
        }
    }
}
