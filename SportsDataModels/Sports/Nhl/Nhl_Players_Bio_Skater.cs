using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    [Table("Nhl_Players_Bio_Skater")]
    public class Nhl_Players_Bio_Skater : Nhl_Players_BioBaseModel
    {
        public string Shoots { get; set; }

        public int Goals { get; set; }

        public int Assists { get; set; }

        public int Points { get; set; }

        public int PlusMinus { get; set; }

        public int PIM { get; set; }

        public int ToiSecondsPerGame { get; set; }

    }
}
