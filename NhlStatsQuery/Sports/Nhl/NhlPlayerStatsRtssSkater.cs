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
    public partial class NhlPlayerStatsRtssSkater : NhlPlayerStatsBaseClass
    {
 
        #region Abstract Overrides

        protected override string RelativeUrlFormatString
        {
            // Example: "/ice/playerstats.htm?season=2013&gameType=3&viewName=rtssPlayerStats&pg=3"
            get { return "/ice/playerstats.htm?season={0}&gameType={1}&viewName=rtssPlayerStats&pg={2}"; }
        }

        #endregion

        #region Public Methods

        public static List<NhlPlayerStatsRtssSkaterModel> GetFullSeason(int year, [Optional] bool saveToDb)
        {
            List<NhlPlayerStatsRtssSkaterModel> results = new List<NhlPlayerStatsRtssSkaterModel>();

            foreach (NhlSeasonType seasonType in Enum.GetValues(typeof(NhlSeasonType)))
            {
                if (seasonType == NhlSeasonType.None) { continue; }

                List<NhlPlayerStatsRtssSkaterModel> partialResults = NhlPlayerStatsRtssSkater.UpdateSeason(year, seasonType, saveToDb);
                if (null != partialResults)
                {
                   results.AddRange(partialResults);
                }
            }

            return results;
        }

        #endregion

        #region Private Methods

        private static List<NhlPlayerStatsRtssSkaterModel> UpdateSeason(int year, NhlSeasonType nhlSeasonType, bool saveToDb)
        {
            // Get HTML rows
            NhlPlayerStatsRtssSkater nhl = new NhlPlayerStatsRtssSkater();
            List<HtmlNode> rows = nhl.GetResultsForSeasonType(year, nhlSeasonType);

            // Parse into a list
            List<NhlPlayerStatsRtssSkaterModel> results = new List<NhlPlayerStatsRtssSkaterModel>();
            foreach (HtmlNode row in rows)
            {
                NhlPlayerStatsRtssSkaterModel result = NhlPlayerStatsRtssSkater.MapHtmlRowToModel(row, nhlSeasonType, year);

                if (null != result)
                {
                    results.Add(result);
                }
            }

            // Update DB
            if (saveToDb)
            {
                NhlPlayerStatsRtssSkater.AddOrUpdateDb(results);
            }

            return results;
        }

        private static NhlPlayerStatsRtssSkaterModel MapHtmlRowToModel(HtmlNode row, NhlSeasonType nhlSeasonType, int year)
        {
            HtmlNodeCollection tdNodes = row.SelectNodes(@"./td");

            NhlPlayerStatsRtssSkaterModel model = new NhlPlayerStatsRtssSkaterModel();

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

        private static void AddOrUpdateDb(List<NhlPlayerStatsRtssSkaterModel> models)
        {
            using (SportsDataContext db = new SportsDataContext())
            {
                db.NhlPlayerStatsRtssSkaters.AddOrUpdate<NhlPlayerStatsRtssSkaterModel>(p => new { p.NhlSeasonType, p.Name, p.Year, p.Team }, models.ToArray());
                db.SaveChanges();
            }
        }

        #endregion

    }
}