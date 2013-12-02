using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;
using SportsData.Models;

namespace SportsData.Mlb
{
    public class RawData
    {
        public static void AddOrUpdate(RawDataTable rawDataTable)
        {
            using (SportsDataContext db = new SportsDataContext())
            {
                rawDataTable.LastUpdated = DateTime.UtcNow;

                db.RawDataTables.AddOrUpdate(
                    r => r.SourceUrl,
                    rawDataTable);

                db.SaveChanges();
            }
        }
    }
}
