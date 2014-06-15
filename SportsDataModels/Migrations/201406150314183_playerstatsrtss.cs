namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class playerstatsrtss : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NhlPlayerStatsRtssModel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GamesPlayed = c.Int(nullable: false),
                        Hits = c.Int(nullable: false),
                        BlockedShots = c.Int(nullable: false),
                        MissedShots = c.Int(nullable: false),
                        Giveaways = c.Int(nullable: false),
                        Takeaways = c.Int(nullable: false),
                        FaceoffsWon = c.Int(nullable: false),
                        FaceoffsLost = c.Int(nullable: false),
                        FaceoffsTaken = c.Int(nullable: false),
                        FaceoffWinPercentage = c.Double(nullable: false),
                        PercentageOfTeamFaceoffsTaken = c.Double(nullable: false),
                        Shots = c.Int(nullable: false),
                        Goals = c.Int(nullable: false),
                        ShootingPercentage = c.Double(nullable: false),
                        NhlSeasonType = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Position = c.String(nullable: false),
                        Team = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NhlPlayerStatsRtssModel");
        }
    }
}
