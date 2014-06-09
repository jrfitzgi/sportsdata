using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    [Table("NhlPlayerStatsBioModel")]
    public class NhlPlayerStatsBioModel : NhlPlayerStatsBaseModel
    {
        public DateTime DateOfBirth { get; set; }

        public string BirthCity { get; set; }

        public string StateOrProvince { get; set; }

        public string BirthCountry { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public string Shoots { get; set; }

        public int DraftYear { get; set; }

        public int DraftRound { get; set; }

        public int DraftOverall { get; set; }

        public bool Rookie { get; set; }

        public int GamesPlayed { get; set; }

        public int Goals { get; set; }

        public int Assists { get; set; }

        public int Points { get; set; }

        public int PlusMinus { get; set; }

        public int PIM { get; set; }

        public int ToiSecondsPerGame { get; set; }

    }
}
