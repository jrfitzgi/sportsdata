namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MlbTeamsUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MlbTeams", "City", c => c.String());
            AddColumn("dbo.MlbTeams", "Name", c => c.String());
            DropColumn("dbo.MlbTeams", "FullName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MlbTeams", "FullName", c => c.String());
            DropColumn("dbo.MlbTeams", "Name");
            DropColumn("dbo.MlbTeams", "City");
        }
    }
}
