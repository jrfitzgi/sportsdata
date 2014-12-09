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
    public abstract class NhlBaseClass
    {
        public const string BaseAddress = "http://www.nhl.com";

        #region Abstract Methods

        /// <summary>
        /// Implement this property in subclasses
        /// </summary>
        /// <remarks>
        /// Params in the format string must be in the following order:
        /// 0. year (the latest of the two years in a season, eg. use 2014 for the 2013-2014 season)
        /// 1. season type
        /// 2. page number
        /// 
        /// Example: "/ice/gamestats.htm?season={0}&gameType={1}&viewName=teamRTSSreports&sort=date&pg={2}"
        ///           /ice/playerstats.htm?season=20122013&gameType=3&viewName=bios&pg=3
        /// </remarks>
        protected abstract string RelativeUrlFormatString
        {
            get;
        }

        ///// <summary>
        ///// Update the db with the results from specified year and date
        ///// </summary>
        //protected abstract void AddOrUpdateDb(int year, [Optional] DateTime fromDate);

        /// <summary>
        /// Implemented by the subclass to parse out the date of the result
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <returns></returns>
        protected abstract DateTime ParseDateFromHtmlRow(HtmlNode row);

        #endregion

        #region Public Methods

        public List<HtmlNode> GetResultsForSeason([Optional] int year, [Optional] DateTime fromDate)
        {
            List<HtmlNode> results = new List<HtmlNode>();

            // The season types to collect results for
            List<NhlSeasonType> nhlSeasonTypes = new List<NhlSeasonType> { NhlSeasonType.PreSeason, NhlSeasonType.RegularSeason, NhlSeasonType.Playoff };

            foreach (NhlSeasonType nhlSeasonType in nhlSeasonTypes)
            {
                results.AddRange(this.GetResultsForSeasonType(year, nhlSeasonType, fromDate));
            }

            return results;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets a list of all the results in a stat category on fromDate and later
        /// </summary>
        /// <remarks>
        /// This method assumes that rows are sorted in descending order by date (newest to oldest).
        ///
        /// year defaults to the current year
        /// fromDate defaults to DateTime.MinValue
        /// 
        /// </remarks>
        protected virtual List<HtmlNode> GetResultsForSeasonType([Optional] int year, NhlSeasonType nhlSeasonType, [Optional] DateTime fromDate)
        {
            year = NhlModelHelper.SetDefaultYear(year);

            List<HtmlNode> results = new List<HtmlNode>();

            HtmlNode firstPageTableNode = this.ParseHtmlTableFromPage(year, nhlSeasonType, 1);

            int numberOfResults = NhlBaseClass.GetResultsCount(firstPageTableNode);
            if (numberOfResults <= 0)
            {
                return results;
            }

            int numberOfPages = NhlBaseClass.GetPageCount(firstPageTableNode);

            // Handle the first page. Go through each row and add it to the list of results. When we encounter a result with a date earlier than fromDate then we stop.
            List<HtmlNode> firstPageRows = NhlBaseClass.ParseRowsFromTable(firstPageTableNode);
            foreach (HtmlNode row in firstPageRows)
            {
                DateTime resultDate = this.ParseDateFromHtmlRow(row);
                if (resultDate < fromDate) { return results; }
                else { results.Add(row); }
            }

            // Now similar code to handle the rest of the pages. Go through each row, add it to the list, stop when we hit a date prior to fromDate.
            for (int i = 2; i < numberOfPages + 1; i++)
            {
                HtmlNode tableNode = this.ParseHtmlTableFromPage(year, nhlSeasonType, i);

                List<HtmlNode> rows = NhlBaseClass.ParseRowsFromTable(tableNode);
                foreach (HtmlNode row in rows)
                {
                    DateTime resultDate = this.ParseDateFromHtmlRow(row);
                    if (resultDate < fromDate) { return results; }
                    else { results.Add(row); }
                }

            }

            return results;
        }

        /// <summary>
        /// Gets the html table of a page specified by the xpath query
        /// </summary>
        protected virtual HtmlNode ParseHtmlTableFromPage(int year, NhlSeasonType nhlSeasonType, int page)
        {
            string pageHtml = this.GetFullHtmlPage(year, nhlSeasonType, page);
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(pageHtml);

            string tableXPathQuery = @"//table[@class='data stats']";
            HtmlNode tableNode = htmlDocument.DocumentNode.SelectSingleNode(tableXPathQuery);
            return tableNode;
        }

        /// <summary>
        /// Gets a page and returns the full html
        /// </summary>
        protected virtual string GetFullHtmlPage(int year, NhlSeasonType nhlSeasonType, int page)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(NhlBaseClass.BaseAddress);

            string relativeAddress = String.Format(this.RelativeUrlFormatString, year, Convert.ToInt32(nhlSeasonType), page);
            Uri pageUrl = new Uri(relativeAddress, UriKind.Relative);

            Task<string> response = httpClient.GetStringAsync(pageUrl);
            string responseString = response.Result;

            return responseString;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Given a <![CDATA[<table>]]> element, pull out the <![CDATA[<tr>]]> rows
        /// </summary>
        protected static List<HtmlNode> ParseRowsFromTable(HtmlNode table)
        {
            HtmlNodeCollection rowNodes = table.SelectNodes(@"./tbody/tr");
            return rowNodes.ToList();
        }

        /// <summary>
        /// Finds the number of pages stat category
        /// </summary>
        protected static int GetPageCount(HtmlNode tableNode)
        {
            int resultsCount = NhlBaseClass.GetResultsCount(tableNode);

            // There are 30 results per page, so divide number of results by 30
            double numPages = Math.Ceiling(Convert.ToDouble(resultsCount) / 30);

            return Convert.ToInt32(numPages);
        }

        /// <summary>
        /// Finds the total number of results (records) in a stat category
        /// </summary>
        protected static int GetResultsCount(HtmlNode tableNode)
        {
            // Find the string that states the total number of results (rows) in the stat category
            // Parse out the total number of results

            string query = @"//div[@class = 'numRes']";
            HtmlNode numResults = tableNode.SelectSingleNode(query);

            if (null == numResults)
            {
                return 0;
            }
            else
            {
                string tempString = Regex.Match(numResults.InnerText, @"\d+ results").Value;
                tempString = Regex.Match(tempString, @"\d+").Value;
                return Convert.ToInt32(tempString);
            }
        }

        /// <summary>
        /// Convert minutes in mm:ss format to seconds
        /// </summary>
        public static int ConvertMinutesToSeconds(string text)
        {
            string[] parts = text.Split(':');

            if (null == parts || parts.Length == 0)
            {
                return 0;
            }
            else if (parts.Length == 1)
            {
                return NhlBaseClass.ConvertStringToInt(parts[0]);
            }
            else if (parts.Length == 2)
            {
                int minutes = NhlBaseClass.ConvertStringToInt(parts[0]);
                int seconds = NhlBaseClass.ConvertStringToInt(parts[1]);
                return (minutes * 60) + seconds;
            }
            else
            {
                throw new ArgumentException(String.Format("The string {0} could not be converted to seconds", text));
            }

        }

        public static int ConvertStringToPeriod(string s)
        {
            s = NhlBaseClass.RemoveAllWhitespace(s);
            if (s.Equals("OT", StringComparison.InvariantCultureIgnoreCase))
            {
                return 4;
            }
            else
            {
                return NhlBaseClass.ConvertStringToInt(s);
            }
        }

        public static int ConvertStringToInt(string s)
        {
            int result;
            bool success = Int32.TryParse(s, out result);

            if (!success)
            {
                return 0;
            }
            else
            {
                return result;
            }
        }

        public static void ParseNameText(string nameText, out int number, out string name)
        {
            nameText = NhlBaseClass.RemoveWhitespaceCharacters(nameText);
            nameText = nameText.Trim();

            Regex regex = new Regex(@"(?<number>\d+)(?<name>.*)");

            string numberString = regex.Match(nameText).Groups["number"].Value;
            if (String.IsNullOrWhiteSpace(numberString))
            {
                number = 0;
                name = nameText.Trim();
            }
            else
            {
                number = Convert.ToInt32(numberString);
                name = regex.Match(nameText).Groups["name"].Value.Trim();
            }
        }

        public static string RemoveAllWhitespace(string text)
        {
            string result = text;
            result = result.Replace(" ", String.Empty);
            result = NhlBaseClass.RemoveWhitespaceCharacters(text);

            return result;
        }

        /// <summary>
        /// Removes \n, \t, \r but not spaces
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveWhitespaceCharacters(string text)
        {
            string result = text;
            result = result.Replace("\n", String.Empty);
            result = result.Replace("\t", String.Empty);
            result = result.Replace("\r", String.Empty);

            return result;
        }
        #endregion

        #region Unused Code

        ///// <summary>
        ///// Counts the number of rows in an html table
        ///// </summary>
        //protected static int GetRowCountInTable(HtmlNode table)
        //{
        //    HtmlNodeCollection rows = table.SelectNodes("./tbody/tr");

        //    // Verify that the tables contain rows
        //    if (null == rows)
        //    {
        //        // This is unexpected but could occur
        //        return 0;
        //    }

        //    return rows.Count;
        //}

        ///// <summary>
        ///// Given a <![CDATA[<table>]]> element, pull out the header names from the <![CDATA[<th>]]>
        ///// </summary>
        ///// TODO: make this protected when done testing
        //protected static List<string> GetHeaderNames(HtmlNode table)
        //{
        //    HtmlNodeCollection headerColumnNodes = table.SelectNodes(@"//thead/tr/th");
        //    return headerColumnNodes.Select(n => n.InnerText.RemoveSpecialWhitespaceCharacters()).ToList();
        //}

        ///// <summary>
        ///// Remove the footer from the stats table
        ///// </summary>
        //protected static void RemoveFooter(HtmlNode tableNode)
        //{
        //    HtmlNode footerNode = tableNode.SelectSingleNode(@"//tfoot");
        //    tableNode.RemoveChild(footerNode);
        //}

        #endregion

    }
}
