namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyMlbModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MlbGameSummary", "WinsToDate", c => c.Int(nullable: false));
            AddColumn("dbo.MlbGameSummary", "LossesToDate", c => c.Int(nullable: false));
            AddColumn("dbo.MlbGameSummary", "Attendance", c => c.Int(nullable: false));
            AddColumn("dbo.NhlGameSummary", "Attendance", c => c.Int(nullable: false));
            DropColumn("dbo.NhlGameSummary", "Att");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NhlGameSummary", "Att", c => c.Int(nullable: false));
            DropColumn("dbo.NhlGameSummary", "Attendance");
            DropColumn("dbo.MlbGameSummary", "Attendance");
            DropColumn("dbo.MlbGameSummary", "LossesToDate");
            DropColumn("dbo.MlbGameSummary", "WinsToDate");
        }
    }
}
