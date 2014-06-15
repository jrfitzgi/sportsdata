using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    [Table("NhlGameStatsSummaryModel")]
    public class NhlGameStatsSummaryModel : NhlGameStatsBaseModel
    {
        [Required]
        public int VisitorScore { get; set; }

        [Required]
        public int HomeScore { get; set; }

        [Required]
        public int Attendance { get; set; }

        public string OS { get; set; }

        public string WGoalie { get; set; }

        public string WGoal { get; set; }

        public int VisitorShots { get; set; }

        public int VisitorPPGF { get; set; }

        public int VisitorPPOpp { get; set; }

        public int VisitorPIM { get; set; }

        public int HomeShots { get; set; }

        public int HomePPGF { get; set; }

        public int HomePPOpp { get; set; }

        public int HomePIM { get; set; }
    }
}
