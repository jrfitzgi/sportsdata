namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeIdToDateHomeTeam : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.GameSummary", new[] { "Id" });
            DropColumn("dbo.GameSummary", "Id");
            AddColumn("dbo.GameSummary", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.GameSummary", "Id");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GameSummary", "Id", c => c.Int(nullable: false, identity: true));
        }
    }
}
