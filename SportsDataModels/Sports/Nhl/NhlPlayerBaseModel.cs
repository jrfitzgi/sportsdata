using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public class NhlPlayerStatsBaseModel
    {
        [Key, Column(Order = 0)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public NhlSeasonType NhlSeasonType { get; set; }

        /// <summary>
        /// Use the later year as the year. Eg. use 2014 for the 2013-2014 season.
        /// </summary>
        public int Year { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public string Team { get; set; }

    }
}
