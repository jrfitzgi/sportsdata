﻿using System.Web.Mvc;

namespace SportsData.Areas.Attendance
{
    public class AttendanceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Attendance";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Attendance_default",
                "Attendance/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
