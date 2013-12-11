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

    public class NhlGameStatsBaseModel
    {
        [Key, Column(Order = 0)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public NhlSeasonType NhlSeasonType { get; set; }

        //[Key, Column(Order=0)]
        [Required]
        public DateTime Date { get; set; }

        //[Key, Column(Order = 1)]
        [Required]
        public string Visitor { get; set; }

        //[Key, Column(Order = 2)]
        [Required]
        public string Home { get; set; }

        /// <summary>
        /// Use the later year as the year. Eg. use 2014 for the 2013-2014 season.
        /// </summary>
        public int Year { get; set; }

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
