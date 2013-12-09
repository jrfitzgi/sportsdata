namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SplitMlbNhlModels4 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.NhlGameSummary2", newName: "NhlGameSummary");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.NhlGameSummary", newName: "NhlGameSummary2");
        }
    }
}
