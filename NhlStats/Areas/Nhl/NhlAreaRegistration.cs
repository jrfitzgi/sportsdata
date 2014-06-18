using System.Web.Mvc;

namespace SportsData.Areas.Nhl
{
    public class NhlAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Nhl";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Nhl_default",
                "Nhl/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
