using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices;

using HtmlAgilityPack;
using SportsData.Models;

namespace SportsData.Nhl
{
    public class NhlGamesRtss : NhlGamesBaseClass
    {

        #region Abstract Overrides

        protected override string RelativeUrlFormatString
        {
            get
            {
                return "/ice/gamestats.htm?season={0}&gameType={1}&viewName=teamRTSSreports&sort=date&pg={2}";
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get all the results starting from the last date of the data in the db. If a year is specified then only get latest for that year.
        /// </summary>
        /// <param name="year"></param>
        public static List<Nhl_Games_Rtss> GetNewResultsOnly([Optional] int year, [Optional] bool saveToDb)
        {
            DateTime latestResultDate;
            using (SportsDataContext db = new SportsDataContext())
            {
                latestResultDate = (from m in db.Nhl_Games_Rtss_DbSet
                                    orderby m.Date descending
                                    select m.Date).FirstOrDefault();

            }

            return NhlGamesRtss.GetFullSeason(year, latestResultDate, saveToDb);
        }

        public static List<Nhl_Games_Rtss> GetFullSeason([Optional] int year, [Optional] DateTime fromDate, [Optional] bool saveToDb)
        {
            List<Nhl_Games_Rtss> results = new List<Nhl_Games_Rtss>();

            foreach (NhlSeasonType seasonType in Enum.GetValues(typeof(NhlSeasonType)))
            {
                if (seasonType == NhlSeasonType.None) { continue; }

                List<Nhl_Games_Rtss> partialResults = NhlGamesRtss.UpdateSeason(year, seasonType, fromDate, saveToDb);
                if (null != partialResults)
                {
                    results.AddRange(partialResults);
                }
            }

            return results;
        }

        #endregion

        #region Private Methods

        private static List<Nhl_Games_Rtss> UpdateSeason(int year, NhlSeasonType nhlSeasonType, DateTime fromDate, bool saveToDb)
        {
            // Get HTML rows
            NhlGamesRtss nhl = new NhlGamesRtss();
            List<HtmlNode> rows = nhl.GetResultsForSeasonType(year, nhlSeasonType, fromDate);

            // Parse into a list
            List<Nhl_Games_Rtss> results = new List<Nhl_Games_Rtss>();
            foreach (HtmlNode row in rows)
            {
                Nhl_Games_Rtss result = NhlGamesRtss.MapHtmlRowToModel(row, nhlSeasonType);

                if (null != result)
                {
                    results.Add(result);
                }
            }

            // Update DB
            if (saveToDb)
            {
                NhlGamesRtss.AddOrUpdateDb(results);
            }

            return results;
        }

        private static Nhl_Games_Rtss MapHtmlRowToModel(HtmlNode row, NhlSeasonType nhlSeasonType)
        {
            HtmlNodeCollection tdNodes = row.SelectNodes(@"./td");

            Nhl_Games_Rtss model = new Nhl_Games_Rtss();

            model.NhlSeasonType = nhlSeasonType;
            model.Date = Convert.ToDateTime(tdNodes[0].InnerText.Replace("'", "/"));
            model.Year = NhlModelHelper.GetSeason(model.Date).Item2;

            model.GameNumber = Convert.ToInt32(tdNodes[1].InnerText);
            model.Visitor = tdNodes[2].InnerText;
            model.Home = tdNodes[3].InnerText;

            model.RosterLink = NhlGamesRtss.ParseLinkFromTd(tdNodes[4]);
            model.GameLink = NhlGamesRtss.ParseLinkFromTd(tdNodes[5]);
            model.EventsLink = NhlGamesRtss.ParseLinkFromTd(tdNodes[6]);
            model.FaceOffsLink = NhlGamesRtss.ParseLinkFromTd(tdNodes[7]);
            model.PlayByPlayLink = NhlGamesRtss.ParseLinkFromTd(tdNodes[8]);
            model.ShotsLink = NhlGamesRtss.ParseLinkFromTd(tdNodes[9]);
            model.HomeToiLink = NhlGamesRtss.ParseLinkFromTd(tdNodes[10]);
            model.VistorToiLink = NhlGamesRtss.ParseLinkFromTd(tdNodes[11]);
            model.ShootoutLink = NhlGamesRtss.ParseLinkFromTd(tdNodes[12]);

            return model;
        }

        private static void AddOrUpdateDb(List<Nhl_Games_Rtss> models)
        {
            // Note: downcast for models is not necessary but leave this here in anticipation of moving this method to a base class (and it will be necessary)

            // Special case the FLA/NSH double header on 9/16/2013
            IEnumerable<Nhl_Games_BaseModel> specialCaseModels = NhlGamesBaseClass.GetSpecialCaseModels(models);
            IEnumerable<Nhl_Games_Rtss> downcastSpecialCaseModels = specialCaseModels.ToList().ConvertAll<Nhl_Games_Rtss>(m => (Nhl_Games_Rtss)m);
            IEnumerable<Nhl_Games_Rtss> downcastModels = models.Except(specialCaseModels, new NhlGameStatsBaseModelComparer()).ToList().ConvertAll<Nhl_Games_Rtss>(m => (Nhl_Games_Rtss)m);

            using (SportsDataContext db = new SportsDataContext())
            {
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

                db.Nhl_Games_Rtss_DbSet.AddOrUpdate<Nhl_Games_Rtss>(g => new { g.Date, g.Visitor, g.Home, g.GameNumber}, downcastSpecialCaseModels.ToArray());
                db.Nhl_Games_Rtss_DbSet.AddOrUpdate<Nhl_Games_Rtss>(g => new { g.Date, g.Visitor, g.Home }, downcastModels.ToArray());
                db.SaveChanges();
            }
        }
        
        private static string ParseLinkFromTd(HtmlNode tdNode)
        {
            HtmlNode linkNode = tdNode.SelectSingleNode(@"./a/@href");

            if (null == linkNode)
            {
                return null;
            }
            else
            {
                return linkNode.GetAttributeValue("href", null);
            }
        }

        #endregion

    }
}
