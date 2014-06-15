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

        public static List<NhlPlayerStatsBioSkaterModel> GetFullSeason(int year, [Optional] bool saveToDb)
        {
            List<NhlPlayerStatsBioSkaterModel> results = new List<NhlPlayerStatsBioSkaterModel>();

            foreach (NhlSeasonType seasonType in Enum.GetValues(typeof(NhlSeasonType)))
            {
                if (seasonType == NhlSeasonType.None) { continue; }

                List<NhlPlayerStatsBioSkaterModel> partialResults = NhlPlayerStatsBio.UpdateSeason(year, seasonType, saveToDb);
                if (null != partialResults)
                {
                   results.AddRange(partialResults);
                }
            }

            return results;
        }

        #endregion

        #region Private Methods

        private static List<NhlPlayerStatsBioSkaterModel> UpdateSeason(int year, NhlSeasonType nhlSeasonType, bool saveToDb)
        {
            // Get HTML rows
            NhlPlayerStatsBio nhl = new NhlPlayerStatsBio();
            List<HtmlNode> rows = nhl.GetResultsForSeasonType(year, nhlSeasonType);

            // Parse into a list
            List<NhlPlayerStatsBioSkaterModel> results = new List<NhlPlayerStatsBioSkaterModel>();
            foreach (HtmlNode row in rows)
            {
                NhlPlayerStatsBioSkaterModel result = NhlPlayerStatsBio.MapHtmlRowToModel(row, nhlSeasonType, year);

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

        private static NhlPlayerStatsBioSkaterModel MapHtmlRowToModel(HtmlNode row, NhlSeasonType nhlSeasonType, int year)
        {
            HtmlNodeCollection tdNodes = row.SelectNodes(@"./td");

            NhlPlayerStatsBioSkaterModel model = new NhlPlayerStatsBioSkaterModel();

            model.NhlSeasonType = nhlSeasonType;
            model.Year = year;

            model.Number = ConvertStringToInt(tdNodes[0].InnerText);
            model.Name = tdNodes[1].InnerText;
            model.Team = tdNodes[2].InnerText;
            model.Position = tdNodes[3].InnerText;
            model.DateOfBirth = Convert.ToDateTime(tdNodes[4].InnerText.Replace("'", "/"));
            model.BirthCity = tdNodes[5].InnerText;
            model.StateOrProvince = tdNodes[6].InnerText;
            model.BirthCountry = tdNodes[7].InnerText;
            model.HeightInches = ConvertStringToInt(tdNodes[8].InnerText);
            model.WeightLbs = ConvertStringToInt(tdNodes[9].InnerText);
            model.Shoots = tdNodes[10].InnerText;
            model.DraftYear = ConvertStringToInt(tdNodes[11].InnerText);
            model.DraftRound = ConvertStringToInt(tdNodes[12].InnerText);
            model.DraftOverall = ConvertStringToInt(tdNodes[13].InnerText);
            model.Rookie = tdNodes[14].InnerText;
            model.GamesPlayed = ConvertStringToInt(tdNodes[15].InnerText);
            model.Goals = ConvertStringToInt(tdNodes[16].InnerText);
            model.Assists = ConvertStringToInt(tdNodes[17].InnerText);
            model.Points = ConvertStringToInt(tdNodes[18].InnerText);
            model.PlusMinus = ConvertStringToInt(tdNodes[19].InnerText);
            model.PIM = ConvertStringToInt(tdNodes[20].InnerText);

            string toi = tdNodes[21].InnerText;
            string[] toiParts = toi.Split(':');
            if (toiParts.Length == 2)
            {
                int toiMinutes = ConvertStringToInt(toiParts[0]);
                int toiSeconds = ConvertStringToInt(toiParts[1]);
                model.ToiSecondsPerGame = toiMinutes * 60 + toiSeconds;
            }
            else if (toiParts.Length == 1)
            {
                int toiSeconds = ConvertStringToInt(toiParts[0]);
                model.ToiSecondsPerGame = toiSeconds;
            }
            else
            {
                model.ToiSecondsPerGame = 0;
            }

            return model;
        }

        private static void AddOrUpdateDb(List<NhlPlayerStatsBioSkaterModel> models)
        {
            using (SportsDataContext db = new SportsDataContext())
            {
                db.NhlPlayerStatsBioSkaters.AddOrUpdate<NhlPlayerStatsBioSkaterModel>(p => new { p.NhlSeasonType, p.Name, p.Year, p.Team }, models.ToArray());
                db.SaveChanges();
            }
        }

        #endregion

    }
}