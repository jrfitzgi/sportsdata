using System.Data.Entity.Migrations;

using SportsData.Models;

namespace SportsData.Nhl.Query.Migrations
{
    // RenameTable("NhlGameStatsRtssReport", "Nhl_Games_Rtss");

    public class Configuration : DbMigrationsConfiguration<SportsDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SportsDataContext context)
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //{
            //    System.Diagnostics.Debugger.Launch();
            //}

            SportsDataContext.Seed(context);
        }
    }
}
