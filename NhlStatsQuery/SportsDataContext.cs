using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Data.Entity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SportsData.Nhl;
using SportsData.Mlb;

namespace SportsData
{
    public class SportsDataContext : DbContext
    {
        public SportsDataContext()
            : base("DefaultConnection")
        {
            //Database.SetInitializer<MlbAttendanceContext>(new DropCreateDatabaseAlways<MlbAttendanceContext>()); 
            Database.SetInitializer<SportsDataContext>(new CreateDatabaseIfNotExists<SportsDataContext>());
            //Database.SetInitializer<SportsDataContext>(new SportsDataContextInitializer());  
        }

        // Mlb
        public DbSet<MlbTeam> MlbTeams { get; set; }
        public DbSet<MlbGameSummary> MlbGameSummaries { get; set; }

        // Nhl
        public DbSet<NhlGameSummary> NhlGameSummaries { get; set; }

        
    }

    //public class SportsDataContextInitializer : CreateDatabaseIfNotExists<SportsDataContext>
    //{
    //    protected override void Seed(SportsDataContext context)
    //    { }
    //}

}

