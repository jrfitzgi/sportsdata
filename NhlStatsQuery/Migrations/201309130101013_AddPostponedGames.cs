namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostponedGames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MlbGameSummary", "Postponed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MlbGameSummary", "Postponed");
        }
    }
}
