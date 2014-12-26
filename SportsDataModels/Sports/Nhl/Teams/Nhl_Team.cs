using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public class Nhl_Team
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Nhl_FranchiseId { get; set; }
        [ForeignKey("Nhl_FranchiseId")]
        public Nhl_Franchise Nhl_Franchise { get; set; }

        public string City { get; set; }
        public string Name { get; set; }

        public int YearStarted { get; set; }
        public int YearEnded { get; set; }

        public string StateProvince { get; set; }
        public string Country { get; set; }
    }
}
