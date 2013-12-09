using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    [Table("NhlRtssReport")]
    public class NhlRtssReportModel : NhlGameStatsBaseModel
    {
        [Required]
        public int GameNumber { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        public string Visitor { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        public string Home { get; set; }

        public string RosterLink { get; set; }

        public string GameLink { get; set; }

        public string EventsLink { get; set; }

        public string FaceOffsLink { get; set; }

        public string PlayByPlayLink { get; set; }

        public string ShotsLink { get; set; }

        public string HomeToiLink { get; set; }

        public string VistorToiLink { get; set; }

        public string ShootoutLink { get; set; }
    }
}
