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


        [HttpPost]
        public ActionResult Index()
        {
            Guid key = new Guid("5774B680-047B-47EB-B465-3DBF946C3E7A");
            Guid submittedKey = Guid.Parse(this.Request["Key"]);
            bool keyIsValid = (key == submittedKey);

            Dictionary<string, Exception> results = new Dictionary<string, Exception>();

            if (keyIsValid)
            {
                results.Add("FacebookData", this.Update(delegate() { FacebookData.UpdateAllSnapshotsInDb(); }));
                results.Add("TwitterData", this.Update(delegate() { TwitterData.UpdateAllSnapshotsInDb(); }));
                results.Add("NhlGameSummary", this.Update(delegate() { NhlGameSummary.UpdateSeasonWithLatestOnly(); }));
                results.Add("NhlRtssReport", this.Update(delegate() { NhlRtssReport.UpdateSeasonWithLatestOnly(); }));
                results.Add("NhlHtmlReportSummary", this.Update(delegate() { NhlHtmlReportSummary.UpdateSeason(); }));
                results.Add("NhlHtmlReportRoster", this.Update(delegate() { NhlHtmlReportRoster.UpdateSeason(); }));

                // Mlb data doesn't incrementally update so don't do it every time this controller action is called
                //results.Add("MlbAttendanceData", this.Update(delegate()
                //{
                //    int mlbYearToUpdate = DateTime.Now.Year;
                //    MlbAttendanceData.UpdateSeason(MlbSeasonType.Spring, mlbYearToUpdate);
                //    MlbAttendanceData.UpdateSeason(MlbSeasonType.Regular, mlbYearToUpdate);
                //    MlbAttendanceData.UpdateSeason(MlbSeasonType.PostSeason, mlbYearToUpdate);
                //}));
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

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
