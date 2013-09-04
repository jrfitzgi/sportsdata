using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Data.Entity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Nhl
{
    [Table("NhlGameSummary")]
    public class NhlGameSummary : GameSummary
    {
        [Display(Name = "O/S")]
        public string OS { get; set; }

        [Display(Name = "W Goalie")]
        public string WGoalie { get; set; }

        [Display(Name = "W Goal")]
        public string WGoal { get; set; }

        [Required]
        [Display(Name = "Visitor: Shots")]
        public int VisitorShots { get; set; }

        [Required]
        [Display(Name = "Visitor: PPGF")]
        public int VisitorPPGF { get; set; }

        [Required]
        [Display(Name = "Visitor: PPOpp")]
        public int VisitorPPOpp { get; set; }

        [Required]
        [Display(Name = "Visitor: PIM")]
        public int VisitorPIM { get; set; }

        [Required]
        [Display(Name = "Home: Shots")]
        public int HomeShots { get; set; }

        [Required]
        [Display(Name = "Home: PPGF")]
        public int HomePPGF { get; set; }

        [Required]
        [Display(Name = "Home: PPOpp")]
        public int HomePPOpp { get; set; }

        [Required]
        [Display(Name = "Home: PIM")]
        public int HomePIM { get; set; }

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
