using System.Web.Mvc;

namespace SportsData.Areas.Social
{
    public class SocialAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Social";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Social_default",
                "Social/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
