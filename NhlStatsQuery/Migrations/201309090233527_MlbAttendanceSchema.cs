namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MlbAttendanceSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MlbTeams",
                c => new
                    {
                        ShortNameId = c.Int(nullable: false),
                        ShortName = c.String(),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.ShortNameId);
            
            CreateTable(
                "dbo.MlbGameSummary",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WPitcher = c.String(),
                        LPitcher = c.String(),
                        SavePitcher = c.String(),
                        Innings = c.Int(nullable: false),
                        Season = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        GameType = c.Int(nullable: false),
                        Visitor = c.String(nullable: false),
                        VisitorScore = c.Int(nullable: false),
                        Home = c.String(nullable: false),
                        HomeScore = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NhlGameSummary",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                        Att = c.Int(nullable: false),
                        Season = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        GameType = c.Int(nullable: false),
                        Visitor = c.String(nullable: false),
                        VisitorScore = c.Int(nullable: false),
                        Home = c.String(nullable: false),
                        HomeScore = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.GameSummary");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GameSummary",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Season = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        GameType = c.Int(nullable: false),
                        Visitor = c.String(nullable: false),
                        VisitorScore = c.Int(nullable: false),
                        Home = c.String(nullable: false),
                        HomeScore = c.Int(nullable: false),
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
                        Att = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.NhlGameSummary");
            DropTable("dbo.MlbGameSummary");
            DropTable("dbo.MlbTeams");
        }
    }
}
