using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using HtmlAgilityPack;
using SportsData.Models;

namespace SportsData.Nhl
{
    public partial class NhlRtssReport : NhlBaseClass
    {
        #region Public Methods

        public static void UpdateSeason(int year)
        {
            NhlRtssReport instance = new NhlRtssReport();
            instance.UpdateSeason_protected(year);
        }

        #endregion

        #region Abstract Overrides

        protected override string RelativeUrlFormatString
        {
            get
            {
                // eg. /ice/gamestats.htm?season=2014&gameType=1&viewName=teamRTSSreports&sort=date&pg=2
                return "/ice/gamestats.htm?season={0}&gameType={1}&viewName=teamRTSSreports&sort=date&pg={2}";
            }
        }

        protected override Type ModelType
        {
            get
            {
                return typeof(NhlRtssReportModel);
            }
        }

        protected override NhlGameStatsBaseModel MapHtmlRowToModel(HtmlNode row, NhlSeasonType nhlSeasonType)
        {
            HtmlNodeCollection tdNodes = row.SelectNodes(@"./td");

            NhlRtssReportModel model = new NhlRtssReportModel();

            model.NhlSeasonType = nhlSeasonType;
            model.Date = Convert.ToDateTime(tdNodes[0].InnerText.Replace("'", "/"));
            model.Year = NhlGameSummaryModel.GetSeason(model.Date).Item2;

            model.GameNumber = Convert.ToInt32(tdNodes[1].InnerText);
            model.Visitor = tdNodes[2].InnerText;
            model.Home = tdNodes[3].InnerText;

            model.RosterLink = NhlRtssReport.ParseLinkFromTd(tdNodes[4]);
            model.GameLink = NhlRtssReport.ParseLinkFromTd(tdNodes[5]);
            model.EventsLink = NhlRtssReport.ParseLinkFromTd(tdNodes[6]);
            model.FaceOffsLink = NhlRtssReport.ParseLinkFromTd(tdNodes[7]);
            model.PlayByPlayLink = NhlRtssReport.ParseLinkFromTd(tdNodes[8]);
            model.ShotsLink = NhlRtssReport.ParseLinkFromTd(tdNodes[9]);
            model.HomeToiLink = NhlRtssReport.ParseLinkFromTd(tdNodes[10]);
            model.VistorToiLink = NhlRtssReport.ParseLinkFromTd(tdNodes[11]);
            model.ShootoutLink = NhlRtssReport.ParseLinkFromTd(tdNodes[12]);

            return model;
        }

        protected override void AddOrUpdateDb(List<NhlGameStatsBaseModel> models)
        {
            // Special case the FLA/NSH double header on 9/16/2013
            IEnumerable<NhlGameStatsBaseModel> specialCaseModels = this.GetSpecialCaseModels(models);
            IEnumerable<NhlRtssReportModel> downcastSpecialCaseModels = specialCaseModels.ToList().ConvertAll<NhlRtssReportModel>(m => (NhlRtssReportModel)m);
            IEnumerable<NhlRtssReportModel> downcastModels = models.Except(specialCaseModels, new NhlGameStatsBaseModelComparer()).ToList().ConvertAll<NhlRtssReportModel>(m => (NhlRtssReportModel)m);

            using (SportsDataContext db = new SportsDataContext())
            {
                db.NhlRtssReports.AddOrUpdate<NhlRtssReportModel>(g => new { g.Date, g.Visitor, g.Home, g.GameNumber}, downcastSpecialCaseModels.ToArray());
                db.NhlRtssReports.AddOrUpdate<NhlRtssReportModel>(g => new { g.Date, g.Visitor, g.Home }, downcastModels.ToArray());
                db.SaveChanges();
            }
        }

        protected override void UpdateSeason_protected(int year)
        {
            List<NhlGameStatsBaseModel> results = this.GetSeason(year);
            this.AddOrUpdateDb(results);
        }
        
        #endregion

        #region Private Methods

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

        #region Unused Code

        //public override T MapHtmlRowToModel<T>(HtmlNode row, NhlSeasonType nhlSeasonType)
        //{
        //    this.CheckType<T>();

        //    HtmlNodeCollection tdNodes = row.SelectNodes(@"./td");

        //    NhlRtssReportModel model = new NhlRtssReportModel();
        //    model.NhlSeasonType = nhlSeasonType;
        //    model.Date = Convert.ToDateTime(tdNodes[0].InnerText.Replace("'", "/"));
        //    model.Year = NhlGameSummaryModel.GetSeason(model.Date).Item2;

        //    model.GameNumber = Convert.ToInt32(tdNodes[1].InnerText);
        //    model.Visitor = tdNodes[2].InnerText;
        //    model.Home = tdNodes[3].InnerText;

        //    model.RosterLink = NhlRtssReport.ParseLinkFromTd(tdNodes[4]);
        //    model.GameLink = NhlRtssReport.ParseLinkFromTd(tdNodes[5]);
        //    model.EventsLink = NhlRtssReport.ParseLinkFromTd(tdNodes[6]);
        //    model.FaceOffsLink = NhlRtssReport.ParseLinkFromTd(tdNodes[7]);
        //    model.PlayByPlayLink = NhlRtssReport.ParseLinkFromTd(tdNodes[8]);
        //    model.ShotsLink = NhlRtssReport.ParseLinkFromTd(tdNodes[9]);
        //    model.HomeToiLink = NhlRtssReport.ParseLinkFromTd(tdNodes[10]);
        //    model.VistorToiLink = NhlRtssReport.ParseLinkFromTd(tdNodes[11]);
        //    model.ShootoutLink = NhlRtssReport.ParseLinkFromTd(tdNodes[12]);

        //    return model as T;
        //}
        #endregion

    }
}
