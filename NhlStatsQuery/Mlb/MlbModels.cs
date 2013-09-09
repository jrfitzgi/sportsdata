using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Mlb
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
    }

    [Table("MlbGameSummary")]
    public class MlbGameSummary : GameSummary
    {
        [Display(Name = "W Pitcher")]
        public string WPitcher { get; set; }

        [Display(Name = "L Pitcher")]
        public string LPitcher { get; set; }

        [Display(Name = "Save Pitcher")]
        public string SavePitcher { get; set; }

        [Required]
        [Display(Name = "Innings")]
        public int Innings { get; set; }
    }
}

