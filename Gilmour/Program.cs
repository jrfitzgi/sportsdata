using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SportsData.Demographics;
using SportsData.Models;

namespace Gilmour
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine();

            if (args == null || args.Count() == 0)
            {
                Console.WriteLine("Error: No arguments provided");
                Program.PrintValidArguments();
                //return;
            }
            else
            {
                string command = args[0].ToLowerInvariant();

                switch (command)
                {
                    case "demographics":
                        Program.GetDemographics();
                        break;
                    default:
                        Console.WriteLine("Error: Argument '{0}' not recognized");
                        Program.PrintValidArguments();
                        break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void PrintValidArguments()
        {
            List<string> validArgs = new List<string> { "demographics" };
            Console.WriteLine("Valid arguments are:");
            validArgs.ForEach(va => Console.WriteLine("   " + va));
            Console.WriteLine();
            Console.WriteLine("Example: Gilmour.exe demographics");
        }

        private static void GetDemographics()
        {
            // Try to read zips from app.config
            string zipCsv = ConfigurationManager.AppSettings["zipCodes"];

            List<int> allZipCodes = zipCsv.Replace(" ", String.Empty).Split(',').ToList().ConvertAll<int>(x => Convert.ToInt32(x));
            List<int> missingZipCodes = new List<int>(); //allZipCodes.GetRange(592, 1);
            using (SportsDataContext db = new SportsDataContext())
            {
                missingZipCodes = allZipCodes.Except(db.Demographics.Select(z => z.Zip).ToList()).ToList();
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
