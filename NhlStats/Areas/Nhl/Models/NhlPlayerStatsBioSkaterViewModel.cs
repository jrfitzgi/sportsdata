using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsData.Areas.Nhl.Models
{
    public class NhlPlayerStatsBioSkaterViewModel
    {
        public string Name { get; set; }
        public int DraftYear { get; set; }
        public int SeasonsPlayed { get; set; }
        public double AvgGamesPerSeason { get; set; }
        public int TotalGamesPlayed { get; set; }
    }
}