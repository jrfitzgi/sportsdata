using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Data.Entity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData
{
    [Table("GameSummary")]
    public class GameSummary
    {
        public enum SeasonType
        {
            PreSeason = 1,
            RegularSeason = 2,
            Playoff = 3
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Season { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int GameType { get; set; }

        [Required]
        public string Visitor { get; set; }

        [Required]
        [Display(Name = "Visitor Score")]
        public int VisitorScore { get; set; }

        [Required]
        public string Home { get; set; }

        [Required]
        [Display(Name = "Home Score")]
        public int HomeScore { get; set; }

        //[Display(Name = "O/S")]
        //public string OS { get; set; }

        //[Display(Name = "W Goalie")]
        //public string WGoalie { get; set; }

        //[Display(Name = "W Goal")]
        //public string WGoal { get; set; }

        //[Required]
        //[Display(Name = "Visitor: Shots")]
        //public int VisitorShots { get; set; }

        //[Required]
        //[Display(Name = "Visitor: PPGF")]
        //public int VisitorPPGF { get; set; }

        //[Required]
        //[Display(Name = "Visitor: PPOpp")]
        //public int VisitorPPOpp { get; set; }

        //[Required]
        //[Display(Name = "Visitor: PIM")]
        //public int VisitorPIM { get; set; }

        //[Required]
        //[Display(Name = "Home: Shots")]
        //public int HomeShots { get; set; }

        //[Required]
        //[Display(Name = "Home: PPGF")]
        //public int HomePPGF { get; set; }

        //[Required]
        //[Display(Name = "Home: PPOpp")]
        //public int HomePPOpp { get; set; }

        //[Required]
        //[Display(Name = "Home: PIM")]
        //public int HomePIM { get; set; }

        [Required]
        [Display(Name = "Attendance")]
        public int Att { get; set; }

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
