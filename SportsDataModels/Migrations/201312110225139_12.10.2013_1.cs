namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12102013_1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NhlRtssReport",
                c => new
                    {
                        Date = c.DateTime(nullable: false),
                        Visitor = c.String(nullable: false, maxLength: 128),
                        Home = c.String(nullable: false, maxLength: 128),
                        GameNumber = c.Int(nullable: false),
                        RosterLink = c.String(),
                        GameLink = c.String(),
                        EventsLink = c.String(),
                        FaceOffsLink = c.String(),
                        PlayByPlayLink = c.String(),
                        ShotsLink = c.String(),
                        HomeToiLink = c.String(),
                        VistorToiLink = c.String(),
                        ShootoutLink = c.String(),
                        NhlSeasonType = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Date, t.Visitor, t.Home });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NhlRtssReport");
        }
    }
}
