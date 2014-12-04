using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public class Nhl_Games_Rtss_Summary : Nhl_HtmlReportBaseModel
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Visitor { get; set; }

        public int VisitorScore { get; set; }

        public int VisitorGameNumber { get; set; }

        public int VisitorAwayGameNumber { get; set; }

        [Required]
        public string Home { get; set; }

        public int HomeScore { get; set; }

        public int HomeGameNumber { get; set; }

        public int HomeHomeGameNumber { get; set; }

        public int Attendance { get; set; }

        public string ArenaName { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int LeagueGameNumber { get; set; }

        public string GameStatus { get; set; }

        [InverseProperty("Nhl_Games_Rtss_Summary_ScoringSummary")]
        public List<Nhl_Games_Rtss_Summary_ScoringSummary_Item> ScoringSummary { get; set; }

        [InverseProperty("Nhl_Games_Rtss_Summary_PenaltySummary_Home")]
        public List<Nhl_Games_Rtss_Summary_PenaltySummary_Item> PenaltySummary_Home { get; set; }

        [InverseProperty("Nhl_Games_Rtss_Summary_PenaltySummary_Visitor")]
        public List<Nhl_Games_Rtss_Summary_PenaltySummary_Item> PenaltySummary_Visitor { get; set; }

        [InverseProperty("Nhl_Games_Rtss_Summary_PeriodSummary_Home")]
        public List<Nhl_Games_Rtss_Summary_PeriodSummary_Item> PeriodSummary_Home { get; set; }

        [InverseProperty("Nhl_Games_Rtss_Summary_PeriodSummary_Visitor")]
        public List<Nhl_Games_Rtss_Summary_PeriodSummary_Item> PeriodSummary_Visitor { get; set; }

        //[InverseProperty("Nhl_Games_Rtss_Summary_PowerPlaySummary_Home")]
        public Nhl_Games_Rtss_Summary_PowerPlaySummary_Item PowerPlaySummary_Home { get; set; }

        //[InverseProperty("Nhl_Games_Rtss_Summary_PowerPlaySummary_Visitor")]
        public Nhl_Games_Rtss_Summary_PowerPlaySummary_Item PowerPlaySummary_Visitor { get; set; } 

        [InverseProperty("Nhl_Games_Rtss_Summary_GoalieSummary_Home")]
        public List<Nhl_Games_Rtss_Summary_GoalieSummary_Item> GoalieSummary_Home { get; set; }

        [InverseProperty("Nhl_Games_Rtss_Summary_GoalieSummary_Visitor")]
        public List<Nhl_Games_Rtss_Summary_GoalieSummary_Item> GoalieSummary_Visitor { get; set; }

        [InverseProperty("Nhl_Games_Rtss_Summary_Officials")]
        public List<Nhl_Games_Rtss_Summary_Officials_Item> Officials { get; set; }

        [InverseProperty("Nhl_Games_Rtss_Summary_Stars")]
        public List<Nhl_Games_Rtss_Summary_Stars_Item> Stars { get; set; }

    }

    public class Nhl_Games_Rtss_Summary_ScoringSummary_Item
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [InverseProperty("ScoringSummary")]
        public Nhl_Games_Rtss_Summary Nhl_Games_Rtss_Summary_ScoringSummary { get; set; }

        public int GoalNumber { get; set; }
        public int Period { get; set; }
        public int TimeInSeconds { get; set; }
        public string Strength { get; set; }
        public string Team { get; set; }
        public string GoalScorer { get; set; }
        public int GoalScorerPlayerNumber { get; set; }
        public int GoalScorerGoalNumber { get; set; }
        public string Assist1 { get; set; }
        public int Assist1PlayerNumber { get; set; }
        public int Assist1AssistNumber { get; set; }
        public string Assist2 { get; set; }
        public int Assist2PlayerNumber { get; set; }
        public int Assist2AssistNumber { get; set; }
        public string VisitorOnIce { get; set; }
        public string HomeOnIce { get; set; }
    }

    public class Nhl_Games_Rtss_Summary_PenaltySummary_Item
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [InverseProperty("PenaltySummary_Home")]
        public Nhl_Games_Rtss_Summary Nhl_Games_Rtss_Summary_PenaltySummary_Home { get; set; }

        [InverseProperty("PenaltySummary_Visitor")]
        public Nhl_Games_Rtss_Summary Nhl_Games_Rtss_Summary_PenaltySummary_Visitor { get; set; }

        public int PenaltyNumber { get; set; }
        public int Period { get; set; }
        public int TimeInSeconds { get; set; }
        public int PlayerNumber { get; set; }
        public string Name { get; set; }
        public int PIM { get; set; }
        public string Penalty { get; set; }
    }

    public class Nhl_Games_Rtss_Summary_PeriodSummary_Item
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [InverseProperty("PeriodSummary_Home")]
        public Nhl_Games_Rtss_Summary Nhl_Games_Rtss_Summary_PeriodSummary_Home { get; set; }

        [InverseProperty("PeriodSummary_Visitor")]
        public Nhl_Games_Rtss_Summary Nhl_Games_Rtss_Summary_PeriodSummary_Visitor { get; set; }

        public int Period { get; set; }
        public int Goals { get; set; }
        public int Shots { get; set; }
        public int Penalties { get; set; }
        public int PIM { get; set; }
    }

    public class Nhl_Games_Rtss_Summary_PowerPlaySummary_Item
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[InverseProperty("PowerPlaySummary_Home")]
        //public Nhl_Games_Rtss_Summary Nhl_Games_Rtss_Summary_PowerPlaySummary_Home { get; set; }

        //[InverseProperty("PowerPlaySummary_Visitor")]
        //public Nhl_Games_Rtss_Summary Nhl_Games_Rtss_Summary_PowerPlaySummary_Visitor { get; set; }

        public int PowerPlay5v4Goals { get; set; }
        public int PowerPlay5v4Occurrences { get; set; }
        public int PowerPlay5v4ToiSeconds { get; set; }

        public int PowerPlay5v3Goals { get; set; }
        public int PowerPlay5v3Occurrences { get; set; }
        public int PowerPlay5v3ToiSeconds { get; set; }

        public int PowerPlay4v3Goals { get; set; }
        public int PowerPlay4v3Occurrences { get; set; }
        public int PowerPlay4v3ToiSeconds { get; set; }

        public int EvenStrength5v5Goals { get; set; }
        public int EvenStrength5v5Occurrences { get; set; }
        public int EvenStrength5v5ToiSeconds { get; set; }

        public int EvenStrength4v4Goals { get; set; }
        public int EvenStrength4v4Occurrences { get; set; }
        public int EvenStrength4v4ToiSeconds { get; set; }

        public int EvenStrength3v3Goals { get; set; }
        public int EvenStrength3v3Occurrences { get; set; }
        public int EvenStrength3v3ToiSeconds { get; set; }
    }

    public class Nhl_Games_Rtss_Summary_GoalieSummary_Item
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [InverseProperty("GoalieSummary_Home")]
        public Nhl_Games_Rtss_Summary Nhl_Games_Rtss_Summary_GoalieSummary_Home { get; set; }

        [InverseProperty("GoalieSummary_Visitor")]
        public Nhl_Games_Rtss_Summary Nhl_Games_Rtss_Summary_GoalieSummary_Visitor { get; set; }

        public int Number { get; set; }
        public string Name { get; set; }
        public int ToiInSecondsEvenStrength { get; set; }
        public int ToiInSecondsPowerPlay { get; set; }
        public int ToiInSecondsShortHanded { get; set; }
        public int ToiInSecondsTotal { get; set; }

        [InverseProperty("Nhl_Games_Rtss_Summary_GoalieSummary_Item")]
        public List<Nhl_Games_Rtss_Summary_GoaliePeriodSummary_Item> GoaliePeriodSummary { get; set; }

        public class Nhl_Games_Rtss_Summary_GoaliePeriodSummary_Item
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [InverseProperty("GoaliePeriodSummary")]
            public Nhl_Games_Rtss_Summary_GoalieSummary_Item Nhl_Games_Rtss_Summary_GoalieSummary_Item { get; set; }

            public int Period { get; set; }
            public int GoalsAgainst { get; set; }
            public int ShotsAgainst { get; set; }
        }
    }

    public class Nhl_Games_Rtss_Summary_Officials_Item
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [InverseProperty("Officials")]
        public Nhl_Games_Rtss_Summary Nhl_Games_Rtss_Summary_Officials { get; set; }

        public Designation OfficialType { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
    }

    public class Nhl_Games_Rtss_Summary_Stars_Item
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [InverseProperty("Stars")]
        public Nhl_Games_Rtss_Summary Nhl_Games_Rtss_Summary_Stars { get; set; }

        public int StarNumber { get; set; }
        public int Team { get; set; }
        public string Position { get; set; }
        public int PlayerNumber { get; set; }
        public string Name { get; set; }
    }

}
