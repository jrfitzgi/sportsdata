using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public class NhlGameStatsBaseModel
    {
        [Key, Column(Order = 0)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public NhlSeasonType NhlSeasonType { get; set; }

        //[Key, Column(Order=0)]
        [Required]
        public DateTime Date { get; set; }

        //[Key, Column(Order = 1)]
        [Required]
        public string Visitor { get; set; }

        //[Key, Column(Order = 2)]
        [Required]
        public string Home { get; set; }

        /// <summary>
        /// Use the later year as the year. Eg. use 2014 for the 2013-2014 season.
        /// </summary>
        public int Year { get; set; }

    }
}
