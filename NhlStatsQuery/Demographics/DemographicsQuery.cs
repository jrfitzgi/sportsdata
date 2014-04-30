using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;
using SportsData.Models;

namespace SportsData.Demographics
{
    public abstract class DemographicsQuery
    {
        protected string BaseAddress = "http://zipwho.com/";
        protected string PageFormatString = "?zip={0}&mode=zip";

        public static List<DemographicsModel> GetDemographics(List<int> zipCodes)
        {
            List<DemographicsModel> results = new List<DemographicsModel>();

            DemographicsModel result = null;
            foreach (int zipCode in zipCodes)
            {
                result = DemographicsQuery.GetDemographic(zipCode);
                if (null != result)
                {
                    results.Add(result);
                }
            }

            return results;
        }

        public static DemographicsModel GetDemographic(int zipCodes)
        {
            DemographicsModel result = new DemographicsModel();


            return result;
        }

    }
}
