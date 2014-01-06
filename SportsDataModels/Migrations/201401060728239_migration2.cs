namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_HomeHeadCoach_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_HomeRoster_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_HomeScratches_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_Linesman_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterModels", "NhlRtssReportModelId", "dbo.NhlRtssReport");
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_Referees_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_VisitorHeadCoach_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_VisitorRoster_Id", "dbo.NhlHtmlReportRosterModels");
            DropForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_VisitorScratches_Id", "dbo.NhlHtmlReportRosterModels");
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_HomeHeadCoach_Id" });
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_HomeRoster_Id" });
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_HomeScratches_Id" });
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_Linesman_Id" });
            DropIndex("dbo.NhlHtmlReportRosterModels", new[] { "NhlRtssReportModelId" });
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_Referees_Id" });
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_VisitorHeadCoach_Id" });
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_VisitorRoster_Id" });
            DropIndex("dbo.NhlHtmlReportRosterParticipantModels", new[] { "NhlHtmlReportRosterModel_VisitorScratches_Id" });
            DropTable("dbo.NhlHtmlReportRosterModels");
            DropTable("dbo.NhlHtmlReportRosterParticipantModels");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NhlHtmlReportRosterModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NhlRtssReportModelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_VisitorScratches_Id");
            CreateIndex("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_VisitorRoster_Id");
            CreateIndex("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_VisitorHeadCoach_Id");
            CreateIndex("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_Referees_Id");
            CreateIndex("dbo.NhlHtmlReportRosterModels", "NhlRtssReportModelId");
            CreateIndex("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_Linesman_Id");
            CreateIndex("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_HomeScratches_Id");
            CreateIndex("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_HomeRoster_Id");
            CreateIndex("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_HomeHeadCoach_Id");
            AddForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_VisitorScratches_Id", "dbo.NhlHtmlReportRosterModels", "Id");
            AddForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_VisitorRoster_Id", "dbo.NhlHtmlReportRosterModels", "Id");
            AddForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_VisitorHeadCoach_Id", "dbo.NhlHtmlReportRosterModels", "Id");
            AddForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_Referees_Id", "dbo.NhlHtmlReportRosterModels", "Id");
            AddForeignKey("dbo.NhlHtmlReportRosterModels", "NhlRtssReportModelId", "dbo.NhlRtssReport", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_Linesman_Id", "dbo.NhlHtmlReportRosterModels", "Id");
            AddForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_HomeScratches_Id", "dbo.NhlHtmlReportRosterModels", "Id");
            AddForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_HomeRoster_Id", "dbo.NhlHtmlReportRosterModels", "Id");
            AddForeignKey("dbo.NhlHtmlReportRosterParticipantModels", "NhlHtmlReportRosterModel_HomeHeadCoach_Id", "dbo.NhlHtmlReportRosterModels", "Id");
        }
    }
}
