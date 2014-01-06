namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NhlHtmlReportRosterModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NhlRtssReportModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NhlRtssReport", t => t.NhlRtssReportModelId, cascadeDelete: true)
                .Index(t => t.NhlRtssReportModelId);
            
            CreateTable(
                "dbo.NhlHtmlReportRosterParticipantModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParticipantType = c.Int(nullable: false),
                        Designation = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Position = c.String(),
                        StartingLineup = c.Boolean(nullable: false),
                        NhlHtmlReportRosterModel_HomeHeadCoach_Id = c.Int(),
                        NhlHtmlReportRosterModel_HomeRoster_Id = c.Int(),
                        NhlHtmlReportRosterModel_HomeScratches_Id = c.Int(),
                        NhlHtmlReportRosterModel_Linesman_Id = c.Int(),
                        NhlHtmlReportRosterModel_Referees_Id = c.Int(),
                        NhlHtmlReportRosterModel_VisitorHeadCoach_Id = c.Int(),
                        NhlHtmlReportRosterModel_VisitorRoster_Id = c.Int(),
                        NhlHtmlReportRosterModel_VisitorScratches_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_HomeHeadCoach_Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_HomeRoster_Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_HomeScratches_Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_Linesman_Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_Referees_Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_VisitorHeadCoach_Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_VisitorRoster_Id)
                .ForeignKey("dbo.NhlHtmlReportRosterModels", t => t.NhlHtmlReportRosterModel_VisitorScratches_Id)
                .Index(t => t.NhlHtmlReportRosterModel_HomeHeadCoach_Id)
                .Index(t => t.NhlHtmlReportRosterModel_HomeRoster_Id)
                .Index(t => t.NhlHtmlReportRosterModel_HomeScratches_Id)
                .Index(t => t.NhlHtmlReportRosterModel_Linesman_Id)
                .Index(t => t.NhlHtmlReportRosterModel_Referees_Id)
                .Index(t => t.NhlHtmlReportRosterModel_VisitorHeadCoach_Id)
                .Index(t => t.NhlHtmlReportRosterModel_VisitorRoster_Id)
                .Index(t => t.NhlHtmlReportRosterModel_VisitorScratches_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_VisitorScratches_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_VisitorRoster_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_VisitorHeadCoach_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_Referees_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterModels", "NhlRtssReportModelId", "dbo.NhlRtssReport");
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_Linesman_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_HomeScratches_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_HomeRoster_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_HomeHeadCoach_Id", "dbo.NhlHtmlReportRosterModels");
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_VisitorScratches_Id" });
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_VisitorRoster_Id" });
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_VisitorHeadCoach_Id" });
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_Referees_Id" });
            DropIndex("dbo.NhlHtmlReportRosterModels", new[] { "NhlRtssReportModelId" });
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_Linesman_Id" });
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_HomeScratches_Id" });
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_HomeRoster_Id" });
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_HomeHeadCoach_Id" });
            DropTable("dbo.NhlHtmlReportRosterParticipantModels");
            DropTable("dbo.NhlHtmlReportRosterModels");
        }
    }
}
