using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using HtmlAgilityPack;
using SportsData.Models;

namespace SportsData.Nhl
{
    public partial class NhlGamesSummary : NhlGamesBaseClass
    {
 
        #region Abstract Overrides

        protected override string RelativeUrlFormatString
        {
            get { return "/ice/gamestats.htm?season={0}&gameType={1}&viewName=summary&sort=date&pg={2}"; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get all the results starting from the last date of the data in the db. If a year is specified then only get latest for that year.
        /// </summary>
        /// <param name="year"></param>
        public static List<Nhl_Games_Summary> GetNewResultsOnly([Optional] int year, [Optional] bool saveToDb)
        {
            DateTime latestResultDate;
            using (SportsDataContext db = new SportsDataContext())
            {
                latestResultDate = (from m in db.Nhl_Games_Summary_DbSet
                                    orderby m.Date descending
                                    select m.Date).FirstOrDefault();

            }

            return NhlGamesSummary.GetFullSeason(year, latestResultDate, saveToDb);
        }

        public static List<Nhl_Games_Summary> GetFullSeason([Optional] int year, [Optional] DateTime fromDate, [Optional] bool saveToDb)
        {
            List<Nhl_Games_Summary> results = new List<Nhl_Games_Summary>();

            foreach (NhlSeasonType seasonType in Enum.GetValues(typeof(NhlSeasonType)))
            {
                if (seasonType == NhlSeasonType.None) { continue; }

                List<Nhl_Games_Summary> partialResults = NhlGamesSummary.UpdateSeason(year, seasonType, fromDate, saveToDb);
                if (null != partialResults)
                {
                    results.AddRange(partialResults);
                }
            }

            return results;
        }

        #endregion

        #region Private Methods

        private static List<Nhl_Games_Summary> UpdateSeason(int year, NhlSeasonType nhlSeasonType, DateTime fromDate, bool saveToDb)
        {
            // Get HTML rows
            NhlGamesSummary nhl = new NhlGamesSummary();
            List<HtmlNode> rows = nhl.GetResultsForSeasonType(year, nhlSeasonType, fromDate);

            // Parse into a list
            List<Nhl_Games_Summary> results = new List<Nhl_Games_Summary>();
            foreach (HtmlNode row in rows)
            {
                Nhl_Games_Summary result = NhlGamesSummary.MapHtmlRowToModel(row, nhlSeasonType);

                if (null != result)
                {
                    results.Add(result);
                }
            }

            // Update DB
            if (saveToDb)
            {
                NhlGamesSummary.AddOrUpdateDb(results);
            }

            return results;
        }
        
        private static Nhl_Games_Summary MapHtmlRowToModel(HtmlNode row, NhlSeasonType nhlSeasonType)
        {
            HtmlNodeCollection tdNodes = row.SelectNodes(@"./td");

            Nhl_Games_Summary model = new Nhl_Games_Summary();

            model.NhlSeasonType = nhlSeasonType;
            model.Date = Convert.ToDateTime(tdNodes[0].InnerText.Replace("'", "/"));
            model.Year = NhlModelHelper.GetSeason(model.Date).Item2;

            model.Visitor = tdNodes[1].InnerText;
            model.VisitorScore = ConvertStringToInt(tdNodes[2].InnerText);
            model.Home = tdNodes[3].InnerText;
            model.HomeScore = ConvertStringToInt(tdNodes[4].InnerText);
            model.OS = tdNodes[5].InnerText;
            model.WGoalie = tdNodes[6].InnerText;
            model.WGoal = tdNodes[7].InnerText;
            model.VisitorShots = ConvertStringToInt(tdNodes[8].InnerText);
            model.VisitorPPGF = ConvertStringToInt(tdNodes[9].InnerText);
            model.VisitorPPOpp = ConvertStringToInt(tdNodes[10].InnerText);
            model.VisitorPIM = ConvertStringToInt(tdNodes[11].InnerText);
            model.HomeShots = ConvertStringToInt(tdNodes[12].InnerText);
            model.HomePPGF = ConvertStringToInt(tdNodes[13].InnerText);
            model.HomePPOpp = ConvertStringToInt(tdNodes[14].InnerText);
            model.HomePIM = ConvertStringToInt(tdNodes[15].InnerText);
            model.Attendance = ConvertStringToInt(tdNodes[16].InnerText.Replace(",", String.Empty));

            return model;
        }

        private static void AddOrUpdateDb(List<Nhl_Games_Summary> models)
        {
            // Note: downcast for models is not necessary but leave this here in anticipation of moving this method to a base class (and it will be necessary)

            // Special case the FLA/NSH double header on 9/16/2013
            IEnumerable<Nhl_Games_BaseModel> specialCaseModels = NhlGamesBaseClass.GetSpecialCaseModels(models);
            IEnumerable<Nhl_Games_Summary> downcastSpecialCaseModels = specialCaseModels.ToList().ConvertAll<Nhl_Games_Summary>(m => (Nhl_Games_Summary)m);
            IEnumerable<Nhl_Games_Summary> downcastModels = models.Except(specialCaseModels, new NhlGameStatsBaseModelComparer()).ToList().ConvertAll<Nhl_Games_Summary>(m => (Nhl_Games_Summary)m);

            using (SportsDataContext db = new SportsDataContext())
            {
                db.Nhl_Games_Summary_DbSet.AddOrUpdate<Nhl_Games_Summary>(g => new { g.Date, g.Visitor, g.Home, g.VisitorScore, g.HomeScore }, downcastSpecialCaseModels.ToArray());
                db.Nhl_Games_Summary_DbSet.AddOrUpdate<Nhl_Games_Summary>(g => new { g.Date, g.Visitor, g.Home }, downcastModels.ToArray());
                db.SaveChanges();
            }
        }

        #endregion

    }
}