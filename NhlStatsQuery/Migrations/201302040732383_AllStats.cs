namespace NhlStatsQuery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllStats : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameSummary", "Home", c => c.String(nullable: false));
            AddColumn("dbo.GameSummary", "HomeScore", c => c.Int(nullable: false));
            AddColumn("dbo.GameSummary", "Visitor", c => c.String(nullable: false));
            AddColumn("dbo.GameSummary", "VisitorScore", c => c.Int(nullable: false));
            AddColumn("dbo.GameSummary", "OS", c => c.String(nullable: false));
            AddColumn("dbo.GameSummary", "WGoalie", c => c.String(nullable: false));
            AddColumn("dbo.GameSummary", "WGoal", c => c.String(nullable: false));
            AddColumn("dbo.GameSummary", "VisitorShots", c => c.Int(nullable: false));
            AddColumn("dbo.GameSummary", "VisitorPPGF", c => c.Int(nullable: false));
            AddColumn("dbo.GameSummary", "VisitorPPOpp", c => c.Int(nullable: false));
            AddColumn("dbo.GameSummary", "VisitorPIM", c => c.Int(nullable: false));
            AddColumn("dbo.GameSummary", "HomeShots", c => c.Int(nullable: false));
            AddColumn("dbo.GameSummary", "HomePPGF", c => c.Int(nullable: false));
            AddColumn("dbo.GameSummary", "HomePPOpp", c => c.Int(nullable: false));
            AddColumn("dbo.GameSummary", "HomePIM", c => c.Int(nullable: false));
            AddColumn("dbo.GameSummary", "Att", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GameSummary", "Att");
            DropColumn("dbo.GameSummary", "HomePIM");
            DropColumn("dbo.GameSummary", "HomePPOpp");
            DropColumn("dbo.GameSummary", "HomePPGF");
            DropColumn("dbo.GameSummary", "HomeShots");
            DropColumn("dbo.GameSummary", "VisitorPIM");
            DropColumn("dbo.GameSummary", "VisitorPPOpp");
            DropColumn("dbo.GameSummary", "VisitorPPGF");
            DropColumn("dbo.GameSummary", "VisitorShots");
            DropColumn("dbo.GameSummary", "WGoal");
            DropColumn("dbo.GameSummary", "WGoalie");
            DropColumn("dbo.GameSummary", "OS");
            DropColumn("dbo.GameSummary", "VisitorScore");
            DropColumn("dbo.GameSummary", "Visitor");
            DropColumn("dbo.GameSummary", "HomeScore");
            DropColumn("dbo.GameSummary", "Home");
        }
    }
}
