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
    public class HtmlBlobTests : SportsDataTestsBaseClass
    {
        /// <summary>
        /// Get the html blobs for rosters
        /// </summary>
        [TestMethod]
        public void HtmlBlob_Test()
        {
            List<NhlRtssReportModel> models;
            using (SportsDataContext db = new SportsDataContext())
            {
                models = (from m in db.NhlRtssReports
                          where
                            (m.Home == "VANCOUVER" || m.Visitor == "VANCOUVER") &&
                            m.Date >= new DateTime(2014,1,29)
                          select m).ToList();
            }

            Dictionary<Uri, string> items = new Dictionary<Uri, string>();
            models.ForEach(m => items.Add(new Uri(m.RosterLink), m.Id.ToString()));
            HtmlBlob.GetAndStoreHtmlBlobs(HtmlBlobType.NhlRoster, items, false);

            foreach (NhlRtssReportModel model in models)
            {
                string html = HtmlBlob.RetrieveBlob(HtmlBlobType.NhlRoster, model.Id.ToString(), new Uri(model.RosterLink), true);
            }
        }
    }
}
