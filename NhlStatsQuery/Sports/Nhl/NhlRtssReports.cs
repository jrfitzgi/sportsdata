using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HtmlAgilityPack;
using SportsData.Models;

namespace SportsData.Nhl
{
    public partial class NhlRtssReport : NhlBaseClass
    {
        // Figure out number of pages
        // Get all pages into a blob
        // Save collection

        protected override string RelativeUrlFormatString
        {
            get
            {
                // eg. /ice/gamestats.htm?season=2014&gameType=1&viewName=teamRTSSreports&sort=date&pg=2
                return "/ice/gamestats.htm?season={0}&gameType={1}&viewName=teamRTSSreports&sort=date&pg={2}";
            }
        }

    }
}
