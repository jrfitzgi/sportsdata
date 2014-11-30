using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using HtmlAgilityPack;
using SportsData.Nhl;
using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class NhlGamesRtssSummaryTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void NhlGamesRtssSummary_ParseHtmlBlob()
        {

            //string htmlBlob = HtmlBlob.RetrieveBlob(HtmlBlobType.NhlRoster, "1", new Uri("http://www.nhl.com/scores/htmlreports/20132014/GS030315.HTM"), true);
            System.IO.StreamReader myFile =
               new System.IO.StreamReader(@"C:\coding\sportsdata\SportsDataTests\Sports\GameSummary1.html");
            string htmlBlob = myFile.ReadToEnd();

            myFile.Close();
                        
            Nhl_Games_Rtss_Summary report = NhlGamesRtssSummary.ParseHtmlBlob(1, htmlBlob);


        }
    }
}
