using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public class Nhl_Draftbook
    {
        [Key, Column(Order = 0)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Year { get; set; }

        public int Round { get; set; }

        public int Pick { get; set; }

        public int Overall { get; set; }

        public string Team { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string POB { get; set; }

        public int HeightInches { get; set; }

        public int WeightLbs { get; set; }

        public string AmateurLeague { get; set; }

        public string AmateurTeam { get; set; }
    }
}
