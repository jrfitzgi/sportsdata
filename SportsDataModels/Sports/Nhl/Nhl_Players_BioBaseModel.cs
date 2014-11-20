using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public class Nhl_Players_BioBaseModel : Nhl_Players_BaseModel
    {
        public DateTime DateOfBirth { get; set; }

        public string BirthCity { get; set; }

        public string StateOrProvince { get; set; }

        public string BirthCountry { get; set; }

        public int HeightInches { get; set; }

        public int WeightLbs { get; set; }

        public int DraftYear { get; set; }

        public int DraftRound { get; set; }

        public int DraftOverall { get; set; }

        public string Rookie { get; set; }

        public int GamesPlayed { get; set; }

    }
}
