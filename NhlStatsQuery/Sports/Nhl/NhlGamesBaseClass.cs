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
    public abstract class NhlGamesBaseClass : NhlBaseClass
    {
        protected override DateTime ParseDateFromHtmlRow(HtmlNode row)
        {
            HtmlNodeCollection tdNodes = row.SelectNodes(@"./td");
            return Convert.ToDateTime(tdNodes[0].InnerText.Replace("'", "/"));
        }
        
        /// <summary>
        /// Extract the items that have the special case of the FLA/NSH double header on 9/16/2013
        /// </summary>
        protected static IEnumerable<Nhl_Games_BaseModel> GetSpecialCaseModels(IEnumerable<Nhl_Games_BaseModel> models)
        {
            Func<Nhl_Games_BaseModel, bool> specialCasePredicate = new Func<Nhl_Games_BaseModel, bool>(m => m.Date == Convert.ToDateTime("9/16/2013") && m.Home.Equals("Florida", StringComparison.InvariantCultureIgnoreCase));
            IEnumerable<Nhl_Games_BaseModel> specialCaseModels = models.Where(specialCasePredicate);

            return specialCaseModels;
        }

    }
}
