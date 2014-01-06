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
    /// <summary>
    /// Represents a query that will be used to retrieve stats from a url
    /// </summary>
    public partial class NhlGameSummary : NhlBaseClass
    {
        #region Public Methods

        /// <summary>
        /// Get all the results starting from the last date of the data in the db. If a year is specified then only get latest for that year.
        /// </summary>
        /// <param name="year"></param>
        public static void UpdateSeasonWithLatestOnly([Optional] int year)
        {
            DateTime latestResultDate;
            using (SportsDataContext db = new SportsDataContext())
            {
                latestResultDate = (from m in db.NhlGameSummaries
                                    orderby m.Date descending
                                    select m.Date).FirstOrDefault();

            }

            NhlGameSummary.UpdateSeason(year, latestResultDate);
        }
        
        public static void UpdateSeason([Optional] int year, [Optional] DateTime fromDate)
        {
            NhlBaseClass.UpdateSeason<NhlGameSummary>(year, fromDate);
        }

        #endregion

        #region Abstract Overrides

        protected override string RelativeUrlFormatString
        {
            get { return "/ice/gamestats.htm?season={0}&gameType={1}&viewName=summary&sort=date&pg={2}"; }
        }

        protected override Type ModelType
        {
            get
            {
                return typeof(NhlGameSummaryModel);
            }
        }

        protected override NhlGameStatsBaseModel MapHtmlRowToModel(HtmlNode row, NhlSeasonType nhlSeasonType)
        {
            HtmlNodeCollection tdNodes = row.SelectNodes(@"./td");

            NhlGameSummaryModel model = new NhlGameSummaryModel();

            model.NhlSeasonType = nhlSeasonType;
            model.Date = Convert.ToDateTime(tdNodes[0].InnerText.Replace("'", "/"));
            model.Year = NhlGameSummaryModel.GetSeason(model.Date).Item2;

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

        protected override void AddOrUpdateDb(List<NhlGameStatsBaseModel> models)
        {
            // Special case the FLA/NSH double header on 9/16/2013
            IEnumerable<NhlGameStatsBaseModel> specialCaseModels = this.GetSpecialCaseModels(models);
            IEnumerable<NhlGameSummaryModel> downcastSpecialCaseModels = specialCaseModels.ToList().ConvertAll<NhlGameSummaryModel>(m => (NhlGameSummaryModel)m);
            IEnumerable<NhlGameSummaryModel> downcastModels = models.Except(specialCaseModels, new NhlGameStatsBaseModelComparer()).ToList().ConvertAll<NhlGameSummaryModel>(m => (NhlGameSummaryModel)m);

            using (SportsDataContext db = new SportsDataContext())
            {
                db.NhlGameSummaries.AddOrUpdate<NhlGameSummaryModel>(g => new { g.Date, g.Visitor, g.Home, g.VisitorScore, g.HomeScore }, downcastSpecialCaseModels.ToArray());
                db.NhlGameSummaries.AddOrUpdate<NhlGameSummaryModel>(g => new { g.Date, g.Visitor, g.Home }, downcastModels.ToArray());
                db.SaveChanges();
            }
        }

        #endregion

        #region Private Methods

        private static int ConvertStringToInt(string s)
        {
            int result;
            bool success = Int32.TryParse(s, out result);

            if (!success)
            {
                return 0;
            }
            else
            {
                return result;
            }
        }

        #endregion

    }
}