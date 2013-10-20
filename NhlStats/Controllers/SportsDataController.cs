using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsData.Controllers
{
    public class SportsDataController : Controller
    {
        public ActionResult Result<T>(T model)
        {
            string acceptHeader = this.Request.Headers["Accept"];
            if (acceptHeader.IndexOf("application/json", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View(model);
            }
        }
    }
}
