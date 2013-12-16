using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HtmlAgilityPack;
using SportsData;
using SportsData.Nhl;
using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class HtmlBlobTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void HtmlBlobTest()
        {
            Uri url = new Uri("http://www.nhl.com/scores/htmlreports/20132014/RO020502.HTM");
            string result = HtmlBlob.GetHtmlPage(url);

            //Assert.IsFalse(HtmlBlobType.NhlRoster, HtmlBlob.BlobExists(url));

            HtmlBlob.SaveAsBlob(HtmlBlobType.NhlRoster, url, result);
            string downloadedBlob = HtmlBlob.RetrieveBlob(HtmlBlobType.NhlRoster, url);

            Assert.IsTrue(HtmlBlob.BlobExists(HtmlBlobType.NhlRoster, url));
            
        }
    }
}
