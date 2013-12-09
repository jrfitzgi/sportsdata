namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SplitMlbNhlModels3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.MlbGameSummary2", newName: "MlbGameSummary");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.MlbGameSummary", newName: "MlbGameSummary2");
        }
    }
}
