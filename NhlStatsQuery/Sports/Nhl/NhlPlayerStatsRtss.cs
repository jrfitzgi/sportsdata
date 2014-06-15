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
    public partial class NhlPlayerStatsRtss : NhlPlayerStatsBaseClass
    {
 
        #region Abstract Overrides

        protected override string RelativeUrlFormatString
        {
            // Example: "/ice/playerstats.htm?season=20122013&gameType=3&viewName=rtssPlayerStats&pg=3"
            get { return "/ice/playerstats.htm?season={0}&gameType={1}&viewName=rtssPlayerStats&pg={2}"; }
        }

        #endregion

        #region Public Methods

        public static List<NhlPlayerStatsRtssModel> GetFullSeason(int year, [Optional] bool saveToDb)
        {
            List<NhlPlayerStatsRtssModel> results = new List<NhlPlayerStatsRtssModel>();

            foreach (NhlSeasonType seasonType in Enum.GetValues(typeof(NhlSeasonType)))
            {
                if (seasonType == NhlSeasonType.None) { continue; }

                List<NhlPlayerStatsRtssModel> partialResults = NhlPlayerStatsRtss.UpdateSeason(year, seasonType, saveToDb);
                if (null != partialResults)
                {
                   results.AddRange(partialResults);
                }
            }

            return results;
        }

        #endregion

        #region Private Methods

        private static List<NhlPlayerStatsRtssModel> UpdateSeason(int year, NhlSeasonType nhlSeasonType, bool saveToDb)
        {
            // Get HTML rows
            NhlPlayerStatsRtss nhl = new NhlPlayerStatsRtss();
            List<HtmlNode> rows = nhl.GetResultsForSeasonType(year, nhlSeasonType);

            // Parse into a list
            List<NhlPlayerStatsRtssModel> results = new List<NhlPlayerStatsRtssModel>();
            foreach (HtmlNode row in rows)
            {
                NhlPlayerStatsRtssModel result = NhlPlayerStatsRtss.MapHtmlRowToModel(row, nhlSeasonType, year);

                if (null != result)
                {
                    results.Add(result);
                }
            }

            // Update DB
            if (saveToDb)
            {
                NhlPlayerStatsRtss.AddOrUpdateDb(results);
            }

            return results;
        }

        private static NhlPlayerStatsRtssModel MapHtmlRowToModel(HtmlNode row, NhlSeasonType nhlSeasonType, int year)
        {
            HtmlNodeCollection tdNodes = row.SelectNodes(@"./td");

            NhlPlayerStatsRtssModel model = new NhlPlayerStatsRtssModel();

            model.NhlSeasonType = nhlSeasonType;
            model.Year = year;

            model.Number = 0;
            model.Name = tdNodes[1].InnerText;
            model.Team = tdNodes[2].InnerText;
            model.Position = tdNodes[3].InnerText;

            model.GamesPlayed = ConvertStringToInt(tdNodes[4].InnerText);
            model.Hits = ConvertStringToInt(tdNodes[5].InnerText);
            model.BlockedShots = ConvertStringToInt(tdNodes[6].InnerText);
            model.MissedShots = ConvertStringToInt(tdNodes[7].InnerText);
            model.Giveaways = ConvertStringToInt(tdNodes[8].InnerText);
            model.Takeaways = ConvertStringToInt(tdNodes[9].InnerText);
            model.FaceoffsWon = ConvertStringToInt(tdNodes[10].InnerText);
            model.FaceoffsLost = ConvertStringToInt(tdNodes[11].InnerText);
            model.FaceoffsTaken = ConvertStringToInt(tdNodes[12].InnerText);
            model.FaceoffWinPercentage = Convert.ToDouble(tdNodes[13].InnerText);
            model.PercentageOfTeamFaceoffsTaken = Convert.ToDouble(tdNodes[14].InnerText);
            model.Shots = ConvertStringToInt(tdNodes[15].InnerText);
            model.Goals = ConvertStringToInt(tdNodes[16].InnerText);
            model.ShootingPercentage = Convert.ToDouble(tdNodes[17].InnerText);

            return model;
        }

        private static void AddOrUpdateDb(List<NhlPlayerStatsRtssModel> models)
        {
            using (SportsDataContext db = new SportsDataContext())
            {
                db.NhlPlayerStatsRtss.AddOrUpdate<NhlPlayerStatsRtssModel>(p => new { p.NhlSeasonType, p.Name, p.Year, p.Team }, models.ToArray());
                db.SaveChanges();
            }
        }

        #endregion

    }
}