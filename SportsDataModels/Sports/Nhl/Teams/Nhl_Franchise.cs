using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public class Nhl_Franchise
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AutoId { get; set; }

        public int Id { get; set; }

        public string OriginalName { get; set; }
        public string OriginalCity { get; set; }

        public int YearStarted { get; set; }
        public int YearEnded { get; set; }
    }
}
