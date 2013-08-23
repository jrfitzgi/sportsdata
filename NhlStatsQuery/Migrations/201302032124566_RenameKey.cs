namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.GameSummary", new[] { "OldId" });
            DropColumn("dbo.GameSummary", "OldId");
            AddColumn("dbo.GameSummary", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.GameSummary", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GameSummary", "DateHomeTeam", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.GameSummary", new[] { "Id" });
            AddPrimaryKey("dbo.GameSummary", "DateHomeTeam");
            DropColumn("dbo.GameSummary", "Id");
        }
    }
}
