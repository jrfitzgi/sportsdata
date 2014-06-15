using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    [Table("NhlPlayerStatsRtssModel")]
    public class NhlPlayerStatsRtssModel : NhlPlayerStatsBaseModel
    {
        public int GamesPlayed { get; set; }

        public int Hits { get; set; }

        public int BlockedShots { get; set; }

        public int MissedShots { get; set; }

        public int Giveaways { get; set; }
        
        public int Takeaways { get; set; }
        
        public int FaceoffsWon { get; set; }
        
        public int FaceoffsLost { get; set; }
        
        public int FaceoffsTaken { get; set; }

        public double FaceoffWinPercentage { get; set; }

        public double PercentageOfTeamFaceoffsTaken { get; set; }

        public int Shots { get; set; }

        public int Goals { get; set; }

        public double ShootingPercentage { get; set; }

    }
}
