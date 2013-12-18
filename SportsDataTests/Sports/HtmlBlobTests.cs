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
            string id = (new Guid()).ToString();
            string result = HtmlBlob.GetHtmlPage(url);

            Assert.IsFalse(HtmlBlob.BlobExists(HtmlBlobType.NhlRoster, id, url));

            HtmlBlob.SaveBlob(HtmlBlobType.NhlRoster, id, url, result);
            string downloadedBlob = HtmlBlob.RetrieveBlob(HtmlBlobType.NhlRoster, id, url);

            Assert.IsTrue(HtmlBlob.BlobExists(HtmlBlobType.NhlRoster, id, url));
            
        }
    }
}
