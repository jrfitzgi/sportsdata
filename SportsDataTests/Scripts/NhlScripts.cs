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
        public void Script_NhlGamesSummary()
        {
            DateTime fromDate = DateTime.Now.AddDays(-2);
            NhlGamesSummary.GetFullSeason(year: 2015, fromDate: fromDate, saveToDb: true);
        }

        /// <summary>
        /// Get the RTSS Reports for a season. No pre-reqs in the DB.
        /// </summary>
        [TestMethod]
        public void Script_NhlGamesRtss()
        {
            DateTime fromDate = DateTime.Now.AddDays(-100);
            NhlGamesRtss.GetFullSeason(year: 2015, fromDate: fromDate, saveToDb: true);
        }

        /// <summary>
        /// Get the game summaries for a season from the existing HTML blobs in blob storage.
        /// </summary>
        [TestMethod]
        public void Script_NhlGamesRtssSummary()
        {
            DateTime fromDate = DateTime.Now.AddDays(-100);
            NhlGamesRtssSummary.UpdateSeason(year: 2015, forceOverwrite: true, fromDate: fromDate);
        }

        /// <summary>
        /// Get the rosters for a season from the existing HTML blobs in blob storage.
        /// </summary>
        [TestMethod]
        public void Script_NhlGamesRtssRoster()
        {
            //NhlHtmlReportRoster.UpdateSeason(year: 2008, forceOverwrite: true);
            DateTime fromDate = DateTime.Now.AddDays(-2);
            NhlGamesRtssRoster.UpdateSeason(year: 2015, fromDate: fromDate, forceOverwrite: false);
        }

        /// <summary>
        /// Get the Player Stats Bios Skaters for a season
        /// </summary>
        [TestMethod]
        public void Script_NhlPlayersBioSkater()
        {
            for (int y = 2004; y >= 1998; y--)
            {
                NhlPlayersBioSkater.GetFullSeason(year: y, saveToDb: true);
            }
        }

        /// <summary>
        /// Get the Player Stats Bio Goalies for a season
        /// </summary>
        [TestMethod]
        public void Script_NhlPlayersBioGoalie()
        {
            for (int y = 2003; y >= 1998; y--)
            {
                NhlPlayersBioGoalie.GetFullSeason(year: y, saveToDb: true);
            }
        }


        /// <summary>
        /// Get the Player Stats Real-time Stats Skaters for a season
        /// </summary>
        [TestMethod]
        public void Script_NhlPlayersRtssSkater()
        {
            for (int y = 2004; y >= 1998; y--)
            {
                NhlPlayersRtssSkater.GetFullSeason(year: y, saveToDb: true);
            }
        }

        [TestMethod]
        public void Script_NhlDraftBook()
        {
            string fileName = @"C:\Users\Jordanf\Google Drive\Domi\Draft\NHL Draftbook 2013-1963 (V2.0).csv";
            List<Nhl_Draftbook> results = NhlDraftbook.UpdateDraftbook(fileName, true);
        }
    }
}
