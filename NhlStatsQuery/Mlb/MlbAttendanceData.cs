using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace SportsData.Mlb
{
    public class MlbAttendanceData
    {
        public static List<MlbGameSummary> UpdateSeason(MlbSeasonType mlbSeasonType, int seasonYear)
        {
            // Get latest results
            List<MlbGameSummary> gamesToAdd = MlbAttendanceQuery.GetSeason(mlbSeasonType, seasonYear);

            // Remove existing results from DB and save new ones
            using (SportsDataContext db = new SportsDataContext())
            {
                IEnumerable<MlbGameSummary> gamesToRemove = from g in db.MlbGameSummaries
                                                            where g.MlbSeasonType == mlbSeasonType && g.Season == seasonYear
                                                            select g;

                foreach (MlbGameSummary gameToRemove in gamesToRemove)
                {
                    db.MlbGameSummaries.Remove(gameToRemove);
                }

                foreach (MlbGameSummary gameToAdd in gamesToAdd)
                {
                    db.MlbGameSummaries.Add(gameToAdd);
                }

                db.SaveChanges();
            }

            return gamesToAdd;
        }

        public static List<MlbGameSummary> UpdateSeasonForTeam(MlbSeasonType mlbSeasonType, MlbTeamShortName mlbTeam, int seasonYear)
        {
            // Get latest results
            List<MlbGameSummary> gamesToAdd = MlbAttendanceQuery.GetSeasonForTeam(mlbSeasonType, mlbTeam, seasonYear);

            // Remove existing results from DB and save new ones
            using (SportsDataContext db = new SportsDataContext())
            {
                string mlbTeamString = mlbTeam.ToString(); // http://stackoverflow.com/questions/5899683/linq-to-entities-does-not-recognize-the-method-system-string-tostring-method
                var gamesToRemove = from g in db.MlbGameSummaries
                                    where g.MlbSeasonType == mlbSeasonType &&
                                          g.Season == seasonYear &&
                                          g.Home.Equals(mlbTeamString, StringComparison.InvariantCultureIgnoreCase)
                                    select g;

                foreach (MlbGameSummary gameToRemove in gamesToRemove)
                {
                    db.MlbGameSummaries.Remove(gameToRemove);
                }

                if (null != gamesToAdd)
                {
                    foreach (MlbGameSummary gameToAdd in gamesToAdd)
                    {
                        db.MlbGameSummaries.Add(gameToAdd);
                    }
                }

                db.SaveChanges();
            }

            return gamesToAdd;
        }
    }
}
