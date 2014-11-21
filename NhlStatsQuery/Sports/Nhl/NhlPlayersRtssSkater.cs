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
    public partial class NhlPlayersRtssSkater : NhlPlayersBaseClass
    {
 
        #region Abstract Overrides

        protected override string RelativeUrlFormatString
        {
            // Example: "/ice/playerstats.htm?season=2013&gameType=3&viewName=rtssPlayerStats&pg=3"
            get { return "/ice/playerstats.htm?season={0}&gameType={1}&viewName=rtssPlayerStats&pg={2}"; }
        }

        #endregion

        #region Public Methods

        public static List<Nhl_Players_Rtss_Skater> GetFullSeason(int year, [Optional] bool saveToDb)
        {
            List<Nhl_Players_Rtss_Skater> results = new List<Nhl_Players_Rtss_Skater>();

            foreach (NhlSeasonType seasonType in Enum.GetValues(typeof(NhlSeasonType)))
            {
                if (seasonType == NhlSeasonType.None) { continue; }

                List<Nhl_Players_Rtss_Skater> partialResults = NhlPlayersRtssSkater.UpdateSeason(year, seasonType, saveToDb);
                if (null != partialResults)
                {
                   results.AddRange(partialResults);
                }
            }

            return results;
        }

        #endregion

        #region Private Methods

        private static List<Nhl_Players_Rtss_Skater> UpdateSeason(int year, NhlSeasonType nhlSeasonType, bool saveToDb)
        {
            // Get HTML rows
            NhlPlayersRtssSkater nhl = new NhlPlayersRtssSkater();
            List<HtmlNode> rows = nhl.GetResultsForSeasonType(year, nhlSeasonType);

            // Parse into a list
            List<Nhl_Players_Rtss_Skater> results = new List<Nhl_Players_Rtss_Skater>();
            foreach (HtmlNode row in rows)
            {
                Nhl_Players_Rtss_Skater result = NhlPlayersRtssSkater.MapHtmlRowToModel(row, nhlSeasonType, year);

                if (null != result)
                {
                    results.Add(result);
                }
            }

            // Update DB
            if (saveToDb)
            {
                NhlPlayersRtssSkater.AddOrUpdateDb(results);
            }

            return results;
        }

        private static Nhl_Players_Rtss_Skater MapHtmlRowToModel(HtmlNode row, NhlSeasonType nhlSeasonType, int year)
        {
            HtmlNodeCollection tdNodes = row.SelectNodes(@"./td");

            Nhl_Players_Rtss_Skater model = new Nhl_Players_Rtss_Skater();

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

        private static void AddOrUpdateDb(List<Nhl_Players_Rtss_Skater> models)
        {
            using (SportsDataContext db = new SportsDataContext())
            {
                db.Nhl_Players_Rtss_Skater_DbSet.AddOrUpdate<Nhl_Players_Rtss_Skater>(p => new { p.NhlSeasonType, p.Name, p.Year, p.Team }, models.ToArray());
                db.SaveChanges();
            }
        }

        #endregion

    }
}