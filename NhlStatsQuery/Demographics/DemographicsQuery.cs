using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HtmlAgilityPack;
using SportsData.Models;

namespace SportsData.Demographics
{
    public abstract class DemographicsQuery
    {
        protected static string BaseAddress = "http://zipwho.com/";
        protected static string RelativeUrlFormatString = "?zip={0}&mode=zip";

        /// <summary>
        /// Get the demographics data for all zipcodes in the list
        /// </summary>
        /// <param name="zipCodes"></param>
        /// <param name="saveToDb">Save each demographic as it is retrieved</param>
        /// <param name="randomWaitSeconds">Sleep for between 0 and randomWaitSeconds between each request</param>
        public static List<DemographicsModel> GetDemographics(List<int> zipCodes, [Optional] bool saveToDb, [Optional] int randomWaitSeconds)
        {
            List<DemographicsModel> results = new List<DemographicsModel>();

            DemographicsModel result = null;
            foreach (int zipCode in zipCodes)
            {
                // Sleep for a random number of seconds between 0 and randomWaitSeconds. Default is 0.
                Random random = new Random();
                int seconds = random.Next(0, randomWaitSeconds);
                System.Threading.Thread.Sleep(seconds * 1000);

                result = DemographicsQuery.GetDemographic(zipCode);
                if (null != result)
                {
                    results.Add(result);
                    DemographicsData.UpdateDatabase(new List<DemographicsModel> {result});
                }
                else
                {
                    // We hit a ? and are blocked
                    break;
                }
            }

            return results;
        }

        public static DemographicsModel GetDemographic(int zipCode)
        {
            Assert.IsTrue(zipCode <= 99999, "Zip Code {0} cannot be more than 5 digits", zipCode);

            string page = DemographicsQuery.GetPage(zipCode);
            if (page.Equals("?"))
            {
                Console.WriteLine("Page for {0} contains '?'", zipCode);
                return null;
            }

            DemographicsModel result = DemographicsQuery.ParsePage(page, zipCode);
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

        private static DemographicsModel ParsePage(string page, int zipCode)
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

            // Now populate the object with data from the dictionary
            DemographicsModel result = new DemographicsModel();
            result.Zip = zipCode;

            // Some of the data is empty so this condition will check if it is populated or not.
            if (!regex.IsMatch(partialText))
            {
                Console.WriteLine("No data found for {0}", zipCode);
            }
            else
            {
                result.City = data["city"];
                result.State = data["state"];
                result.MedianIncome = Convert.ToInt32(data["MedianIncome"]);
                result.MedianIncomeRank = Convert.ToInt32(data["MedianIncomeRank"]);
                result.CostOfLivingIndex = Convert.ToDouble(data["CostOfLivingIndex"]);
                result.CostOfLivingRank = Convert.ToInt32(data["CostOfLivingRank"]);
                result.MedianMortgageToIncomeRatio = Convert.ToDouble(data["MedianMortgageToIncomeRatio"]);
                result.MedianMortgageToIncomeRank = Convert.ToInt32(data["MedianMortgageToIncomeRank"]);
                result.OwnerOccupiedHomesPercent = Convert.ToDouble(data["OwnerOccupiedHomesPercent"]);
                result.OwnerOccupiedHomesRank = Convert.ToInt32(data["OwnerOccupiedHomesRank"]);
                result.MedianRoomsInHome = Convert.ToDouble(data["MedianRoomsInHome"]);
                result.MedianRoomsInHomeRank = Convert.ToInt32(data["MedianRoomsInHomeRank"]);
                result.CollegeDegreePercent = Convert.ToDouble(data["CollegeDegreePercent"]);
                result.CollegeDegreeRank = Convert.ToInt32(data["CollegeDegreeRank"]);
                result.ProfessionalPercent = Convert.ToDouble(data["ProfessionalPercent"]);
                result.ProfessionalRank = Convert.ToInt32(data["ProfessionalRank"]);
                result.Population = Convert.ToInt32(data["Population"]);
                result.PopulationRank = Convert.ToInt32(data["PopulationRank"]);
                result.AverageHouseholdSize = Convert.ToDouble(data["AverageHouseholdSize"]);
                result.AverageHouseholdSizeRank = Convert.ToInt32(data["AverageHouseholdSizeRank"]);
                result.MedianAge = Convert.ToDouble(data["MedianAge"]);
                result.MedianAgeRank = Convert.ToInt32(data["MedianAgeRank"]);
                result.MaleToFemaleRatio = Convert.ToDouble(data["MaleToFemaleRatio"]);
                result.MaleToFemaleRank = Convert.ToInt32(data["MaleToFemaleRank"]);
                result.MarriedPercent = Convert.ToDouble(data["MarriedPercent"]);
                result.MarriedRank = Convert.ToInt32(data["MarriedRank"]);
                result.DivorcedPercent = Convert.ToDouble(data["DivorcedPercent"]);
                result.DivorcedRank = Convert.ToInt32(data["DivorcedRank"]);
                result.WhitePercent = Convert.ToDouble(data["WhitePercent"]);
                result.WhiteRank = Convert.ToInt32(data["WhiteRank"]);
                result.BlackPercent = Convert.ToDouble(data["BlackPercent"]);
                result.BlackRank = Convert.ToInt32(data["BlackRank"]);
                result.AsianPercent = Convert.ToDouble(data["AsianPercent"]);
                result.AsianRank = Convert.ToInt32(data["AsianRank"]);
                result.HispanicEthnicityPercent = Convert.ToDouble(data["HispanicEthnicityPercent"]);
                result.HispanicEthnicityRank = Convert.ToInt32(data["HispanicEthnicityRank"]);
            }

            result.LastUpdated = DateTime.UtcNow;
            return result;
        }
    }
}
