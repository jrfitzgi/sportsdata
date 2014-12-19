using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using SportsData;
using SportsData.Nhl;
using SportsData.Models;

namespace Gilmour
{
    public class Nhl
    {
        private static bool useLocalhost = false;

        public static string GetNhlGamesSummary()
        {
            //NhlGamesSummary.GetNewResultsOnly(saveToDb: true);

            return SportsData.WebUpdate.Update("NhlGamesSummary", useLocalhost);
        }

        public static string GetNhlGamesRtss()
        {
            //NhlGamesRtss.GetNewResultsOnly(saveToDb: true);
            //HtmlBlob.UpdateSeason(forceOverwrite: false);

            return WebUpdate.Update("NhlGamesRtss", useLocalhost);
        }

        public static string GetNhlGamesRtssRoster()
        {
            //NhlGamesRtssRoster.UpdateSeason(forceOverwrite: false);

            return WebUpdate.Update("NhlGamesRtssRoster", useLocalhost);
        }

        public static string GetNhlGamesRtssSummary()
        {
            //NhlGamesRtssSummary.UpdateSeason(forceOverwrite: false);

            return WebUpdate.Update("NhlGamesRtssSummary", useLocalhost);
        }

        public static void GetNhlPlayersBioGoalie(int year)
        {
            NhlPlayersBioGoalie.GetFullSeason(year: year, saveToDb: true);
        }

        public static void GetNhlPlayersBioSkater(int year)
        {
            NhlPlayersBioSkater.GetFullSeason(year: year, saveToDb: true);
        }

        public static void GetNhlPlayersRtssSkater(int year)
        {
            NhlPlayersRtssSkater.GetFullSeason(year: year, saveToDb: true);
        }
    }
}
