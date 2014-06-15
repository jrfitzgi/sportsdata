using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    [Table("NhlGameStatsRtssReport")]
    public class NhlGameStatsRtssReportModel : NhlGameStatsBaseModel
    {
        [Required]
        public int GameNumber { get; set; }

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
