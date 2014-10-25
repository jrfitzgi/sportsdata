﻿using System;
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
    public class NhlPlayerStatsBioGoalieTests : SportsDataTestsBaseClass
    {
        [TestMethod]
        public void NhlPlayerStatsBioGoalie_GetFullSeason()
        {
            int year = 2013;
            List<NhlPlayerStatsBioGoalieModel> results = NhlPlayerStatsBioGoalie.GetFullSeason(year, false);
            Assert.AreEqual(105, results.Count);
        }
    }
}