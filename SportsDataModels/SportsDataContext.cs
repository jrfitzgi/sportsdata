using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Collections.Generic;

namespace SportsData.Models
{
    public class SportsDataContext : DbContext
    {
        public SportsDataContext()
            //: base("DefaultConnection")
            : base("ProdConnection")
        {
        }

        // Mlb
        public DbSet<MlbTeam> MlbTeam_DbSet { get; set; }
        public DbSet<MlbGameSummaryModel> MlbGameSummaryModel_DbSet { get; set; }

        // Nhl
        public DbSet<Nhl_Players_Bio_Skater> Nhl_Players_Bio_Skater_DbSet { get; set; }
        public DbSet<Nhl_Players_Bio_Goalie> Nhl_Players_Bio_Goalie_DbSet { get; set; }
        public DbSet<Nhl_Players_Rtss_Skater> Nhl_Players_Rtss_Skater_DbSet { get; set; }

        public DbSet<Nhl_Games_Summary> Nhl_Games_Summary_DbSet { get; set; }
        public DbSet<Nhl_Games_Rtss> Nhl_Games_Rtss_DbSet { get; set; }
        public DbSet<Nhl_Games_Rtss_Summary> Nhl_Games_Rtss_Summary_DbSet { get; set; }
        public DbSet<Nhl_Games_Rtss_Roster> Nhl_Games_Rtss_Roster_DbSet { get; set; }

        public DbSet<Nhl_Draftbook> Nhl_Draftbook_DbSet { get; set; }

        public DbSet<Nhl_Franchise> Nhl_Franchise_DbSet { get; set; }
        public DbSet<Nhl_Team> Nhl_Team_DbSet { get; set; }

        // Twitter
        public DbSet<TwitterAccount> TwitterAccount_DbSet { get; set; }
        public DbSet<TwitterSnapshot> TwitterSnapshot_DbSet { get; set; }

        // Facebook
        public DbSet<FacebookAccount> FacebookAccount_DbSet { get; set; }
        public DbSet<FacebookSnapshot> FacebookSnapshot_DbSet { get; set; }

        // Demographics
        public DbSet<DemographicsModel> Demographic_DbSet { get; set; }

        public static void Seed(SportsDataContext context)
        {
            #region Mlb

            context.MlbTeam_DbSet.AddOrUpdate(
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

            foreach (MlbTeam mlbTeam in context.MlbTeam_DbSet)
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

            context.TwitterAccount_DbSet.AddOrUpdate(
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

            context.FacebookAccount_DbSet.AddOrUpdate(
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

            #region Nhl

            #region Active Franchises

            Nhl_Franchise anaheimDucks = new Nhl_Franchise
            {
                Id = 1,
                OriginalCity = "Anaheim",
                OriginalName = "Mighty Ducks",
                YearStarted = 1899,
                YearEnded = 0,
                //CurrentCity = "Anaheim",
                //CurrentName = "Ducks",
            };

            Nhl_Franchise arizonaCoyotes = new Nhl_Franchise
            {
                Id = 2,
                OriginalCity = "Phoenix",
                OriginalName = "Coyotes",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise bostonBruins = new Nhl_Franchise
            {
                Id = 3,
                OriginalCity = "Boston",
                OriginalName = "Bruins",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise buffaloSabres = new Nhl_Franchise
            {
                Id = 4,
                OriginalCity = "Buffalo",
                OriginalName = "Sabres",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise calgaryFlames = new Nhl_Franchise
            {
                Id = 5,
                OriginalCity = "Calgary",
                OriginalName = "Flames",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise carolinaHurricanes = new Nhl_Franchise
            {
                Id = 6,
                OriginalCity = "Carolina",
                OriginalName = "Hurricanes",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise chicagoBlackhawks = new Nhl_Franchise
            {
                Id = 7,
                OriginalCity = "Chicago",
                OriginalName = "Blackhawks",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise coloradoAvalanche = new Nhl_Franchise
            {
                Id = 8,
                OriginalCity = "Colorado",
                OriginalName = "Avalanche",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise columbusBlueJackets = new Nhl_Franchise
            {
                Id = 9,
                OriginalCity = "Calgary",
                OriginalName = "Flames",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise dallasStars = new Nhl_Franchise
            {
                Id = 10,
                OriginalCity = "Dallas",
                OriginalName = "Stars",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise detroitRedWings = new Nhl_Franchise
            {
                Id = 11,
                OriginalCity = "Detroit",
                OriginalName = "Red Wings",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise edmontonOilers = new Nhl_Franchise
            {
                Id = 12,
                OriginalCity = "Edmonton",
                OriginalName = "Oilers",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise floridaPanthers = new Nhl_Franchise
            {
                Id = 13,
                OriginalCity = "Florida",
                OriginalName = "Panthers",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise losAngelesKings = new Nhl_Franchise
            {
                Id = 14,
                OriginalCity = "Los Angeles",
                OriginalName = "Kings",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise minnesotaWild = new Nhl_Franchise
            {
                Id = 15,
                OriginalCity = "Minnesota",
                OriginalName = "Wild",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise montrealCanadiens = new Nhl_Franchise
            {
                Id = 16,
                OriginalCity = "Montreal",
                OriginalName = "Canadiens",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise nashvillePredators = new Nhl_Franchise
            {
                Id = 17,
                OriginalCity = "Nashville",
                OriginalName = "Predators",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise newJerseyDevils = new Nhl_Franchise
            {
                Id = 18,
                OriginalCity = "New Jersey",
                OriginalName = "Devils",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise newYorkIslanders = new Nhl_Franchise
            {
                Id = 19,
                OriginalCity = "New York",
                OriginalName = "Islanders",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise newYorkRangers = new Nhl_Franchise
            {
                Id = 20,
                OriginalCity = "New York",
                OriginalName = "Rangers",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise ottawaSenators = new Nhl_Franchise
            {
                Id = 21,
                OriginalCity = "Ottawa",
                OriginalName = "Senators",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise philadelphiaFlyers = new Nhl_Franchise
            {
                Id = 22,
                OriginalCity = "Philadelphia",
                OriginalName = "Flyers",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise pittsburghPenguins = new Nhl_Franchise
            {
                Id = 23,
                OriginalCity = "Pittsburgh",
                OriginalName = "Penguins",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise sanJoseSharks = new Nhl_Franchise
            {
                Id = 24,
                OriginalCity = "San Jose",
                OriginalName = "Sharks",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise stLouisBlues = new Nhl_Franchise
            {
                Id = 25,
                OriginalCity = "St. Louis",
                OriginalName = "Blues",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise tampaBayLightning = new Nhl_Franchise
            {
                Id = 26,
                OriginalCity = "Tampa Bay",
                OriginalName = "Lightning",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise torontoMapleLeafs = new Nhl_Franchise
            {
                Id = 27,
                OriginalCity = "Toronto",
                OriginalName = "Maple Leafs",
                YearStarted = 1918,
                YearEnded = 0,
            };

            Nhl_Franchise vancouverCanucks = new Nhl_Franchise
            {
                Id = 28,
                OriginalCity = "Vancouver",
                OriginalName = "Canucks",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise washingtonCapitals = new Nhl_Franchise
            {
                Id = 29,
                OriginalCity = "Washington",
                OriginalName = "Capitals",
                YearStarted = 1899,
                YearEnded = 0,
            };

            Nhl_Franchise winnipegJets = new Nhl_Franchise
            {
                Id = 30,
                OriginalCity = "Winnipeg",
                OriginalName = "Jets",
                YearStarted = 2001,
                YearEnded = 0,
            };

            #endregion

            #region Inactive Franchises

            #endregion

            context.Nhl_Franchise_DbSet.AddOrUpdate(
                t => t.Id,
                anaheimDucks,
                arizonaCoyotes,
                bostonBruins,
                buffaloSabres,
                calgaryFlames,
                carolinaHurricanes,
                chicagoBlackhawks,
                coloradoAvalanche,
                columbusBlueJackets,
                dallasStars,
                detroitRedWings,
                edmontonOilers,
                floridaPanthers,
                losAngelesKings,
                minnesotaWild,
                montrealCanadiens,
                nashvillePredators,
                newJerseyDevils,
                newYorkIslanders,
                newYorkRangers,
                ottawaSenators,
                philadelphiaFlyers,
                pittsburghPenguins,
                sanJoseSharks,
                stLouisBlues,
                tampaBayLightning,
                torontoMapleLeafs,
                vancouverCanucks,
                washingtonCapitals,
                winnipegJets
            );


            context.Nhl_Team_DbSet.AddOrUpdate(
                t => t.Id,

            #region Active Teams

            new Nhl_Team
            {
                Id = 1,
                City = "Anaheim",
                MajorCity = "Anaheim",
                Name = "Ducks",
                Abbreviation = "ANA",
                Nhl_Game_Summary_Name = "Anaheim",
                StateProvince = "California",
                Country = "United States",
                YearStarted = 2007,
                YearEnded = 0,
                Nhl_FranchiseId = anaheimDucks.Id
            },

            new Nhl_Team
            {
                Id = 2,
                City = "Arizona",
                MajorCity = "Phoenix",
                Name = "Coyotes",
                Abbreviation = "ARI",
                Nhl_Game_Summary_Name = "Arizona",
                StateProvince = "Arizona",
                Country = "United States",
                YearStarted = 2014,
                YearEnded = 0,
                Nhl_FranchiseId = arizonaCoyotes.Id
            },

            new Nhl_Team
            {
                Id = 3,
                City = "Boston",
                MajorCity = "Boston",
                Name = "Bruins",
                Abbreviation = "BOS",
                Nhl_Game_Summary_Name = "Boston",
                StateProvince = "Massachusetts",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = bostonBruins.Id
            },

            new Nhl_Team
            {
                Id = 4,
                City = "Buffalo",
                MajorCity = "Buffalo",
                Name = "Sabres",
                Abbreviation = "BUF",
                Nhl_Game_Summary_Name = "Buffalo",
                StateProvince = "New York",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = buffaloSabres.Id
            },

            new Nhl_Team
            {
                Id = 5,
                City = "Calgary",
                MajorCity = "Calgary",
                Name = "Flames",
                Abbreviation = "CGY",
                Nhl_Game_Summary_Name = "Calgary",
                StateProvince = "Alberta",
                Country = "Canada",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = calgaryFlames.Id
            },

            new Nhl_Team
            {
                Id = 6,
                City = "Carolina",
                MajorCity = "Raleigh",
                Name = "Hurricanes",
                Abbreviation = "CAR",
                Nhl_Game_Summary_Name = "Carolina",
                StateProvince = "North Carolina",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = carolinaHurricanes.Id
            },

            new Nhl_Team
            {
                Id = 7,
                City = "Chicago",
                MajorCity = "Chicago",
                Name = "Blackhawks",
                Abbreviation = "CHI",
                Nhl_Game_Summary_Name = "Chicago",
                StateProvince = "Illinois",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = chicagoBlackhawks.Id
            },

            new Nhl_Team
            {
                Id = 8,
                City = "Colorado",
                MajorCity = "Denver",
                Name = "Avalanche",
                Abbreviation = "COL",
                Nhl_Game_Summary_Name = "Colorado",
                StateProvince = "Colorado",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = coloradoAvalanche.Id
            },

            new Nhl_Team
            {
                Id = 9,
                City = "Columbus",
                MajorCity = "Columbus",
                Name = "Blue Jackets",
                Abbreviation = "CBJ",
                Nhl_Game_Summary_Name = "Columbus",
                StateProvince = "Ohio",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = columbusBlueJackets.Id
            },

            new Nhl_Team
            {
                Id = 10,
                City = "Dallas",
                MajorCity = "Dallas",
                Name = "Stars",
                Abbreviation = "DAL",
                Nhl_Game_Summary_Name = "Dallas",
                StateProvince = "Texas",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = dallasStars.Id
            },

            new Nhl_Team
            {
                Id = 11,
                City = "Detroit",
                MajorCity = "Detroit",
                Name = "Red Wings",
                Abbreviation = "DET",
                Nhl_Game_Summary_Name = "Detroit",
                StateProvince = "Michigan",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = detroitRedWings.Id
            },

            new Nhl_Team
            {
                Id = 12,
                City = "Edmonton",
                MajorCity = "Edmonton",
                Name = "Oilers",
                Abbreviation = "EDM",
                Nhl_Game_Summary_Name = "Edmonton",
                StateProvince = "Alberta",
                Country = "Canada",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = edmontonOilers.Id
            },

            new Nhl_Team
            {
                Id = 13,
                City = "Florida",
                MajorCity = "Sunrise",
                Name = "Panthers",
                Abbreviation = "FLA",
                Nhl_Game_Summary_Name = "Florida",
                StateProvince = "Florida",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = floridaPanthers.Id
            },

            new Nhl_Team
            {
                Id = 14,
                City = "Los Angeles",
                MajorCity = "Los Angeles",
                Name = "Kings",
                Abbreviation = "LAK",
                Nhl_Game_Summary_Name = "Los Angeles",
                StateProvince = "California",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = losAngelesKings.Id
            },

            new Nhl_Team
            {
                Id = 15,
                City = "Minnesota",
                MajorCity = "Minneapolis",
                Name = "Wild",
                Abbreviation = "MIN",
                Nhl_Game_Summary_Name = "Minnesota",
                StateProvince = "Minnesota",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = minnesotaWild.Id
            },

            new Nhl_Team
            {
                Id = 16,
                City = "Montreal",
                MajorCity = "Montreal",
                Name = "Canadiens",
                Abbreviation = "MTL",
                Nhl_Game_Summary_Name = "Montreal",
                StateProvince = "Quebec",
                Country = "Canada",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = montrealCanadiens.Id
            },

            new Nhl_Team
            {
                Id = 17,
                City = "Nashville",
                MajorCity = "Nashville",
                Name = "Predators",
                Abbreviation = "NSH",
                Nhl_Game_Summary_Name = "Nashville",
                StateProvince = "Tennessee",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = nashvillePredators.Id
            },

            new Nhl_Team
            {
                Id = 18,
                City = "New Jersey",
                MajorCity = "Newark",
                Name = "Devils",
                Abbreviation = "NJD",
                Nhl_Game_Summary_Name = "New Jersey",
                StateProvince = "New Jersey",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = newJerseyDevils.Id
            },

            new Nhl_Team
            {
                Id = 19,
                City = "New York",
                MajorCity = "Long Island",
                Name = "Islanders",
                Abbreviation = "NYI",
                Nhl_Game_Summary_Name = "NY Islanders",
                StateProvince = "New York",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = newYorkIslanders.Id
            },

            new Nhl_Team
            {
                Id = 20,
                City = "New York",
                MajorCity = "New York",
                Name = "Rangers",
                Abbreviation = "NYR",
                Nhl_Game_Summary_Name = "NY Rangers",
                StateProvince = "New York",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = newYorkRangers.Id
            },

            new Nhl_Team
            {
                Id = 21,
                City = "Ottawa",
                MajorCity = "Ottawa",
                Name = "Senators",
                Abbreviation = "OTT",
                Nhl_Game_Summary_Name = "Ottawa",
                StateProvince = "Ontario",
                Country = "Canada",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = ottawaSenators.Id
            },

            new Nhl_Team
            {
                Id = 22,
                City = "Philadelphia",
                MajorCity = "Philadelphia",
                Name = "Flyers",
                Abbreviation = "PHI",
                Nhl_Game_Summary_Name = "Philadelphia",
                StateProvince = "Pennsylvania",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = philadelphiaFlyers.Id
            },

            new Nhl_Team
            {
                Id = 23,
                City = "Pittsburgh",
                MajorCity = "Pittsburgh",
                Name = "Penguins",
                Abbreviation = "PIT",
                Nhl_Game_Summary_Name = "Pittsburgh",
                StateProvince = "Pennsylvania",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = pittsburghPenguins.Id
            },

            new Nhl_Team
            {
                Id = 24,
                City = "San Jose",
                MajorCity = "San Jose",
                Name = "Sharks",
                Abbreviation = "SJS",
                Nhl_Game_Summary_Name = "San Jose",
                StateProvince = "California",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = sanJoseSharks.Id
            },

            new Nhl_Team
            {
                Id = 25,
                City = "St. Louis",
                MajorCity = "St. Louis",
                Name = "Blues",
                Abbreviation = "STL",
                Nhl_Game_Summary_Name = "St Louis",
                StateProvince = "Missouri",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = stLouisBlues.Id
            },

            new Nhl_Team
            {
                Id = 26,
                City = "Tampa Bay",
                MajorCity = "Tampa Bay",
                Name = "Lightning",
                Abbreviation = "TBL",
                Nhl_Game_Summary_Name = "Tampa Bay",
                StateProvince = "Florida",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = tampaBayLightning.Id
            },

            new Nhl_Team
            {
                Id = 27,
                City = "Toronto",
                MajorCity = "Toronto",
                Name = "Maple Leafs",
                Abbreviation = "TOR",
                Nhl_Game_Summary_Name = "Toronto",
                StateProvince = "Ontario",
                Country = "Canada",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = torontoMapleLeafs.Id
            },

            new Nhl_Team
            {
                Id = 28,
                City = "Vancouver",
                MajorCity = "Vancouver",
                Name = "Canucks",
                Abbreviation = "VAN",
                Nhl_Game_Summary_Name = "Vancouver",
                StateProvince = "British Columbia",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = vancouverCanucks.Id
            },

            new Nhl_Team
            {
                Id = 29,
                City = "Washington",
                MajorCity = "Washington",
                Name = "Capitals",
                Abbreviation = "WSH",
                Nhl_Game_Summary_Name = "Washington",
                StateProvince = "DC",
                Country = "United States",
                YearStarted = 1950,
                YearEnded = 0,
                Nhl_FranchiseId = washingtonCapitals.Id
            },

            new Nhl_Team
            {
                Id = 30,
                City = "Winnipeg",
                MajorCity = "Winnipeg",
                Name = "Jets",
                Abbreviation = "WPG",
                Nhl_Game_Summary_Name = "Winnipeg",
                StateProvince = "Manitoba",
                Country = "Canada",
                YearStarted = 2012,
                YearEnded = 0,
                Nhl_FranchiseId = winnipegJets.Id
            },

            #endregion

            #region Inactive Teams

            new Nhl_Team
            {
                Id = 33,
                City = "Anaheim",
                MajorCity = "Anaheim",
                Name = "Mighty Ducks",
                Abbreviation = "ANA",
                Nhl_Game_Summary_Name = "Anaheim",
                StateProvince = "California",
                Country = "United States",
                YearStarted = 1994,
                YearEnded = 2006,
                Nhl_FranchiseId = anaheimDucks.Id
            },

            new Nhl_Team
            {
                Id = 31,
                City = "Atlanta",
                MajorCity = "Atlanta",
                Name = "Thrashers",
                Abbreviation = "ATL",
                Nhl_Game_Summary_Name = "Atlanta",
                StateProvince = "Georgia",
                Country = "United States",
                YearStarted = 2001,
                YearEnded = 2011,
                Nhl_FranchiseId = winnipegJets.Id
            },

            new Nhl_Team
            {
                Id = 32,
                City = "Phoenix",
                MajorCity = "Phoenix",
                Name = "Coyotes",
                Abbreviation = "PHX",
                Nhl_Game_Summary_Name = "Phoenix",
                StateProvince = "Arizona",
                Country = "United States",
                YearStarted = 1997,
                YearEnded = 2013,
                Nhl_FranchiseId = arizonaCoyotes.Id
            }

            #endregion

            );

            #endregion
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Nhl_Team>()
            //    .HasRequired<Nhl_Franchise>(t => t.Nhl_Franchise)
            //    .WithMany(f => f.Teams)
            //    .HasForeignKey(t => t.Nhl_FranchiseId)
            //    .WillCascadeOnDelete(true);
        }

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

