namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SportsDataModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RawDataTables",
                c => new
                    {
                        SourceUrl = c.String(nullable: false, maxLength: 128),
                        RawDataType = c.Int(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        RawData = c.String(),
                        Log = c.String(),
                    })
                .PrimaryKey(t => t.SourceUrl);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RawDataTables");
        }
    }
}
