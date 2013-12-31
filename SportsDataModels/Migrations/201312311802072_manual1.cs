namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manual1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FacebookAccounts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FriendlyName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FacebookSnapshots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FacebookAccountId = c.String(maxLength: 128),
                        TotalLikes = c.Int(nullable: false),
                        PeopleTalkingAboutThis = c.Int(nullable: false),
                        MostPopularWeek = c.DateTime(nullable: false),
                        MostPopularCity = c.String(),
                        MostPopularAgeGroup = c.String(),
                        DateOfSnapshot = c.DateTime(nullable: false),
                        Log = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FacebookAccounts", t => t.FacebookAccountId)
                .Index(t => t.FacebookAccountId);
            
            CreateTable(
                "dbo.MlbGameSummary",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Year = c.Int(nullable: false),
                        MlbSeasonType = c.Int(nullable: false),
                        Visitor = c.String(nullable: false),
                        VisitorScore = c.Int(nullable: false),
                        Home = c.String(nullable: false),
                        HomeScore = c.Int(nullable: false),
                        Attendance = c.Int(nullable: false),
                        Innings = c.Int(nullable: false),
                        WinsToDate = c.Int(nullable: false),
                        LossesToDate = c.Int(nullable: false),
                        Postponed = c.Boolean(nullable: false),
                        WPitcher = c.String(),
                        LPitcher = c.String(),
                        SavePitcher = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MlbTeams",
                c => new
                    {
                        ShortNameId = c.Int(nullable: false),
                        ShortName = c.String(),
                        City = c.String(),
                        Name = c.String(),
                        EspnOpponentName = c.String(),
                    })
                .PrimaryKey(t => t.ShortNameId);
            
            CreateTable(
                "dbo.NhlGameSummary",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VisitorScore = c.Int(nullable: false),
                        HomeScore = c.Int(nullable: false),
                        Attendance = c.Int(nullable: false),
                        OS = c.String(),
                        WGoalie = c.String(),
                        WGoal = c.String(),
                        VisitorShots = c.Int(nullable: false),
                        VisitorPPGF = c.Int(nullable: false),
                        VisitorPPOpp = c.Int(nullable: false),
                        VisitorPIM = c.Int(nullable: false),
                        HomeShots = c.Int(nullable: false),
                        HomePPGF = c.Int(nullable: false),
                        HomePPOpp = c.Int(nullable: false),
                        HomePIM = c.Int(nullable: false),
                        NhlSeasonType = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Visitor = c.String(nullable: false),
                        Home = c.String(nullable: false),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NhlHtmlReportRosterModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NhlRtssReportModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NhlHtmlReportRosterEntryModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NhlHtmlReportRosterModel_HomeHeadCoach_Id = c.Int(),
                        NhlHtmlReportRosterModel_HomeRoster_Id = c.Int(),
                        NhlHtmlReportRosterModel_HomeScratches_Id = c.Int(),
                        NhlHtmlReportRosterModel_Linesman_Id = c.Int(),
                        NhlHtmlReportRosterModel_Referees_Id = c.Int(),
                        NhlHtmlReportRosterModel_VisitorHeadCoach_Id = c.Int(),
                        NhlHtmlReportRosterModel_VisitorRoster_Id = c.Int(),
                        NhlHtmlReportRosterModel_VisitorScratches_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_HomeHeadCoach_Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_HomeRoster_Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_HomeScratches_Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_Linesman_Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_Referees_Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_VisitorHeadCoach_Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_VisitorRoster_Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_VisitorScratches_Id)
                .Index(t => t.NhlHtmlReportRosterModel_HomeHeadCoach_Id)
                .Index(t => t.NhlHtmlReportRosterModel_HomeRoster_Id)
                .Index(t => t.NhlHtmlReportRosterModel_HomeScratches_Id)
                .Index(t => t.NhlHtmlReportRosterModel_Linesman_Id)
                .Index(t => t.NhlHtmlReportRosterModel_Referees_Id)
                .Index(t => t.NhlHtmlReportRosterModel_VisitorHeadCoach_Id)
                .Index(t => t.NhlHtmlReportRosterModel_VisitorRoster_Id)
                .Index(t => t.NhlHtmlReportRosterModel_VisitorScratches_Id);
            
            CreateTable(
                "dbo.NhlHtmlReportSummaryModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Visitor = c.String(nullable: false),
                        VisitorScore = c.Int(nullable: false),
                        VisitorGameNumber = c.Int(nullable: false),
                        VisitorAwayGameNumber = c.Int(nullable: false),
                        Home = c.String(nullable: false),
                        HomeScore = c.Int(nullable: false),
                        HomeGameNumber = c.Int(nullable: false),
                        HomeHomeGameNumber = c.Int(nullable: false),
                        Attendance = c.Int(nullable: false),
                        ArenaName = c.String(),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        LeagueGameNumber = c.Int(nullable: false),
                        GameStatus = c.String(),
                        NhlRtssReportModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NhlRtssReport",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameNumber = c.Int(nullable: false),
                        RosterLink = c.String(),
                        GameLink = c.String(),
                        EventsLink = c.String(),
                        FaceOffsLink = c.String(),
                        PlayByPlayLink = c.String(),
                        ShotsLink = c.String(),
                        HomeToiLink = c.String(),
                        VistorToiLink = c.String(),
                        ShootoutLink = c.String(),
                        NhlSeasonType = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Visitor = c.String(nullable: false),
                        Home = c.String(nullable: false),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TwitterAccounts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FriendlyName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TwitterSnapshots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TwitterAccountId = c.String(maxLength: 128),
                        Followers = c.Int(nullable: false),
                        Following = c.Int(nullable: false),
                        Tweets = c.Int(nullable: false),
                        DateOfSnapshot = c.DateTime(nullable: false),
                        Log = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TwitterAccounts", t => t.TwitterAccountId)
                .Index(t => t.TwitterAccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TwitterSnapshots", "TwitterAccountId", "dbo.TwitterAccounts");
            DropForeignKey("dbo.NhlHtmlReportRosterEntryModels", "NhlHtmlReportRosterModel_VisitorScratches_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterEntryModels", "NhlHtmlReportRosterModel_VisitorRoster_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterEntryModels", "NhlHtmlReportRosterModel_VisitorHeadCoach_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterEntryModels", "NhlHtmlReportRosterModel_Referees_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterEntryModels", "NhlHtmlReportRosterModel_Linesman_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterEntryModels", "NhlHtmlReportRosterModel_HomeScratches_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterEntryModels", "NhlHtmlReportRosterModel_HomeRoster_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterEntryModels", "NhlHtmlReportRosterModel_HomeHeadCoach_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.FacebookSnapshots", "FacebookAccountId", "dbo.FacebookAccounts");
            DropIndex("dbo.TwitterSnapshots", new[] { "TwitterAccountId" });
            DropIndex("dbo.NhlHtmlReportRosterEntryModels", new[] { "NhlHtmlReportRosterModel_VisitorScratches_Id" });
            DropIndex("dbo.NhlHtmlReportRosterEntryModels", new[] { "NhlHtmlReportRosterModel_VisitorRoster_Id" });
            DropIndex("dbo.NhlHtmlReportRosterEntryModels", new[] { "NhlHtmlReportRosterModel_VisitorHeadCoach_Id" });
            DropIndex("dbo.NhlHtmlReportRosterEntryModels", new[] { "NhlHtmlReportRosterModel_Referees_Id" });
            DropIndex("dbo.NhlHtmlReportRosterEntryModels", new[] { "NhlHtmlReportRosterModel_Linesman_Id" });
            DropIndex("dbo.NhlHtmlReportRosterEntryModels", new[] { "NhlHtmlReportRosterModel_HomeScratches_Id" });
            DropIndex("dbo.NhlHtmlReportRosterEntryModels", new[] { "NhlHtmlReportRosterModel_HomeRoster_Id" });
            DropIndex("dbo.NhlHtmlReportRosterEntryModels", new[] { "NhlHtmlReportRosterModel_HomeHeadCoach_Id" });
            DropIndex("dbo.FacebookSnapshots", new[] { "FacebookAccountId" });
            DropTable("dbo.TwitterSnapshots");
            DropTable("dbo.TwitterAccounts");
            DropTable("dbo.NhlRtssReport");
            DropTable("dbo.NhlHtmlReportSummaryModels");
            DropTable("dbo.NhlHtmlReportRosterEntryModels");
            DropTable("dbo.NhlHtmlReportRosterModels");
            DropTable("dbo.NhlGameSummary");
            DropTable("dbo.MlbTeams");
            DropTable("dbo.MlbGameSummary");
            DropTable("dbo.FacebookSnapshots");
            DropTable("dbo.FacebookAccounts");
        }
    }
}
