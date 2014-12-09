namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nhl1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Nhl_Games_Rtss_Summary_PowerPlaySummary_Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PowerPlay5v4Goals = c.Int(nullable: false),
                        PowerPlay5v4Occurrences = c.Int(nullable: false),
                        PowerPlay5v4ToiSeconds = c.Int(nullable: false),
                        PowerPlay5v3Goals = c.Int(nullable: false),
                        PowerPlay5v3Occurrences = c.Int(nullable: false),
                        PowerPlay5v3ToiSeconds = c.Int(nullable: false),
                        PowerPlay4v3Goals = c.Int(nullable: false),
                        PowerPlay4v3Occurrences = c.Int(nullable: false),
                        PowerPlay4v3ToiSeconds = c.Int(nullable: false),
                        EvenStrength5v5Goals = c.Int(nullable: false),
                        EvenStrength5v5Occurrences = c.Int(nullable: false),
                        EvenStrength5v5ToiSeconds = c.Int(nullable: false),
                        EvenStrength4v4Goals = c.Int(nullable: false),
                        EvenStrength4v4Occurrences = c.Int(nullable: false),
                        EvenStrength4v4ToiSeconds = c.Int(nullable: false),
                        EvenStrength3v3Goals = c.Int(nullable: false),
                        EvenStrength3v3Occurrences = c.Int(nullable: false),
                        EvenStrength3v3ToiSeconds = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlaySummary_Home_Id", c => c.Int());
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlaySummary_Visitor_Id", c => c.Int());
            AddColumn("dbo.Nhl_Games_Rtss_Summary_GoalieSummary_Item", "WinOrLoss", c => c.String());
            AddColumn("dbo.Nhl_Games_Rtss_Summary_PeriodSummary_Item", "Penalties", c => c.Int(nullable: false));
            AlterColumn("dbo.Nhl_Games_Rtss_Summary_Stars_Item", "Team", c => c.String());
            CreateIndex("dbo.Nhl_Games_Rtss_Summary", "PowerPlaySummary_Home_Id");
            CreateIndex("dbo.Nhl_Games_Rtss_Summary", "PowerPlaySummary_Visitor_Id");
            AddForeignKey("dbo.Nhl_Games_Rtss_Summary", "PowerPlaySummary_Home_Id", "dbo.Nhl_Games_Rtss_Summary_PowerPlaySummary_Item", "Id");
            AddForeignKey("dbo.Nhl_Games_Rtss_Summary", "PowerPlaySummary_Visitor_Id", "dbo.Nhl_Games_Rtss_Summary_PowerPlaySummary_Item", "Id");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay5v4Goals");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay5v4Occurrences");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay5v4ToiSeconds");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay5v3Goals");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay5v3Occurrences");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay5v3ToiSeconds");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay4v3Goals");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay4v3Occurrences");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay4v3ToiSeconds");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength5v5Goals");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength5v5Occurrences");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength5v5ToiSeconds");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength4v4Goals");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength4v4Occurrences");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength4v4ToiSeconds");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength3v3Goals");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength3v3Occurrences");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength3v3ToiSeconds");
            DropColumn("dbo.Nhl_Games_Rtss_Summary_PeriodSummary_Item", "PN");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Nhl_Games_Rtss_Summary_PeriodSummary_Item", "PN", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength3v3ToiSeconds", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength3v3Occurrences", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength3v3Goals", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength4v4ToiSeconds", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength4v4Occurrences", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength4v4Goals", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength5v5ToiSeconds", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength5v5Occurrences", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "EvenStrength5v5Goals", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay4v3ToiSeconds", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay4v3Occurrences", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay4v3Goals", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay5v3ToiSeconds", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay5v3Occurrences", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay5v3Goals", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay5v4ToiSeconds", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay5v4Occurrences", c => c.Int(nullable: false));
            AddColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlay5v4Goals", c => c.Int(nullable: false));
            DropForeignKey("dbo.Nhl_Games_Rtss_Summary", "PowerPlaySummary_Visitor_Id", "dbo.Nhl_Games_Rtss_Summary_PowerPlaySummary_Item");
            DropForeignKey("dbo.Nhl_Games_Rtss_Summary", "PowerPlaySummary_Home_Id", "dbo.Nhl_Games_Rtss_Summary_PowerPlaySummary_Item");
            DropIndex("dbo.Nhl_Games_Rtss_Summary", new[] { "PowerPlaySummary_Visitor_Id" });
            DropIndex("dbo.Nhl_Games_Rtss_Summary", new[] { "PowerPlaySummary_Home_Id" });
            AlterColumn("dbo.Nhl_Games_Rtss_Summary_Stars_Item", "Team", c => c.Int(nullable: false));
            DropColumn("dbo.Nhl_Games_Rtss_Summary_PeriodSummary_Item", "Penalties");
            DropColumn("dbo.Nhl_Games_Rtss_Summary_GoalieSummary_Item", "WinOrLoss");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlaySummary_Visitor_Id");
            DropColumn("dbo.Nhl_Games_Rtss_Summary", "PowerPlaySummary_Home_Id");
            DropTable("dbo.Nhl_Games_Rtss_Summary_PowerPlaySummary_Item");
        }
    }
}
