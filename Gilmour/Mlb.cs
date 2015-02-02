using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using SportsData;
using SportsData.Mlb;
using SportsData.Models;

namespace Gilmour
{
    public class Mlb
    {
        private static bool useLocalhost = false;

        public static string GetMlbAttendance()
        {
            //int mlbYearToUpdate = DateTime.Now.Year;
            //MlbAttendanceData.UpdateSeason(MlbSeasonType.Spring, mlbYearToUpdate);
            //MlbAttendanceData.UpdateSeason(MlbSeasonType.Regular, mlbYearToUpdate);
            //MlbAttendanceData.UpdateSeason(MlbSeasonType.PostSeason, mlbYearToUpdate);

            return WebUpdate.Update("MlbAttendance", useLocalhost);
        }
    }
}
