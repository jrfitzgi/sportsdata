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
        [TestMethod]
        public void Script_HtmlBlobs()
        {
            List<NhlRtssReportModel> models;
            using (SportsDataContext db = new SportsDataContext())
            {
                models = (from m in db.NhlRtssReports
                          where
                            m.NhlSeasonType == NhlSeasonType.PreSeason &&
                            m.Year == 2014
                          select m).ToList();
            }

            HtmlBlob.GetAndStoreHtmlBlobs(HtmlBlobType.NhlRoster, models.Select(m => new Uri(m.RosterLink)).ToList(), false);
        }
    }
}
