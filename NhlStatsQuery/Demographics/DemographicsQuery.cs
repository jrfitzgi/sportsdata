using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HtmlAgilityPack;
using SportsData.Models;

namespace SportsData.Demographics
{
    public abstract class DemographicsQuery
    {
        protected static string BaseAddress = "http://zipwho.com/";
        protected static string RelativeUrlFormatString = "?zip={0}&mode=zip";

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

        public static DemographicsModel GetDemographic(int zipCode)
        {
            string page = DemographicsQuery.GetPage(zipCode);
            DemographicsModel result = DemographicsQuery.ParsePage(page);
            return result;
        }

        private static string GetPage(int zipCode)
        {
            string relativeUrlString = String.Format(DemographicsQuery.RelativeUrlFormatString, zipCode);
            Uri relativeUrl = new Uri(relativeUrlString, UriKind.Relative);

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(DemographicsQuery.BaseAddress);

            Task<string> httpResponseMessage = httpClient.GetStringAsync(relativeUrl);
            string responseString = httpResponseMessage.Result;
            return responseString;
        }

        private static DemographicsModel ParsePage(string page)
        {
            // Get the csv list of fields and values. This needs to be done in two steps until a better regex can be made.
            string pattern = @"getData\(\)(?<text>.*?)"";";
            Regex regex = new Regex(pattern, RegexOptions.Singleline);
            Match match = regex.Match(page);
            string partialText = match.Groups["text"].Value;

            pattern = @"""(?<fields>.*?)\\n(?<values>.*?)$";
            regex = new Regex(pattern, RegexOptions.Singleline);
            match = regex.Match(partialText);
            string fieldsCsv = match.Groups["fields"].Value;
            string valuesCsv = match.Groups["values"].Value;

            string[] fields = fieldsCsv.Split(',');
            string[] values = valuesCsv.Split(',');
            
            // Fields and values should align by their index. Put them into a dictionary.
            Dictionary<string, string> data = new Dictionary<string, string>();
            for (int i=0; i<fields.Length; i++)
            {
                data.Add(fields[i], values[i]);
            }

            return null;
        }
    }
}
