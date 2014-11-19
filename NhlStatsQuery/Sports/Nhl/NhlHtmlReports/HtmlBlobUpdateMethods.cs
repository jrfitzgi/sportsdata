using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using SportsData;
using SportsData.Nhl;
using SportsData.Models;

namespace SportsData.Nhl
{
    public partial class HtmlBlob
    {
        public static void UpdateSeason([Optional] int year, [Optional] bool forceOverwrite)
        {
            year = NhlModelHelper.SetDefaultYear(year);

            List<Nhl_Games_Rtss> models;
            using (SportsDataContext db = new SportsDataContext())
            {
                models = (from m in db.NhlGameStatsRtssReports
                          where
                            m.Year == year
                          select m).ToList();
            }

            Dictionary<Uri, string> items = new Dictionary<Uri, string>();
            models.ForEach(m => items.Add(new Uri(m.RosterLink), m.Id.ToString()));
            HtmlBlob.GetAndStoreHtmlBlobs(HtmlBlobType.NhlRoster, items, forceOverwrite);
        }
    }
}
