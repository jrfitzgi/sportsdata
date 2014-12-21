using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SportsData.Mlb;
using SportsData.Models;
using SportsData.Nhl;
using SportsData.Social;

namespace SportsData.Controllers
{
    public class UpdateController : SportsDataController
    {
        private Guid expectedKey = new Guid("5774B680-047B-47EB-B465-3DBF946C3E7A");

        /// <summary>
        /// Validates the Key that is part of the http request
        /// </summary>
        /// <returns></returns>
        private bool IsKeyValid()
        {
            Guid submittedKey = Guid.Parse(this.Request["Key"]);
            bool keyIsValid = (expectedKey == submittedKey);
            return keyIsValid;
        }

        [HttpPost]
        public ActionResult AllNhl()
        {
            Dictionary<string, Exception> results = new Dictionary<string, Exception>();

            if (this.IsKeyValid())
            {
                results.Add("NhlGamesSummary", this.Update(delegate() { SportsData.Nhl.NhlGamesSummary.GetNewResultsOnly(saveToDb: true); }));
                results.Add("NhlGamesRtss", this.Update(delegate() { SportsData.Nhl.NhlGamesRtss.GetNewResultsOnly(saveToDb: true); }));
                results.Add("HtmlBlob", this.Update(delegate() { HtmlBlob.UpdateSeason(); }));
                results.Add("NhlGamesRtssSummary", this.Update(delegate() { SportsData.Nhl.NhlGamesRtssSummary.UpdateSeason(); }));
                results.Add("NhlGamesRtssRoster", this.Update(delegate() { SportsData.Nhl.NhlGamesRtssRoster.UpdateSeason(); }));
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NhlGamesSummary()
        {
            Dictionary<string, Exception> results = new Dictionary<string, Exception>();

            if (this.IsKeyValid())
            {
                results.Add("NhlGamesSummary", this.Update(delegate() { SportsData.Nhl.NhlGamesSummary.GetNewResultsOnly(saveToDb: true); }));
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NhlGamesRtss()
        {
            Dictionary<string, Exception> results = new Dictionary<string, Exception>();

            if (this.IsKeyValid())
            {
                //results.Add("NhlGamesRtss", this.Update(delegate() { SportsData.Nhl.NhlGamesRtss.GetNewResultsOnly(saveToDb: true); }));
                results.Add("HtmlBlob", this.Update(delegate() { HtmlBlob.UpdateSeason(forceOverwrite: false); }));
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NhlGamesRtssSummary()
        {
            Dictionary<string, Exception> results = new Dictionary<string, Exception>();

            if (this.IsKeyValid())
            {
                results.Add("NhlGamesRtssSummary", this.Update(delegate() { SportsData.Nhl.NhlGamesRtssSummary.UpdateSeason(); }));
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NhlGamesRtssRoster()
        {
            Dictionary<string, Exception> results = new Dictionary<string, Exception>();

            if (this.IsKeyValid())
            {
                results.Add("NhlGamesRtssRoster", this.Update(delegate() { SportsData.Nhl.NhlGamesRtssRoster.UpdateSeason(); }));
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Mlb Attendance data isn't incrementally updated so only use this as needed
        /// </summary>
        [HttpPost]
        public ActionResult MlbAttendance()
        {
            Dictionary<string, Exception> results = new Dictionary<string, Exception>();

            if (this.IsKeyValid())
            {
                results.Add("MlbAttendanceData", this.Update(delegate()
                {
                    int mlbYearToUpdate = DateTime.Now.Year;
                    MlbAttendanceData.UpdateSeason(MlbSeasonType.Spring, mlbYearToUpdate);
                    MlbAttendanceData.UpdateSeason(MlbSeasonType.Regular, mlbYearToUpdate);
                    MlbAttendanceData.UpdateSeason(MlbSeasonType.PostSeason, mlbYearToUpdate);
                }));
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Social()
        {
            Dictionary<string, Exception> results = new Dictionary<string, Exception>();

            if (this.IsKeyValid())
            {
                results.Add("FacebookData", this.Update(delegate() { FacebookData.UpdateAllSnapshotsInDb(); }));
                results.Add("TwitterData", this.Update(delegate() { TwitterData.UpdateAllSnapshotsInDb(); }));
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Executes the action and captures any exception thrown from it
        /// </summary>
        private Exception Update(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                return e;
            }

            return null;
        }
    }
}
