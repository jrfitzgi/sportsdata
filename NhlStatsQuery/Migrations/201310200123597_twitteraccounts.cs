namespace SportsData.Nhl.Query.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twitteraccounts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TwitterAccountsToFollow",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FriendlyName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TwitterAccountSnapshots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TwitterAccountId = c.String(maxLength: 128),
                        DateOfSnapshot = c.DateTime(nullable: false),
                        Followers = c.Int(nullable: false),
                        Following = c.Int(nullable: false),
                        Tweets = c.Int(nullable: false),
                        Log = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TwitterAccountsToFollow", t => t.TwitterAccountId)
                .Index(t => t.TwitterAccountId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TwitterAccountSnapshots", new[] { "TwitterAccountId" });
            DropForeignKey("dbo.TwitterAccountSnapshots", "TwitterAccountId", "dbo.TwitterAccountsToFollow");
            DropTable("dbo.TwitterAccountSnapshots");
            DropTable("dbo.TwitterAccountsToFollow");
        }
    }
}
