using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Data.Entity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Nhl.Query
{
    public class MlbAttendanceContext : DbContext
    {
        public MlbAttendanceContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<MlbAttendanceContext>(new CreateDatabaseIfNotExists<MlbAttendanceContext>());
            //Database.SetInitializer<MlbAttendanceContext>(new DropCreateDatabaseAlways<MlbAttendanceContext>());   
        }

        public DbSet<MlbGameSummary> GameSummaries { get; set; }
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
