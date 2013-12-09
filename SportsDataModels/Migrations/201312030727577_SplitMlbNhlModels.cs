namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SplitMlbNhlModels : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.MlbGameSummary");
            DropTable("dbo.NhlGameSummary");

            CreateTable(
                "dbo.MlbGameSummary",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Season = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Visitor = c.String(nullable: false),
                        VisitorScore = c.Int(nullable: false),
                        Home = c.String(nullable: false),
                        HomeScore = c.Int(nullable: false),
                        Attendance = c.Int(nullable: false),
                        MlbSeasonType = c.Int(nullable: false),
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
                "dbo.NhlGameSummary",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Season = c.Int(nullable: false),
                        Visitor = c.String(nullable: false),
                        VisitorScore = c.Int(nullable: false),
                        Home = c.String(nullable: false),
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
                        Date = c.DateTime(nullable: false),
                        NhlSeasonType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            

        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.NhlGameSummary",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NhlSeasonType = c.Int(nullable: false),
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
                        Season = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Visitor = c.String(nullable: false),
                        VisitorScore = c.Int(nullable: false),
                        Home = c.String(nullable: false),
                        HomeScore = c.Int(nullable: false),
                        Attendance = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MlbGameSummary",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MlbSeasonType = c.Int(nullable: false),
                        Innings = c.Int(nullable: false),
                        WinsToDate = c.Int(nullable: false),
                        LossesToDate = c.Int(nullable: false),
                        Postponed = c.Boolean(nullable: false),
                        WPitcher = c.String(),
                        LPitcher = c.String(),
                        SavePitcher = c.String(),
                        Season = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Visitor = c.String(nullable: false),
                        VisitorScore = c.Int(nullable: false),
                        Home = c.String(nullable: false),
                        HomeScore = c.Int(nullable: false),
                        Attendance = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.NhlGameSummary");
            DropTable("dbo.MlbGameSummary");
        }
    }
}
