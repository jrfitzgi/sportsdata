using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace SportsData.Models
{
    public class SportsDataContext : DbContext
    {
        public SportsDataContext()
            : base("DefaultConnection")
            //: base("ProdConnection")
        {
        }

        // Mlb
        public DbSet<MlbTeam> MlbTeams { get; set; }
        public DbSet<MlbGameSummaryModel> MlbGameSummaries { get; set; }

        // Nhl
        public DbSet<NhlPlayerStatsBioSkaterModel> NhlPlayerStatsBioSkaters { get; set; }
        public DbSet<NhlPlayerStatsBioGoalieModel> NhlPlayerStatsBioGoalies { get; set; }
        public DbSet<NhlPlayerStatsRtssSkaterModel> NhlPlayerStatsRtssSkaters { get; set; }
        
        public DbSet<NhlGameStatsSummaryModel> NhlGameStatsSummaries { get; set; }
        public DbSet<NhlGameStatsRtssReportModel> NhlGameStatsRtssReports { get; set; }

        public DbSet<NhlHtmlReportSummaryModel> NhlHtmlReportSummaries { get; set; }
        public DbSet<NhlHtmlReportRosterModel> NhlHtmlReportRosters { get; set; }

        public DbSet<NhlDraftbookModel> NhlDraftbookModels { get; set; }

        // Twitter
        public DbSet<TwitterAccount> TwitterAccountsToFollow { get; set; }
        public DbSet<TwitterSnapshot> TwitterSnapshots { get; set; }

        // Facebook
        public DbSet<FacebookAccount> FacebookAccountsToFollow { get; set; }
        public DbSet<FacebookSnapshot> FacebookSnapshots { get; set; }

        // Demographics
        public DbSet<DemographicsModel> Demographics { get; set; }

        public static void Seed(SportsDataContext context)
        {
            #region Mlb

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
                new MlbTeam { ShortNameId = MlbTeamShortName.OAK, City = "Oakland", Name = "Athletics" },
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

            #endregion

            #region Twitter

            context.TwitterAccountsToFollow.AddOrUpdate(
                t => t.Id,
                new TwitterAccount { Id = "anaheimducks", FriendlyName = "Anaheim Ducks" },
                new TwitterAccount { Id = "NHLBruins", FriendlyName = "Boston Bruins" },
                new TwitterAccount { Id = "buffalosabres", FriendlyName = "Bufalo Sabres" },
                new TwitterAccount { Id = "NHLFlames", FriendlyName = "Calgary Flames" },
                new TwitterAccount { Id = "NHLCanes", FriendlyName = "Carolina Hurricanes" },
                new TwitterAccount { Id = "NHLBlackhawks", FriendlyName = "Chicago Blackhawks" },
                new TwitterAccount { Id = "Avalanche", FriendlyName = "Colorado Avalanche" },
                new TwitterAccount { Id = "bluejacketsnhl", FriendlyName = "Columbus Blue Jackets" },
                new TwitterAccount { Id = "DallasStars", FriendlyName = "Dallas Stars" },
                new TwitterAccount { Id = "DetroitRedWings", FriendlyName = "Detroit Red Wings" },
                new TwitterAccount { Id = "EdmontonOilers", FriendlyName = "Edmonton Oilers" },
                new TwitterAccount { Id = "FlaPanthers", FriendlyName = "Florida Panthers" },
                new TwitterAccount { Id = "LAKings", FriendlyName = "Los Angeles Kings" },
                new TwitterAccount { Id = "mnwild", FriendlyName = "Minnesota Wild" },
                new TwitterAccount { Id = "CanadiensMTL", FriendlyName = "Montreal Canadians" },
                new TwitterAccount { Id = "PredsNHL", FriendlyName = "Nashville Predators" },
                new TwitterAccount { Id = "NHLDevils", FriendlyName = "New Jersey Devils" },
                new TwitterAccount { Id = "NYIslanders", FriendlyName = "New York Islanders" },
                new TwitterAccount { Id = "NYRangers", FriendlyName = "New York Rangers" },
                new TwitterAccount { Id = "Senators", FriendlyName = "Ottawa Senators" },
                new TwitterAccount { Id = "NHLFlyers", FriendlyName = "Philadelphia Flyers" },
                new TwitterAccount { Id = "phoenixcoyotes", FriendlyName = "Phoenix Coyotes" },
                new TwitterAccount { Id = "penguins", FriendlyName = "Pittsburgh Penguins" },
                new TwitterAccount { Id = "SanJoseSharks", FriendlyName = "San Jose Sharks" },
                new TwitterAccount { Id = "StLouisBlues", FriendlyName = "St. Louis Blues" },
                new TwitterAccount { Id = "TBLightning", FriendlyName = "Tampa Bay Lightning" },
                new TwitterAccount { Id = "MapleLeafs", FriendlyName = "Toronto Maple Leafs" },
                new TwitterAccount { Id = "VanCanucks", FriendlyName = "Vancouver Canucks" },
                new TwitterAccount { Id = "washcaps", FriendlyName = "Washington Capitals" },
                new TwitterAccount { Id = "NHLJets", FriendlyName = "Winnipeg Jets" },

                new TwitterAccount { Id = "nhlnw", FriendlyName = "*NHL Northwest" },
                new TwitterAccount { Id = "NHLtoSeattle", FriendlyName = "*NHL to Seattle" },
                new TwitterAccount { Id = "NHL", FriendlyName = "*NHL" }
             );

            context.SaveChanges();

            #endregion

            #region Facebook

            context.FacebookAccountsToFollow.AddOrUpdate(
                account => account.Id,
                new FacebookAccount { Id = "anaheimducks", FriendlyName = "Anaheim Ducks" },
                new FacebookAccount { Id = "NHLBruins", FriendlyName = "Boston Bruins" },
                new FacebookAccount { Id = "BuffaloSabres", FriendlyName = "Bufalo Sabres" },
                new FacebookAccount { Id = "NHLFlames", FriendlyName = "Calgary Flames" },
                new FacebookAccount { Id = "Hurricanes", FriendlyName = "Carolina Hurricanes" },
                new FacebookAccount { Id = "nhlblackhawks", FriendlyName = "Chicago Blackhawks" },
                new FacebookAccount { Id = "coloradoavalanche", FriendlyName = "Colorado Avalanche" },
                new FacebookAccount { Id = "columbusbluejackets", FriendlyName = "Columbus Blue Jackets" },
                new FacebookAccount { Id = "DallasStars", FriendlyName = "Dallas Stars" },
                new FacebookAccount { Id = "DETROITREDWINGS", FriendlyName = "Detroit Red Wings" },
                new FacebookAccount { Id = "Oilers.NHL", FriendlyName = "Edmonton Oilers" },
                new FacebookAccount { Id = "FloridaPanthers", FriendlyName = "Florida Panthers" },
                new FacebookAccount { Id = "LAKings", FriendlyName = "Los Angeles Kings" },
                new FacebookAccount { Id = "minnesotawild", FriendlyName = "Minnesota Wild" },
                new FacebookAccount { Id = "canadiensmtl", FriendlyName = "Montreal Canadians" },
                new FacebookAccount { Id = "nashvillepredators", FriendlyName = "Nashville Predators" },
                new FacebookAccount { Id = "NewJerseyDevils", FriendlyName = "New Jersey Devils" },
                new FacebookAccount { Id = "NEWYORKISLANDERS", FriendlyName = "New York Islanders" },
                new FacebookAccount { Id = "nyrangers", FriendlyName = "New York Rangers" },
                new FacebookAccount { Id = "ottawasenators", FriendlyName = "Ottawa Senators" },
                new FacebookAccount { Id = "philadelphiaflyers", FriendlyName = "Philadelphia Flyers" },
                new FacebookAccount { Id = "thephoenixcoyotes", FriendlyName = "Phoenix Coyotes" },
                new FacebookAccount { Id = "penguins", FriendlyName = "Pittsburgh Penguins" },
                new FacebookAccount { Id = "SanJoseSharks", FriendlyName = "San Jose Sharks" },
                new FacebookAccount { Id = "St.LouisBlues", FriendlyName = "St. Louis Blues" },
                new FacebookAccount { Id = "Lightning.NHL", FriendlyName = "Tampa Bay Lightning" },
                new FacebookAccount { Id = "torontomapleleafs", FriendlyName = "Toronto Maple Leafs" },
                new FacebookAccount { Id = "Canucks", FriendlyName = "Vancouver Canucks" },
                new FacebookAccount { Id = "WashingtonCapitals", FriendlyName = "Washington Capitals" },
                new FacebookAccount { Id = "nhljets", FriendlyName = "Winnipeg Jets" },

                new FacebookAccount { Id = "NHLNW", FriendlyName = "*NHL Northwest" },
                new FacebookAccount { Id = "NHLSeattle", FriendlyName = "*NHL Seattle" },
                new FacebookAccount { Id = "NHLtoSeattle", FriendlyName = "*NHLtoSeattle" },
                new FacebookAccount { Id = "NHL", FriendlyName = "*NHL" }
             );

            context.SaveChanges();

            #endregion

        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<NhlHtmlReportRosterEntryModel>().HasRequired(x => x.NhlHtmlReportRosterModel)
        //            .WithMany(x => x.VisitorRoster);

        //    modelBuilder.Entity<NhlHtmlReportRosterEntryModel>().HasRequired(x => x.NhlHtmlReportRosterModel)
        //        .WithMany(x => x.VisitorRoster)
        //        .HasForeignKey(x => x.NhlHtmlReportRosterModelId);

        //    base.OnModelCreating(modelBuilder);
        //}

    }

    public class SportsDataContextMigrateDatabaseToLatestVersion : MigrateDatabaseToLatestVersion<SportsDataContext, SportsData.Nhl.Query.Migrations.Configuration>
    {
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

