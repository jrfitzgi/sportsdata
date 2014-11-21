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

namespace SportsData.Demographics
{
    public class DemographicsData
    {
        public static void UpdateDatabase(List<DemographicsModel> data)
        {
            Console.WriteLine("Saving {0} records", data.Count);

            using (SportsDataContext db = new SportsDataContext())
            {
                db.Demographic_DbSet.AddOrUpdate(x => x.Zip, data.ToArray());

                db.SaveChanges();
            }

            Console.WriteLine("Done saving {0} records", data.Count);

        }

    }
}
