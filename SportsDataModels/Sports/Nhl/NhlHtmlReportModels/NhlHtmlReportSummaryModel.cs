using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    /// <summary>
    /// This is the summary that appears at the top of all html reports
    /// </summary>
    public class NhlHtmlReportSummaryModel : NhlHtmlReportBaseModel
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

        [InverseProperty("NhlHtmlReportSummaryModel")]
        public ICollection<ScoringSummary> ScoringSummary { get; set; }

        [InverseProperty("NhlHtmlReportSummaryModel")]
        public List<PenaltySummary> HomePenaltySummary { get; set; }

        [InverseProperty("NhlHtmlReportSummaryModel")]
        public List<PenaltySummary> VisitorPenaltySummary { get; set; }

        // FK to ByPeriod

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

        // FK to GoaltenderSummary

        // FK to Refs (or re-use roster)

        // FK to Stars
    }

    /// <summary>
    /// The Scoring Summary section of the html report
    /// </summary>
    public class ScoringSummary
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [InverseProperty("ScoringSummary")]
        public NhlHtmlReportSummaryModel NhlHtmlReportSummaryModel { get; set; }

        public int GoalNumber { get; set; }
        public int Period { get; set; }
        public int TimeInSeconds { get; set; }
        public string Strength { get; set; }
        public string Team { get; set; }
        public string GoalScorer { get; set; }
        public string Assist1 { get; set; }
        public string Assist2 { get; set; }
        public string VisitorOnIce { get; set; }
        public string HomeOnIce { get; set; }
    }

    /// <summary>
    /// The Penalty Summary section of the html report
    /// </summary>

    public class PenaltySummary
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public NhlHtmlReportSummaryModel NhlHtmlReportSummaryModel { get; set; }

        public int PenaltyNumber { get; set; }
        public int Period { get; set; }
        public int TimeInSeconds { get; set; }
        public int PlayerNumber { get; set; }
        public string Name { get; set; }
        public int PIM { get; set; }
        public string Penalty { get; set; }   
    }

    /// <summary>
    /// The per-period summary of the html report
    /// </summary>
    public class ByPeriod
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Period { get; set; }
        public int Goals { get; set; }
        public int Shots { get; set; }
        public int PN { get; set; }
        public int PIM { get; set; }
    }

    /// <summary>
    /// The goaltender summary of the html report
    /// </summary>
    public class GoaltenderSummary
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Number { get; set; }
        public string Name { get; set; }
        public int ToiInSecondsEvenStrength { get; set; }
        public int ToiInSecondsPowerPlay { get; set; }
        public int ToiInSecondsShortHanded { get; set; }
        public int ToiInSecondsTotal { get; set; }

        public class PeriodSummary
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            public int Period { get; set; }
            public int GoalsAgainst { get; set; }
            public int ShotsAgainst { get; set; }
        }
    }

    /// <summary>
    /// The Officials section of the html report
    /// </summary>
    public class Officials
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Designation OfficialType { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// The three stars section of the html report
    /// </summary>
    public class Stars
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int StarNumber { get; set; }
        public int Team { get; set; }
        public string Position { get; set; }
        public int PlayerNumber { get; set; }
        public string Name { get; set; }
    }

}