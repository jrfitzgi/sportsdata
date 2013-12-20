using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;
using SportsData;
using SportsData.Models;

namespace SportsData.Nhl
{
    public class NhlHtmlReportRoster
    {
        public static NhlHtmlReportRosterModel ParseHtmlBlob(string html)
        {
            NhlHtmlReportRosterModel model = new NhlHtmlReportRosterModel();

            return model;
        }

    }
}
