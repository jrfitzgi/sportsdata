using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HtmlAgilityPack;
using SportsData;
using SportsData.Models;

namespace SportsData.Nhl
{
    public class NhlHtmlReportBase
    {
        /// <summary>
        /// Set this to "/tbody" if testing a downloaded file that contains the <tbody> tag
        /// </summary>
        public static string TBODY = "";

        /// <summary>
        /// Get the RtssReports for the specified year
        /// </summary>
        public static List<Nhl_Games_Rtss> GetRtssReports([Optional] int year, [Optional] DateTime fromDate)
        {
            year = NhlModelHelper.SetDefaultYear(year);

            List<Nhl_Games_Rtss> models;
            using (SportsDataContext db = new SportsDataContext())
            {
                models = (from m in db.Nhl_Games_Rtss_DbSet
                          where
                             m.Year == year &&
                             m.Date >= fromDate
                          select m).ToList();
            }

            return models;
        }
    }
}

