using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HtmlAgilityPack;
using SportsData;
using SportsData.Models;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("SportsDataTests")]

namespace SportsData.Nhl
{
    public abstract class NhlBaseClass
    {
        public const string BaseAddress = "http://www.nhl.com";

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
        /// 
        /// </remarks>
        protected abstract string RelativeUrlFormatString
        {
            get;
        }

        /// <summary>
        /// Gets a list of all the html tables in a stat category
        /// </summary>
        public virtual List<HtmlNode> GetStatPages(int year, NhlSeasonType nhlSeasonType)
        {
            HtmlNode firstPage = this.GetHtmlTableNode(year, nhlSeasonType, 1);

            // Get number of pages
            int numberOfPages = NhlBaseClass.GetPageCount(firstPage);

            List<HtmlNode> pages = new List<HtmlNode>();
            pages.Add(firstPage);
            for (int i = 2; i < numberOfPages+1; i++)
            {
                pages.Add(this.GetHtmlTableNode(year, nhlSeasonType, i));
            }

            return pages;
        }

        /// <summary>
        /// Gets the html table of a page specified by the xpath query
        /// </summary>
        /// TODO: make this protected when done testing
        public virtual HtmlNode GetHtmlTableNode(int year, NhlSeasonType nhlSeasonType, int page)
        {
            string pageHtml = this.GetPage(year, nhlSeasonType, page);
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(pageHtml);

            string tableXPathQuery = @"//table[@class='data stats']";
            HtmlNode tableNode = htmlDocument.DocumentNode.SelectSingleNode(tableXPathQuery);
            return tableNode;
        }

        /// <summary>
        /// Gets a page and returns the full html
        /// </summary>
        protected virtual string GetPage(int year, NhlSeasonType nhlSeasonType, int page)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(NhlBaseClass.BaseAddress);

            string relativeAddress = String.Format(this.RelativeUrlFormatString, year, Convert.ToInt32(nhlSeasonType), page);
            Uri pageUrl = new Uri(relativeAddress, UriKind.Relative);

            Task<string> response = httpClient.GetStringAsync(pageUrl);
            string responseString = response.Result;

            return responseString;
        }

        #region Static Methods

        /// <summary>
        /// Given a <![CDATA[<table>]]> element, pull out the header names from the <![CDATA[<th>]]>
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        /// TODO: make this protected when done testing
        public static List<string> GetHeaderNames(HtmlNode table)
        {
            HtmlNodeCollection headerColumnNodes = table.SelectNodes(@"//thead/tr/th");

            return headerColumnNodes.Select(n => n.InnerText.RemoveSpecialWhitespaceCharacters()).ToList();
        }

        /// <summary>
        /// Counts the number of rows in an html table
        /// </summary>
        protected static int GetRowCountInTable(HtmlNode table)
        {
            HtmlNodeCollection rows = table.SelectNodes("./tbody/tr");

            // Verify that the tables contain rows
            if (null == rows)
            {
                // This is unexpected but could occur
                return 0;
            }

            return rows.Count;
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
        /// Remove the footer from the stats table
        /// </summary>
        protected static void RemoveFooter(HtmlNode tableNode)
        {
            HtmlNode footerNode = tableNode.SelectSingleNode(@"//tfoot");
            tableNode.RemoveChild(footerNode);
        }

        #endregion

    }
}
