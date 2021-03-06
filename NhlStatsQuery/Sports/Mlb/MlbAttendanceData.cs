﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;
using SportsData.Models;

namespace SportsData.Mlb
{
    public class MlbAttendanceData
    {
        public static List<MlbGameSummaryModel> UpdateSeason(MlbSeasonType mlbSeasonType, int seasonYear)
        {
            // Get latest results
            List<MlbGameSummaryModel> gamesToAdd = MlbAttendanceQuery.GetSeason(mlbSeasonType, seasonYear);

            // Remove existing results from DB and save new ones
            using (SportsDataContext db = new SportsDataContext())
            {
                IEnumerable<MlbGameSummaryModel> gamesToRemove = from g in db.MlbGameSummaryModel_DbSet
                                                            where g.MlbSeasonType == mlbSeasonType && g.Year == seasonYear
                                                            select g;

                foreach (MlbGameSummaryModel gameToRemove in gamesToRemove)
                {
                    db.MlbGameSummaryModel_DbSet.Remove(gameToRemove);
                }

                foreach (MlbGameSummaryModel gameToAdd in gamesToAdd)
                {
                    db.MlbGameSummaryModel_DbSet.Add(gameToAdd);
                }

                db.SaveChanges();
            }

            return gamesToAdd;
        }

        public static List<MlbGameSummaryModel> UpdateSeasonForTeam(MlbSeasonType mlbSeasonType, MlbTeamShortName mlbTeam, int seasonYear)
        {
            // Get latest results
            List<MlbGameSummaryModel> gamesToAdd = MlbAttendanceQuery.GetSeasonForTeam(mlbSeasonType, mlbTeam, seasonYear);

            // Remove existing results from DB and save new ones
            using (SportsDataContext db = new SportsDataContext())
            {
                string mlbTeamString = mlbTeam.ToString(); // http://stackoverflow.com/questions/5899683/linq-to-entities-does-not-recognize-the-method-system-string-tostring-method
                var gamesToRemove = from g in db.MlbGameSummaryModel_DbSet
                                    where g.MlbSeasonType == mlbSeasonType &&
                                          g.Year == seasonYear &&
                                          g.Home.Equals(mlbTeamString, StringComparison.InvariantCultureIgnoreCase)
                                    select g;

                foreach (MlbGameSummaryModel gameToRemove in gamesToRemove)
                {
                    db.MlbGameSummaryModel_DbSet.Remove(gameToRemove);
                }

                if (null != gamesToAdd)
                {
                    foreach (MlbGameSummaryModel gameToAdd in gamesToAdd)
                    {
                        db.MlbGameSummaryModel_DbSet.Add(gameToAdd);
                    }
                }

                db.SaveChanges();
            }

            return gamesToAdd;
        }
    }
}
