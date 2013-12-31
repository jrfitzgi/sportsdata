using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public class NhlHtmlReportRosterModel : NhlHtmlReportBaseModel
    {
        public NhlHtmlReportRosterModel()
        {
            this.VisitorRoster = new Collection<NhlHtmlReportRosterEntryModel>();
            this.VisitorScratches = new Collection<NhlHtmlReportRosterEntryModel>();
            this.VisitorHeadCoach = new Collection<NhlHtmlReportRosterEntryModel>();

            this.HomeRoster = new Collection<NhlHtmlReportRosterEntryModel>();
            this.HomeScratches = new Collection<NhlHtmlReportRosterEntryModel>();
            this.HomeHeadCoach = new Collection<NhlHtmlReportRosterEntryModel>();

            this.Referees = new Collection<NhlHtmlReportRosterEntryModel>();
            this.Linesman = new Collection<NhlHtmlReportRosterEntryModel>();
        }

        [InverseProperty("NhlHtmlReportRosterModel_VisitorRoster")]
        public virtual ICollection<NhlHtmlReportRosterEntryModel> VisitorRoster { get; set; }
        [InverseProperty("NhlHtmlReportRosterModel_VisitorScratches")]
        public virtual ICollection<NhlHtmlReportRosterEntryModel> VisitorScratches { get; set; }
        [InverseProperty("NhlHtmlReportRosterModel_VisitorHeadCoach")]
        public virtual ICollection<NhlHtmlReportRosterEntryModel> VisitorHeadCoach { get; set; }

        [InverseProperty("NhlHtmlReportRosterModel_HomeRoster")]
        public virtual ICollection<NhlHtmlReportRosterEntryModel> HomeRoster { get; set; }
        [InverseProperty("NhlHtmlReportRosterModel_HomeScratches")]
        public virtual ICollection<NhlHtmlReportRosterEntryModel> HomeScratches { get; set; }
        [InverseProperty("NhlHtmlReportRosterModel_HomeHeadCoach")]
        public virtual ICollection<NhlHtmlReportRosterEntryModel> HomeHeadCoach { get; set; }

        [InverseProperty("NhlHtmlReportRosterModel_Referees")]
        public virtual ICollection<NhlHtmlReportRosterEntryModel> Referees { get; set; }
        [InverseProperty("NhlHtmlReportRosterModel_Linesman")]
        public virtual ICollection<NhlHtmlReportRosterEntryModel> Linesman { get; set; }
    }

    public class NhlHtmlReportRosterEntryModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public NhlHtmlReportRosterEntryModel()
        {
        }

        public NhlHtmlReportRosterEntryModel(string name, int id)
        {
            this.Name = name;
            //this.NhlHtmlReportRosterModelId = id;
        }

        public string Name { get; set; }

        //public int NhlHtmlReportRosterModelId { get; set; }
        //[ForeignKey("NhlHtmlReportRosterModelId")]
        [InverseProperty("VisitorRoster")]
        public virtual NhlHtmlReportRosterModel NhlHtmlReportRosterModel_VisitorRoster { get; set; }

        [InverseProperty("VisitorScratches")]
        public virtual NhlHtmlReportRosterModel NhlHtmlReportRosterModel_VisitorScratches { get; set; }

        [InverseProperty("VisitorHeadCoach")]
        public virtual NhlHtmlReportRosterModel NhlHtmlReportRosterModel_VisitorHeadCoach { get; set; }

        [InverseProperty("HomeRoster")]
        public virtual NhlHtmlReportRosterModel NhlHtmlReportRosterModel_HomeRoster { get; set; }

        [InverseProperty("HomeScratches")]
        public virtual NhlHtmlReportRosterModel NhlHtmlReportRosterModel_HomeScratches { get; set; }

        [InverseProperty("HomeHeadCoach")]
        public virtual NhlHtmlReportRosterModel NhlHtmlReportRosterModel_HomeHeadCoach { get; set; }

        [InverseProperty("Referees")]
        public virtual NhlHtmlReportRosterModel NhlHtmlReportRosterModel_Referees { get; set; }

        [InverseProperty("Linesman")]
        public virtual NhlHtmlReportRosterModel NhlHtmlReportRosterModel_Linesman { get; set; }

    }
}
