using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HtmlAgilityPack;
using SportsData.Models;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("SportsDataTests")]

namespace SportsData.Nhl
{
    public abstract class NhlGameStatsBaseClass : NhlBaseClass
    {
        protected override DateTime ParseDateFromHtmlRow(HtmlNode row)
        {
            HtmlNodeCollection tdNodes = row.SelectNodes(@"./td");
            return Convert.ToDateTime(tdNodes[0].InnerText.Replace("'", "/"));
        }
        
        /// <summary>
        /// Extract the items that have the special case of the FLA/NSH double header on 9/16/2013
        /// </summary>
        protected static IEnumerable<NhlGameStatsBaseModel> GetSpecialCaseModels(IEnumerable<NhlGameStatsBaseModel> models)
        {
            Func<NhlGameStatsBaseModel, bool> specialCasePredicate = new Func<NhlGameStatsBaseModel, bool>(m => m.Date == Convert.ToDateTime("9/16/2013") && m.Home.Equals("Florida", StringComparison.InvariantCultureIgnoreCase));
            IEnumerable<NhlGameStatsBaseModel> specialCaseModels = models.Where(specialCasePredicate);

            return specialCaseModels;
        }

    }
}
