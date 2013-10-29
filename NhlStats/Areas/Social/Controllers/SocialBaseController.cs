using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

using SportsData;
using SportsData.Social;

using SportsData.Controllers;

namespace SportsData.Areas.Social.Controllers
{
    public class SocialBaseController : SportsDataController
    {
        protected ActionResult IndexPost<T>(string ControllerName)
        {
            bool performUpdate = (this.Request["Update"] == "Get Latest");
            
            if (performUpdate)
            {
                MethodInfo m = typeof(T).GetMethod("UpdateAllSnapshotsInDb", BindingFlags.Public | BindingFlags.Static);
                m.Invoke(null, null);
            }

            return RedirectToAction("Index", ControllerName, new { update = performUpdate });
        }

        protected ActionResult UpdateGet(string ControllerName)
        {
            return RedirectToAction("Index", ControllerName, new { update = true });
        }

    }
}
