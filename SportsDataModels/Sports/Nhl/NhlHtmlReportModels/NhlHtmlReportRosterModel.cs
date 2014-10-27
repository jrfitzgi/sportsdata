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
            this.VisitorRoster = new Collection<NhlHtmlReportRosterParticipantModel>();
            this.VisitorScratches = new Collection<NhlHtmlReportRosterParticipantModel>();
            this.VisitorHeadCoach = new Collection<NhlHtmlReportRosterParticipantModel>();

            this.HomeRoster = new Collection<NhlHtmlReportRosterParticipantModel>();
            this.HomeScratches = new Collection<NhlHtmlReportRosterParticipantModel>();
            this.HomeHeadCoach = new Collection<NhlHtmlReportRosterParticipantModel>();

            this.Referees = new Collection<NhlHtmlReportRosterParticipantModel>();
            this.Linesman = new Collection<NhlHtmlReportRosterParticipantModel>();
        }

        [InverseProperty("NhlHtmlReportRosterModel_VisitorRoster")]
        public virtual ICollection<NhlHtmlReportRosterParticipantModel> VisitorRoster { get; set; }
        [InverseProperty("NhlHtmlReportRosterModel_VisitorScratches")]
        public virtual ICollection<NhlHtmlReportRosterParticipantModel> VisitorScratches { get; set; }
        [InverseProperty("NhlHtmlReportRosterModel_VisitorHeadCoach")]
        public virtual ICollection<NhlHtmlReportRosterParticipantModel> VisitorHeadCoach { get; set; }

        [InverseProperty("NhlHtmlReportRosterModel_HomeRoster")]
        public virtual ICollection<NhlHtmlReportRosterParticipantModel> HomeRoster { get; set; }
        [InverseProperty("NhlHtmlReportRosterModel_HomeScratches")]
        public virtual ICollection<NhlHtmlReportRosterParticipantModel> HomeScratches { get; set; }
        [InverseProperty("NhlHtmlReportRosterModel_HomeHeadCoach")]
        public virtual ICollection<NhlHtmlReportRosterParticipantModel> HomeHeadCoach { get; set; }

        [InverseProperty("NhlHtmlReportRosterModel_Referees")]
        public virtual ICollection<NhlHtmlReportRosterParticipantModel> Referees { get; set; }
        [InverseProperty("NhlHtmlReportRosterModel_Linesman")]
        public virtual ICollection<NhlHtmlReportRosterParticipantModel> Linesman { get; set; }
    }

    public enum ParticipantType
    {
        None = 0,
        Player,
        Official,
        Coach
    }

    public enum Designation
    {
        None = 0,
        Captain,
        AssistantCaptain,
        HeadCoach,
        AssistantCoach,
        Referee,
        Linesman,
        Standby
    }

    public class NhlHtmlReportRosterParticipantModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public NhlHtmlReportRosterParticipantModel()
        {
        }

        public string Name { get; set; }
        public ParticipantType ParticipantType { get; set; }
        public Designation Designation { get; set; }
        public int Number { get; set; }
        public string Position { get; set; }
        public bool StartingLineup { get; set; }

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
