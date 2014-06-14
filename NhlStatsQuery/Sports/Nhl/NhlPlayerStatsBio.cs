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
    public partial class NhlPlayerStatsBio : NhlPlayerStatsBaseClass
    {
 
        #region Abstract Overrides

        protected override string RelativeUrlFormatString
        {
            // Example: "/ice/playerstats.htm?season=20122013&gameType=3&viewName=bios&pg=3"
            get { return "/ice/playerstats.htm?season={0}&gameType={1}&viewName=bios&pg={2}"; }
        }

        #endregion

        #region Public Methods

        public static List<NhlPlayerStatsBioModel> GetFullSeason([Optional] int year, [Optional] DateTime fromDate, [Optional] bool saveToDb)
        {
            List<NhlPlayerStatsBioModel> results = new List<NhlPlayerStatsBioModel>();

            foreach (NhlSeasonType seasonType in Enum.GetValues(typeof(NhlSeasonType)))
            {
                if (seasonType == NhlSeasonType.None) { continue; }

                List<NhlPlayerStatsBioModel> partialResults = NhlPlayerStatsBio.UpdateSeason(year, seasonType, fromDate, saveToDb);
                if (null != partialResults)
                {
                    results.AddRange(partialResults);
                }
            }

            return results;
        }

        #endregion

        #region Private Methods

        private static List<NhlPlayerStatsBioModel> UpdateSeason(int year, NhlSeasonType nhlSeasonType, DateTime fromDate, bool saveToDb)
        {
            // Get HTML rows
            NhlPlayerStatsBio nhl = new NhlPlayerStatsBio();
            List<HtmlNode> rows = nhl.GetResultsForSeasonType(year, nhlSeasonType, fromDate);

            // Parse into a list
            List<NhlPlayerStatsBioModel> results = new List<NhlPlayerStatsBioModel>();
            foreach (HtmlNode row in rows)
            {
                NhlPlayerStatsBioModel result = NhlPlayerStatsBio.MapHtmlRowToModel(row, nhlSeasonType);

                if (null != result)
                {
                    results.Add(result);
                }
            }

            // Update DB
            if (saveToDb)
            {
                NhlPlayerStatsBio.AddOrUpdateDb(results);
            }

            return results;
        }

        private static NhlPlayerStatsBioModel MapHtmlRowToModel(HtmlNode row, NhlSeasonType nhlSeasonType)
        {
            HtmlNodeCollection tdNodes = row.SelectNodes(@"./td");

            NhlPlayerStatsBioModel model = new NhlPlayerStatsBioModel();

            //model.NhlSeasonType = nhlSeasonType;
            //model.Date = Convert.ToDateTime(tdNodes[0].InnerText.Replace("'", "/"));
            //model.Year = NhlModelHelper.GetSeason(model.Date).Item2;

            //model.Visitor = tdNodes[1].InnerText;
            //model.VisitorScore = ConvertStringToInt(tdNodes[2].InnerText);
            //model.Home = tdNodes[3].InnerText;
            //model.HomeScore = ConvertStringToInt(tdNodes[4].InnerText);
            //model.OS = tdNodes[5].InnerText;
            //model.WGoalie = tdNodes[6].InnerText;
            //model.WGoal = tdNodes[7].InnerText;
            //model.VisitorShots = ConvertStringToInt(tdNodes[8].InnerText);
            //model.VisitorPPGF = ConvertStringToInt(tdNodes[9].InnerText);
            //model.VisitorPPOpp = ConvertStringToInt(tdNodes[10].InnerText);
            //model.VisitorPIM = ConvertStringToInt(tdNodes[11].InnerText);
            //model.HomeShots = ConvertStringToInt(tdNodes[12].InnerText);
            //model.HomePPGF = ConvertStringToInt(tdNodes[13].InnerText);
            //model.HomePPOpp = ConvertStringToInt(tdNodes[14].InnerText);
            //model.HomePIM = ConvertStringToInt(tdNodes[15].InnerText);
            //model.Attendance = ConvertStringToInt(tdNodes[16].InnerText.Replace(",", String.Empty));

            return model;
        }

        private static void AddOrUpdateDb(List<NhlPlayerStatsBioModel> models)
        {
            using (SportsDataContext db = new SportsDataContext())
            {
                db.NhlPlayerStatsBios.AddOrUpdate<NhlPlayerStatsBioModel>(p => new { p.NhlSeasonType, p.Name, p.Year, p.Team }, models.ToArray());
                db.SaveChanges();
            }
        }

        #endregion

    }
}