using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    [Table("NhlGameSummary")]
    public class NhlGameSummaryModel : NhlGameStatsBaseModel
    {

        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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

        [Required]
        [Display(Name = "Attendance")]
        public int Attendance { get; set; }

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
    }
}
