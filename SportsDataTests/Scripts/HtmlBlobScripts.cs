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
                            m.Year == 2014
                          select m).ToList();
            }

            Dictionary<Uri, string> items = new Dictionary<Uri, string>();
            models.ForEach(m => items.Add(new Uri(m.RosterLink), m.Id.ToString()));
            HtmlBlob.GetAndStoreHtmlBlobs(HtmlBlobType.NhlRoster, items, false);
        }
    }
}
