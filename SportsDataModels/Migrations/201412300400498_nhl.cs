namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nhl : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Nhl_Franchise", "CurrentCity");
            DropColumn("dbo.Nhl_Franchise", "CurrentName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Nhl_Franchise", "CurrentName", c => c.String());
            AddColumn("dbo.Nhl_Franchise", "CurrentCity", c => c.String());
        }
    }
}
