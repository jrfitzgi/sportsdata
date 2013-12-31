using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
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
        /// Get the RtssReports for the specified year
        /// </summary>
        public static List<NhlRtssReportModel> GetRtssReports(int year)
        {
            List<NhlRtssReportModel> models;
            using (SportsDataContext db = new SportsDataContext())
            {
                models = (from m in db.NhlRtssReports
                          where
                             m.Year == year
                          select m).ToList();
            }

            return models;
        }
    }
}

