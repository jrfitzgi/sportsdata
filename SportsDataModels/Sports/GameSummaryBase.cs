using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    [Table("GameSummary")]
    public class GameSummary
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Season { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Visitor { get; set; }

        [Required]
        [Display(Name = "Visitor Score")]
        public int VisitorScore { get; set; }

        [Required]
        public string Home { get; set; }

        [Required]
        [Display(Name = "Home Score")]
        public int HomeScore { get; set; }

        [Required]
        [Display(Name = "Attendance")]
        public int Attendance { get; set; }
    }
}
