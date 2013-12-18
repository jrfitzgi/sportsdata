using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public class NhlHtmlReportRosterModel : NhlHtmlReportBaseModel
    {

        public string VisitorRoster { get; set; }
        public string VisitorScratches { get; set; }
        public string VisitorHeadCoach { get; set; }

        public string HomeRoster { get; set; }
        public string HomeScratches { get; set; }
        public string HomeHeadCoach { get; set; }

        public string Referees { get; set; }
        public string Linesman { get; set; }
    }
}
