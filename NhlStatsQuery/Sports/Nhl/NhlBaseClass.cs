using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
        /// 
        /// </remarks>
        protected abstract string RelativeUrlFormatString
        {
            get;
        }

        protected abstract Type ModelType
        {
            get;
        }

        protected abstract NhlGameStatsBaseModel MapHtmlRowToModel(HtmlNode row, NhlSeasonType nhlSeasonType);

        /// <summary>
        /// Update the db with the results in the list.
        /// </summary>
        /// <remarks>
        /// When this is overridden, the List<NhlGameStatsBaseModel> must be converted to List<T> where T is the model type that the subclass uses.
        /// </remarks>
        protected abstract void AddOrUpdateDb(List<NhlGameStatsBaseModel> models);

        protected abstract void UpdateSeason_protected(int year);

        #endregion

        #region Public Methods

        public List<NhlGameStatsBaseModel> GetSeason(int year)
        {
            List<NhlGameStatsBaseModel> results = new List<NhlGameStatsBaseModel>();

            List<NhlSeasonType> nhlSeasonTypes = new List<NhlSeasonType> { NhlSeasonType.PreSeason, NhlSeasonType.RegularSeason, NhlSeasonType.Playoff };
            foreach (NhlSeasonType nhlSeasonType in nhlSeasonTypes)
            {
                List<HtmlNode> htmlTables = this.GetAllPagesForSeasonType(year, nhlSeasonType);

                List<HtmlNode> rows = new List<HtmlNode>();
                htmlTables.ForEach(t => rows.AddRange(NhlBaseClass.ParseRowsFromTable(t)));

                foreach (HtmlNode row in rows)
                {
                    results.Add(this.MapHtmlRowToModel(row, nhlSeasonType));
                }
            }

            return results;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets a list of all the html tables in a stat category
        /// </summary>
        protected virtual List<HtmlNode> GetAllPagesForSeasonType(int year, NhlSeasonType nhlSeasonType)
        {
            HtmlNode firstPage = this.ParseHtmlTableFromPage(year, nhlSeasonType, 1);

            int numberOfResults = NhlBaseClass.GetResultsCount(firstPage);
            if (numberOfResults <= 0)
            {
                return new List<HtmlNode>();
            }

            // Get number of pages
            int numberOfPages = NhlBaseClass.GetPageCount(firstPage);

            List<HtmlNode> pages = new List<HtmlNode>();
            pages.Add(firstPage);
            for (int i = 2; i < numberOfPages + 1; i++)
            {
                pages.Add(this.ParseHtmlTableFromPage(year, nhlSeasonType, i));
            }

            return pages;
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

        protected virtual void CheckType<T>()
        {
            // Check that T is of the right type
            if (typeof(T) != this.ModelType)
            {
                throw new ArgumentException("T must be of type " + this.ModelType.ToString());
            }
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
        /// Given a <![CDATA[<table>]]> element, pull out the header names from the <![CDATA[<th>]]>
        /// </summary>
        /// TODO: make this protected when done testing
        protected static List<string> GetHeaderNames(HtmlNode table)
        {
            HtmlNodeCollection headerColumnNodes = table.SelectNodes(@"//thead/tr/th");
            return headerColumnNodes.Select(n => n.InnerText.RemoveSpecialWhitespaceCharacters()).ToList();
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

        #region Unused Code

        //public abstract T MapHtmlRowToModel<T>(HtmlNode row, NhlSeasonType nhlSeasonType) where T : NhlGameStatsBaseModel;

        //public List<T> GetSeason<T>(int year) where T:NhlGameStatsBaseModel
        //{
        //    List<HtmlNode> rows = new List<HtmlNode>();
        //    rows.AddRange(this.GetAllPagesForSeasonType(year, NhlSeasonType.PreSeason));
        //    rows.AddRange(this.GetAllPagesForSeasonType(year, NhlSeasonType.RegularSeason));
        //    rows.AddRange(this.GetAllPagesForSeasonType(year, NhlSeasonType.Playoff));

        //    List<T> results = new List<T>();
        //    foreach (HtmlNode row in rows)
        //    {
        //         results.Add(this.MapHtmlRowToModel<T>(row, NhlSeasonType.None));
        //    }

        //    return results;
        //}

        #endregion

    }
}
