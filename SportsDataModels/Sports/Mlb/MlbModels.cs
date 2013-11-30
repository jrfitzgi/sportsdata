using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public enum MlbTeamShortName
    {
        ARI,
        ATL,
        BAL,
        BOS,
        CHC,
        CHW,
        CIN,
        CLE,
        COL,
        DET,
        HOU,
        KC,
        LAA,
        LAD,
        MIA,
        MIL,
        MIN,
        NYM,
        NYY,
        OAK,
        PHI,
        PIT,
        SD,
        SF,
        SEA,
        STL,
        TB,
        TEX,
        TOR,
        WSH,
    }

    public enum MlbSeasonType
    {
        Spring = 1,
        Regular = 2,
        PostSeason = 3,
    }

    [Table("MlbTeams")]
    public class MlbTeam
    {
        [Key]
        public MlbTeamShortName ShortNameId { get; set; }

        public string ShortName { get; set; }

        public string City { get; set; }

        public string Name {get; set;}

        public string FullName
        {
            get
            {
                return this.City + " " + this.Name;
            }
        }

        // The name used in the espn.com tables. We need this here as a lookup key.
        public string EspnOpponentName { get; set; }
    }

    [Table("MlbGameSummary")]
    public class MlbGameSummary : GameSummary
    {
        [Display(Name = "Season Type")]
        public MlbSeasonType MlbSeasonType { get; set; }

        [Display(Name = "Innings")]
        public int Innings { get; set; }

        [Display(Name = "Wins To Date")]
        public int WinsToDate { get; set; }

        [Display(Name = "Losses To Date")]
        public int LossesToDate { get; set; }

        [Display(Name = "Postponed")]
        public bool Postponed { get; set; }
        [Display(Name = "W Pitcher")]
        public string WPitcher { get; set; }

        [Display(Name = "L Pitcher")]
        public string LPitcher { get; set; }

        [Display(Name = "Save Pitcher")]
        public string SavePitcher { get; set; }
    }
}

