namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makedateakeyfornhl : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.NhlGameSummary");
            AddPrimaryKey("dbo.NhlGameSummary", new[] { "Date", "Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.NhlGameSummary");
            AddPrimaryKey("dbo.NhlGameSummary", "Id");
        }
    }
}
