using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public class Nhl_Team
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AutoId { get; set; }
        public int Id { get; set; }

        public int Nhl_FranchiseId { get; set; }
        //[ForeignKey("Nhl_FranchiseId")] // Not needed - done with fluent api
        //public Nhl_Franchise Nhl_Franchise { get; set; }

        public string City { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Nhl_Game_Summary_Name { get; set; }

        public string MajorCity { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }

        public int YearStarted { get; set; }
        public int YearEnded { get; set; }

    }
}
