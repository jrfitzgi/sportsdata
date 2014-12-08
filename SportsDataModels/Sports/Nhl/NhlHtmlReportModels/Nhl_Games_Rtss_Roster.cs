using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public class Nhl_Games_Rtss_Roster : Nhl_HtmlReportBaseModel
    {
        public Nhl_Games_Rtss_Roster()
        {
            this.VisitorRoster = new Collection<Nhl_Games_Rtss_RosterParticipantItem>();
            this.VisitorScratches = new Collection<Nhl_Games_Rtss_RosterParticipantItem>();
            this.VisitorHeadCoach = new Collection<Nhl_Games_Rtss_RosterParticipantItem>();

            this.HomeRoster = new Collection<Nhl_Games_Rtss_RosterParticipantItem>();
            this.HomeScratches = new Collection<Nhl_Games_Rtss_RosterParticipantItem>();
            this.HomeHeadCoach = new Collection<Nhl_Games_Rtss_RosterParticipantItem>();

            this.Referees = new Collection<Nhl_Games_Rtss_RosterParticipantItem>();
            this.Linesman = new Collection<Nhl_Games_Rtss_RosterParticipantItem>();
        }

        [InverseProperty("Nhl_Games_Rtss_Roster_VisitorRoster")]
        public virtual ICollection<Nhl_Games_Rtss_RosterParticipantItem> VisitorRoster { get; set; }
        [InverseProperty("Nhl_Games_Rtss_Roster_VisitorScratches")]
        public virtual ICollection<Nhl_Games_Rtss_RosterParticipantItem> VisitorScratches { get; set; }
        [InverseProperty("Nhl_Games_Rtss_Roster_VisitorHeadCoach")]
        public virtual ICollection<Nhl_Games_Rtss_RosterParticipantItem> VisitorHeadCoach { get; set; }

        [InverseProperty("Nhl_Games_Rtss_Roster_HomeRoster")]
        public virtual ICollection<Nhl_Games_Rtss_RosterParticipantItem> HomeRoster { get; set; }
        [InverseProperty("Nhl_Games_Rtss_Roster_HomeScratches")]
        public virtual ICollection<Nhl_Games_Rtss_RosterParticipantItem> HomeScratches { get; set; }
        [InverseProperty("Nhl_Games_Rtss_Roster_HomeHeadCoach")]
        public virtual ICollection<Nhl_Games_Rtss_RosterParticipantItem> HomeHeadCoach { get; set; }

        [InverseProperty("Nhl_Games_Rtss_Roster_Referees")]
        public virtual ICollection<Nhl_Games_Rtss_RosterParticipantItem> Referees { get; set; }
        [InverseProperty("Nhl_Games_Rtss_Roster_Linesman")]
        public virtual ICollection<Nhl_Games_Rtss_RosterParticipantItem> Linesman { get; set; }
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
        Standby,
        StandbyReferee,
        StandbyLinesman
    }

    public class Nhl_Games_Rtss_RosterParticipantItem
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Nhl_Games_Rtss_RosterParticipantItem()
        {
        }

        public string Name { get; set; }
        public ParticipantType ParticipantType { get; set; }
        public Designation Designation { get; set; }
        public int Number { get; set; }
        public string Position { get; set; }
        public bool StartingLineup { get; set; }

        [InverseProperty("VisitorRoster")]
        public virtual Nhl_Games_Rtss_Roster Nhl_Games_Rtss_Roster_VisitorRoster { get; set; }

        [InverseProperty("VisitorScratches")]
        public virtual Nhl_Games_Rtss_Roster Nhl_Games_Rtss_Roster_VisitorScratches { get; set; }

        [InverseProperty("VisitorHeadCoach")]
        public virtual Nhl_Games_Rtss_Roster Nhl_Games_Rtss_Roster_VisitorHeadCoach { get; set; }

        [InverseProperty("HomeRoster")]
        public virtual Nhl_Games_Rtss_Roster Nhl_Games_Rtss_Roster_HomeRoster { get; set; }

        [InverseProperty("HomeScratches")]
        public virtual Nhl_Games_Rtss_Roster Nhl_Games_Rtss_Roster_HomeScratches { get; set; }

        [InverseProperty("HomeHeadCoach")]
        public virtual Nhl_Games_Rtss_Roster Nhl_Games_Rtss_Roster_HomeHeadCoach { get; set; }

        [InverseProperty("Referees")]
        public virtual Nhl_Games_Rtss_Roster Nhl_Games_Rtss_Roster_Referees { get; set; }

        [InverseProperty("Linesman")]
        public virtual Nhl_Games_Rtss_Roster Nhl_Games_Rtss_Roster_Linesman { get; set; }

    }
}
