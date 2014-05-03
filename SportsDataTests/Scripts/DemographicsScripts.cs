﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SportsData.Demographics;
using SportsData.Models;

namespace SportsDataTests
{
    [TestClass]
    public class DemographicsScripts : SportsDataScriptsBaseClass
    {
        private string zipCsv =
            "99105,99341,99344,99169,99371,99401,99402,99403,99320,99336,99337,99338,99345,99346,99350,99352,99354,99353,98811,98815,98816,98817,98821,98822,98826,98828,98831,98836,98847,98852,98801,98807,98305,98324,98326,98331,98343,98350,98357,98362,98363,98381,98382,98601,98604,98606,98607,98622,98629,98642,98660,98661,98662,98663,98664,98665,98666,98667,98668,98682,98683,98684,98685,98686,98687,98671,98675,99328,99359,98603,98609,98611,98616,98625,98626,98632,98581,98645,98649,98674,98813,98802,98830,98843,98845,98850,98858,99107,99118,99121,99138,99140,99146,99150,99160,99166,99326,99330,99335,99343,99301,99302,99347,99321,99115,99123,98823,98824,99133,99135,98832,99349,98837,98848,99357,98851,98853,98857,98860,98520,98526,98535,98536,98537,98541,98547,98550,98552,98559,98557,98562,98563,98566,98568,98569,98571,98575,98583,98587,98595,98282,98236,98239,98249,98253,98260,98277,98278,98320,98325,98358,98339,98365,98368,98376,98001,98002,98071,98092,98224,98004,98005,98006,98007,98008,98009,98015,98010,98011,98041,98013,98014,98019,98022,98024,98003,98023,98063,98093,98025,98027,98029,98028,98030,98031,98032,98035,98042,98064,98089,98033,98034,98083,98038,98039,98040,98045,98047,98050,98051,98052,98053,98073,98054,98055,98056,98057,98058,98059,98074,98075,98062,98101,98102,98103,98104,98105,98106,98107,98108,98109,98111,98112,98113,98114,98115,98116,98117,98118,98119,98121,98122,98124,98125,98126,98127,98129,98131,98132,98133,98134,98136,98138,98139,98141,98144,98145,98146,98148,98151,98154,98155,98158,98160,98161,98164,98165,98166,98168,98170,98171,98174,98175,98177,98178,98181,98184,98185,98188,98190,98191,98194,98195,98198,98199,98288,98065,98070,98072,98077,98110,98310,98311,98312,98314,98337,98322,98340,98342,98345,98346,98353,98359,98364,98366,98367,98370,98378,98061,98380,98315,98383,98384,98386,98392,98393,98922,98925,98926,98934,98940,98941,98068,98943,98946,98950,98602,99322,98605,98613,98617,98619,98620,98623,98628,98635,99356,98650,98670,98672,98673,98522,98531,98532,98533,98538,98539,98542,98544,98336,98355,98356,98564,98565,98570,98361,98572,98377,98582,98585,98591,98593,98596,99103,99117,99122,99008,99134,99144,99147,99154,99159,99029,99032,99185,98524,98528,98546,98548,98555,98560,98584,98588,98592,98812,98814,98819,99116,99124,98827,98829,98833,98834,99155,98840,98841,98844,98846,98849,98855,98856,98859,98862,98527,98614,98624,98554,98631,98561,98637,98638,98640,98641,98577,98644,98586,98590,99119,99139,99152,99153,99156,99180,98303,98304,98391,98321,98430,98323,98327,98328,98330,98333,98329,98332,98335,98338,98344,98348,98349,98439,98492,98496,98497,98498,98499,98351,98397,98438,98558,98354,98360,98398,98371,98372,98373,98374,98375,98580,98385,98387,98388,98352,98390,98401,98402,98403,98404,98405,98406,98407,98408,98409,98411,98412,98413,98415,98416,98417,98418,98419,98421,98422,98424,98431,98433,98442,98443,98444,98445,98446,98447,98448,98450,98455,98460,98464,98465,98466,98471,98477,98481,98490,98493,98467,98394,98395,98396,98222,98243,98245,98250,98261,98279,98280,98286,98297,98221,98232,98233,98235,98237,98238,98255,98257,98263,98267,98273,98274,98283,98284,98610,98639,98648,98651,98223,98012,98021,98241,98020,98026,98201,98203,98204,98205,98206,98207,98208,98213,98251,98252,98256,98258,98036,98037,98046,98087,98270,98271,98082,98272,98043,98275,98259,98287,98290,98291,98296,98292,98293,98294,99001,99003,99004,99005,99006,99009,99011,99012,99014,99016,99018,99019,99020,99021,99022,99023,99025,99026,99027,99030,99031,99201,99202,99203,99204,99205,99206,99207,99208,99209,99210,99211,99212,99213,99214,99215,99216,99217,99218,99219,99220,99223,99224,99228,99251,99252,99256,99258,99260,99299,99036,99037,99039,99101,99109,99110,99114,99126,99013,99129,99131,99137,99141,99148,99151,99157,99167,99173,99034,99181,99040,98530,98540,98503,98509,98556,98501,98502,98504,98505,98506,98507,98508,98512,98513,98516,98599,98576,98579,98589,98511,98597,98612,98621,98643,98647,99323,99324,99329,99348,99360,99361,99362,99363,98220,98225,98226,98227,98228,98229,98230,98231,98240,98244,98247,98248,98262,98264,98266,98276,98281,98295,99102,99104,99111,99113,99125,99128,99130,99136,99333,99143,99017,99149,99158,99161,99163,99164,99165,99170,99171,99174,99033,99176,99179,98920,98921,98923,98929,98930,98932,98933,98935,98936,98937,98938,98939,98942,98944,98947,98948,98951,98952,98901,98902,98903,98904,98907,98908,98909,98953";

        [TestMethod]
        public void DemographicsScript()
        {

            List<int> allZipCodes = zipCsv.Replace(" ", String.Empty).Split(',').ToList().ConvertAll<int>(x => Convert.ToInt32(x));
            List<int> selectedZipCodes = allZipCodes.GetRange(59, 41);

            List<DemographicsModel> results = DemographicsQuery.GetDemographics(selectedZipCodes, true, 120);
            //DemographicsData.UpdateDatabase(results);
        }
    }
}
