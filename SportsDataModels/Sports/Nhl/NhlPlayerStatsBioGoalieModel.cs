using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    [Table("NhlPlayerStatsBioGoalieModel")]
    public class NhlPlayerStatsBioGoalieModel : NhlPlayerStatsBioBaseModel
    {
        public string Catches { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int OTSOLosses { get; set; }

        public double GAA { get; set; }

        public double SavePercentage { get; set; }

        public int Shutouts { get; set; }

    }
}
