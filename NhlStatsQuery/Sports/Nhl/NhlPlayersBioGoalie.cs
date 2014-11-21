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
    public partial class NhlPlayersBioGoalie : NhlPlayersBaseClass
    {
 
        #region Abstract Overrides

        protected override string RelativeUrlFormatString
        {
            // Example: "/ice/playerstats.htm?season=2013&gameType=3&pg=3&viewName=goalieBios&position=G"
            get { return "/ice/playerstats.htm?season={0}&gameType={1}&pg={2}&viewName=goalieBios&position=G"; }
        }

        #endregion

        #region Public Methods

        public static List<Nhl_Players_Bio_Goalie> GetFullSeason(int year, [Optional] bool saveToDb)
        {
            List<Nhl_Players_Bio_Goalie> results = new List<Nhl_Players_Bio_Goalie>();

            foreach (NhlSeasonType seasonType in Enum.GetValues(typeof(NhlSeasonType)))
            {
                if (seasonType == NhlSeasonType.None) { continue; }

                List<Nhl_Players_Bio_Goalie> partialResults = NhlPlayersBioGoalie.UpdateSeason(year, seasonType, saveToDb);
                if (null != partialResults)
                {
                   results.AddRange(partialResults);
                }
            }

            return results;
        }

        #endregion

        #region Private Methods

        private static List<Nhl_Players_Bio_Goalie> UpdateSeason(int year, NhlSeasonType nhlSeasonType, bool saveToDb)
        {
            // Get HTML rows
            NhlPlayersBioGoalie nhl = new NhlPlayersBioGoalie();
            List<HtmlNode> rows = nhl.GetResultsForSeasonType(year, nhlSeasonType);

            // Parse into a list
            List<Nhl_Players_Bio_Goalie> results = new List<Nhl_Players_Bio_Goalie>();
            foreach (HtmlNode row in rows)
            {
                Nhl_Players_Bio_Goalie result = NhlPlayersBioGoalie.MapHtmlRowToModel(row, nhlSeasonType, year);

                if (null != result)
                {
                    results.Add(result);
                }
            }

            // Update DB
            if (saveToDb)
            {
                NhlPlayersBioGoalie.AddOrUpdateDb(results);
            }

            return results;
        }

        private static Nhl_Players_Bio_Goalie MapHtmlRowToModel(HtmlNode row, NhlSeasonType nhlSeasonType, int year)
        {
            HtmlNodeCollection tdNodes = row.SelectNodes(@"./td");

            Nhl_Players_Bio_Goalie model = new Nhl_Players_Bio_Goalie();

            model.NhlSeasonType = nhlSeasonType;
            model.Year = year;

            model.Number = ConvertStringToInt(tdNodes[0].InnerText);
            model.Name = tdNodes[1].InnerText;
            model.Team = tdNodes[2].InnerText;
            model.Position = "G";
            model.DateOfBirth = Convert.ToDateTime(tdNodes[3].InnerText.Replace("'", "/"));
            model.BirthCity = tdNodes[4].InnerText;
            model.StateOrProvince = tdNodes[5].InnerText;
            model.BirthCountry = tdNodes[6].InnerText;
            model.HeightInches = ConvertStringToInt(tdNodes[7].InnerText);
            model.WeightLbs = ConvertStringToInt(tdNodes[8].InnerText);
            model.Catches = tdNodes[9].InnerText;
            model.Rookie = tdNodes[10].InnerText;
            model.DraftYear = ConvertStringToInt(tdNodes[11].InnerText);
            model.DraftRound = ConvertStringToInt(tdNodes[12].InnerText);
            model.DraftOverall = ConvertStringToInt(tdNodes[13].InnerText);

            model.GamesPlayed = ConvertStringToInt(tdNodes[14].InnerText);
            model.Wins = ConvertStringToInt(tdNodes[15].InnerText);
            model.Losses = ConvertStringToInt(tdNodes[16].InnerText);
            model.OTSOLosses = ConvertStringToInt(tdNodes[17].InnerText);
            model.GAA = Convert.ToDouble(tdNodes[18].InnerText);
            model.SavePercentage = Convert.ToDouble(tdNodes[19].InnerText);
            model.Shutouts = ConvertStringToInt(tdNodes[20].InnerText);

            return model;
        }

        private static void AddOrUpdateDb(List<Nhl_Players_Bio_Goalie> models)
        {
            using (SportsDataContext db = new SportsDataContext())
            {
                db.Nhl_Players_Bio_Goalie_DbSet.AddOrUpdate<Nhl_Players_Bio_Goalie>(p => new { p.NhlSeasonType, p.Name, p.Year, p.Team }, models.ToArray());
                db.SaveChanges();
            }
        }

        #endregion

    }
}