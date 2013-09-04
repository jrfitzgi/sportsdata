using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Data.Entity;

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

    [Table("MlbTeams")]
    public class MlbTeam
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public MlbTeamShortName ShortName { get; set; }

        public string LongName { get; set; }
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

