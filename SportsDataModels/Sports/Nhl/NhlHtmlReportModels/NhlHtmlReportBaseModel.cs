using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public class NhlHtmlReportBaseModel
    {
        [Key, Column(Order = 0)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int NhlRtssReportModelId { get; set; }
        //[ForeignKey("NhlRtssReportModelId")]
        //public NhlRtssReportModel NhlRtssReportModel { get; set; }
    }
}
