﻿using System;
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
        /// Get the RtssReports for the specified year
        /// </summary>
        public static List<NhlRtssReportModel> GetRtssReports([Optional] int year, [Optional] DateTime fromDate)
        {
            year = NhlGameStatsBaseModel.SetDefaultYear(year);

            List<NhlRtssReportModel> models;
            using (SportsDataContext db = new SportsDataContext())
            {
                models = (from m in db.NhlRtssReports
                          where
                             m.Year == year &&
                             m.Date >= fromDate
                          select m).ToList();
            }

            return models;
        }
    }
}
