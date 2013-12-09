namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SplitMlbNhlModels2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.MlbGameSummary", newName: "MlbGameSummary2");
            RenameTable(name: "dbo.NhlGameSummary", newName: "NhlGameSummary2");
            AddColumn("dbo.MlbGameSummary2", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.NhlGameSummary2", "Year", c => c.Int(nullable: false));
            DropColumn("dbo.MlbGameSummary2", "Season");
            DropColumn("dbo.NhlGameSummary2", "Season");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NhlGameSummary2", "Season", c => c.Int(nullable: false));
            AddColumn("dbo.MlbGameSummary2", "Season", c => c.Int(nullable: false));
            DropColumn("dbo.NhlGameSummary2", "Year");
            DropColumn("dbo.MlbGameSummary2", "Year");
            RenameTable(name: "dbo.NhlGameSummary2", newName: "NhlGameSummary");
            RenameTable(name: "dbo.MlbGameSummary2", newName: "MlbGameSummary");
        }
    }
}
