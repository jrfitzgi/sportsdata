using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

using SportsData.Nhl;
using SportsData.Mlb;

namespace SportsData
{
    public class SportsDataContext : DbContext
    {
        public SportsDataContext()
            : base("DefaultConnection")
            //: base("ProdConnection")
        {
            //Database.SetInitializer<SportsDataContext>(new SportsDataContextDropCreateDatabaseAlways());
            //Database.SetInitializer<SportsDataContext>(new SportsDataContextDropCreateDatabaseIfNotExists());
            //Database.SetInitializer<SportsDataContext>(new SportsDataContextDropCreateDatabaseIfModelChanges());
            //Database.SetInitializer<SportsDataContext>(null); // Use for ProdConnection migrations

            //Database.Initialize(false);
        }

        // Mlb
        public DbSet<MlbTeam> MlbTeams { get; set; }
        public DbSet<MlbGameSummary> MlbGameSummaries { get; set; }

        // Nhl
        public DbSet<NhlGameSummary> NhlGameSummaries { get; set; }

        public static void Seed(SportsDataContext context)
        {
            context.MlbTeams.AddOrUpdate(
                t => t.ShortNameId,
                new MlbTeam { ShortNameId = MlbTeamShortName.ARI, City = "Arizona", Name = "Diamondbacks" },
                new MlbTeam { ShortNameId = MlbTeamShortName.ATL, City = "Atlanta", Name = "Braves" },
                new MlbTeam { ShortNameId = MlbTeamShortName.BAL, City = "Baltimore", Name = "Orioles" },
                new MlbTeam { ShortNameId = MlbTeamShortName.BOS, City = "Boston", Name = "Red Sox" },
                new MlbTeam { ShortNameId = MlbTeamShortName.CHC, City = "Chicago", Name = "Cubs", EspnOpponentName = "Cubs" },
                new MlbTeam { ShortNameId = MlbTeamShortName.CHW, City = "Chicago", Name = "White Sox", EspnOpponentName = "White Sox" },
                new MlbTeam { ShortNameId = MlbTeamShortName.CIN, City = "Cincinnati", Name = "Reds" },
                new MlbTeam { ShortNameId = MlbTeamShortName.CLE, City = "Cleveland", Name = "Indians" },
                new MlbTeam { ShortNameId = MlbTeamShortName.COL, City = "Colorado", Name = "Rockies" },
                new MlbTeam { ShortNameId = MlbTeamShortName.DET, City = "Detroit", Name = "Tigers" },
                new MlbTeam { ShortNameId = MlbTeamShortName.HOU, City = "Houston", Name = "Astros" },
                new MlbTeam { ShortNameId = MlbTeamShortName.KC, City = "Kansas City", Name = "Royals" },
                new MlbTeam { ShortNameId = MlbTeamShortName.LAA, City = "Los Angeles", Name = "Angels", EspnOpponentName = "LA Angels" },
                new MlbTeam { ShortNameId = MlbTeamShortName.LAD, City = "Los Angeles", Name = "Dodgers", EspnOpponentName = "LA Dodgers" },
                new MlbTeam { ShortNameId = MlbTeamShortName.MIA, City = "Miami", Name = "Marlins" },
                new MlbTeam { ShortNameId = MlbTeamShortName.MIL, City = "Milwaukee", Name = "Brewers" },
                new MlbTeam { ShortNameId = MlbTeamShortName.MIN, City = "Minnesota", Name = "Twins" },
                new MlbTeam { ShortNameId = MlbTeamShortName.NYM, City = "New York", Name = "Mets", EspnOpponentName = "NY Mets" },
                new MlbTeam { ShortNameId = MlbTeamShortName.NYY, City = "New York", Name = "Yankees", EspnOpponentName = "NY Yankees" },
                new MlbTeam { ShortNameId = MlbTeamShortName.OAK, City = "Oakland", Name = "Athletics"},
                new MlbTeam { ShortNameId = MlbTeamShortName.PHI, City = "Philadelphia", Name = "Phillies" },
                new MlbTeam { ShortNameId = MlbTeamShortName.PIT, City = "Pittsburgh", Name = "Pirates" },
                new MlbTeam { ShortNameId = MlbTeamShortName.SD, City = "San Diego", Name = "Padres" },
                new MlbTeam { ShortNameId = MlbTeamShortName.SF, City = "San Francisco", Name = "Giants" },
                new MlbTeam { ShortNameId = MlbTeamShortName.SEA, City = "Seattle", Name = "Mariners" },
                new MlbTeam { ShortNameId = MlbTeamShortName.STL, City = "St. Louis", Name = "Cardinals" },
                new MlbTeam { ShortNameId = MlbTeamShortName.TB, City = "Tampa Bay", Name = "Rays" },
                new MlbTeam { ShortNameId = MlbTeamShortName.TEX, City = "Texas", Name = "Rangers" },
                new MlbTeam { ShortNameId = MlbTeamShortName.TOR, City = "Toronto", Name = "Blue Jays" },
                new MlbTeam { ShortNameId = MlbTeamShortName.WSH, City = "Washington", Name = "Nationals" }
                );

            context.SaveChanges();

            foreach (MlbTeam mlbTeam in context.MlbTeams)
            {
                mlbTeam.ShortName = mlbTeam.ShortNameId.ToString("g");
            
                if (String.IsNullOrWhiteSpace(mlbTeam.EspnOpponentName))
                {
                    mlbTeam.EspnOpponentName = mlbTeam.City;
                }
            }

            context.SaveChanges();
        }

    }

    public class SportsDataContextDropCreateDatabaseAlways : DropCreateDatabaseAlways<SportsDataContext>
    {
        protected override void Seed(SportsDataContext context)
        {
            SportsDataContext.Seed(context);
            base.Seed(context);
        }
    }

    public class SportsDataContextDropCreateDatabaseIfNotExists : CreateDatabaseIfNotExists<SportsDataContext>
    {
        protected override void Seed(SportsDataContext context)
        {
            SportsDataContext.Seed(context);
            base.Seed(context);
        }
    }

    public class SportsDataContextDropCreateDatabaseIfModelChanges : DropCreateDatabaseIfModelChanges<SportsDataContext>
    {
        protected override void Seed(SportsDataContext context)
        {
            SportsDataContext.Seed(context);
            base.Seed(context);
        }
    }

}

