using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Migrations;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SportsData.Nhl;
using SportsData.Nhl.Query;
using SportsData.Mlb;


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
            context.MlbTeams.AddOrUpdate(
                t => t.ShortName,
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = "Arizona Cardinals"},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""},
                new MlbTeam { ShortName = MlbTeamShortName.ARI, LongName = ""}
                );


        }
    }
}
