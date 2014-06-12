using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public enum NhlSeasonType
    {
        None = 0,
        PreSeason = 1,
        RegularSeason = 2,
        Playoff = 3
    }

    public static class NhlModelHelper
    {
        /// <summary>
        /// If the year is 0, set it to the default. Else, leave it as is.
        /// </summary>
        public static int SetDefaultYear(int year)
        {
            if (year == 0)
            {
                // Default to the current year
                year = NhlModelHelper.GetSeason(DateTime.Now).Item2;
            }

            return year;
        }

        /// <summary>
        /// Gets the NHL season in YYYN-YYYM format, where M = N+1. Eg. 2012-2013.
        /// </summary>
        public static Tuple<int, int> GetSeason(DateTime date)
        {

            if (date.Month <= 7)
            {
                // Jan-Jul
                return new Tuple<int, int>(date.Year - 1, date.Year);
            }
            else
            {
                // Aug-Dec
                return new Tuple<int, int>(date.Year, date.Year + 1);
            }
        }
    }
}
