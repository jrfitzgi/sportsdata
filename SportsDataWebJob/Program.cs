using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SportsData;
using SportsData.Nhl;

namespace SportsDataWebJob
{
    /// <summary>
    /// Used as an Azure WebJob to update data
    /// TODO: Use Azure WebJob SDK for VS 2013 and auto-run when new blobs are created
    /// http://www.hanselman.com/blog/IntroducingWindowsAzureWebJobs.aspx
    /// </summary>
    public class Program
    {
        private static bool useLocalhost = false;

        public static void Main(string[] args)
        {
            Program.GetNhlGamesRtssSummary();
            Program.GetNhlGamesRtssRoster();
        }

        public static void GetNhlGamesRtssRoster()
        {
            Console.WriteLine("Running GetNhlGamesRtssRoster");

            string result =  WebUpdate.Update("NhlGamesRtssRoster", useLocalhost);
            Console.WriteLine(result);

            Console.WriteLine("Finished GetNhlGamesRtssRoster");
            Console.WriteLine();
        }

        public static void GetNhlGamesRtssSummary()
        {
            Console.WriteLine("Running GetNhlGamesRtssSummary");

            string result = WebUpdate.Update("NhlGamesRtssSummary", useLocalhost);
            Console.WriteLine(result);

            Console.WriteLine("Finished GetNhlGamesRtssRoster");
            Console.WriteLine();
        }
    }
}
