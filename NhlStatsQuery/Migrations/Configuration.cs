using System.Data.Entity.Migrations;

namespace SportsData.Nhl.Query.Migrations
{
    public class Configuration : DbMigrationsConfiguration<SportsDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SportsDataContext context)
        {
            if (System.Diagnostics.Debugger.IsAttached == false)
            {
                System.Diagnostics.Debugger.Launch();
            }

            SportsDataContext.Seed(context);
        }
    }
}
