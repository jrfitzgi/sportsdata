using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using SportsData.Demographics;
using SportsData.Models;

namespace Gilmour
{
    public class Demographics
    {
        public static void GetDemographics()
        {
            // Try to read zips from app.config
            string zipCsv = ConfigurationManager.AppSettings["zipCodes"];

            List<int> allZipCodes = zipCsv.Replace(" ", String.Empty).Split(',').ToList().ConvertAll<int>(x => Convert.ToInt32(x));
            List<int> missingZipCodes = new List<int>(); //allZipCodes.GetRange(592, 1);
            using (SportsDataContext db = new SportsDataContext())
            {
                missingZipCodes = allZipCodes.Except(db.Demographic_DbSet.Select(z => z.Zip).ToList()).ToList();
            }

            if (missingZipCodes.Count == 0)
            {
                Console.WriteLine("All zip codes have been retrieved and stored in the database. To update existing zip code data, delete it from the database and re-run");
                Console.WriteLine("Zip codes in the list of zip codes to retrieve:");
                Console.WriteLine(allZipCodes);
            }

            List<DemographicsModel> results = DemographicsQuery.GetDemographics(missingZipCodes, false, 0);
            DemographicsData.UpdateDatabase(results);
        }
    }
}
