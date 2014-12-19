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
            }
            else
            {
                string command = args[0].ToLowerInvariant();

                switch (command)
                {
                    case "demographics":
                        Demographics.GetDemographics();
                        break;

                    case "mlb":
                        Mlb.GetMlbAttendance();
                        break;

                    case "nhlgamessummary":
                        Nhl.GetNhlGamesSummary();
                        break;
                    case "nhlgamesrtss":
                        Nhl.GetNhlGamesRtss();
                        break;
                    case "nhlgamesrtssroster":
                        Nhl.GetNhlGamesRtssRoster();
                        break;
                    case "nhlgamesrtsssummary":
                        Nhl.GetNhlGamesRtssSummary();
                        break;

                    // Probably should delete these if they are only used once per year
                    case "nhlplayersbiogoalie":
                        if (args.Length < 2)
                        {
                            Program.PrintInvalidArgumentsError("[year] not specified");
                        }
                        Nhl.GetNhlPlayersBioGoalie(Int32.Parse(args[1]));
                        break;
                    case "nhlplayersbioskater":
                        if (args.Length < 2)
                        {
                            Program.PrintInvalidArgumentsError("[year] not specified");
                        }
                        Nhl.GetNhlPlayersBioSkater(Int32.Parse(args[1]));
                        break;
                    case "nhlplayersrtssskater":
                        if (args.Length < 2)
                        {
                            Program.PrintInvalidArgumentsError("[year] not specified");
                        }
                        Nhl.GetNhlPlayersRtssSkater(Int32.Parse(args[1]));
                        break;
                    
                    default:
                        Program.PrintInvalidArgumentsError("Error: Argument '{0}' not recognized", args[0]);
                        break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void PrintInvalidArgumentsError(string formatMessage, params object[] args)
        {
            Console.WriteLine(formatMessage, args);
            Console.WriteLine();
            Program.PrintValidArguments();
        }
        
        private static void PrintValidArguments()
        {
            List<string> validArgs = new List<string>();
            validArgs.Add("Demographics");
            validArgs.Add("Mlb");
            validArgs.Add("NhlGamesSummary");
            validArgs.Add("NhlGamesRtss");
            validArgs.Add("NhlGamesRtssRoster");
            validArgs.Add("NhlGamesRtssSummary");
            validArgs.Add("NhlPlayersBioSkater [year]");
            validArgs.Add("NhlPlayersBioGoalie [year]");
            validArgs.Add("NhlPlayersRtssSkater [year]");

            Console.WriteLine("Valid arguments are:");
            validArgs.ForEach(va => Console.WriteLine("   " + va));
            Console.WriteLine();
            Console.WriteLine("Example: Gilmour.exe demographics");
        }
    }
}
