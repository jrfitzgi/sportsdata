using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HtmlAgilityPack;
using SportsData.Nhl;
using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class NhlDraftbookTests
    {
        [TestMethod]
        public void NhlDraftBook_Update()
        {
            string fileName = @"C:\Users\Jordanf\Google Drive\Domi\Draft\NHL Draftbook 2013-1963 (V2.0).csv";
            List<Nhl_Draftbook> results = NhlDraftbook.UpdateDraftbook(fileName, false);
            Assert.AreEqual(10296, results.Count);
        }
    }
}
