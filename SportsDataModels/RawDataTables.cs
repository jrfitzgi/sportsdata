using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Data.Entity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    [Table("RawDataTables")]
    public class RawDataTable
    {
        [Key]
        [Required]
        public string SourceUrl { get; set; }
        
        [Required]
        public int RawDataType { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        [MaxLength]
        public string RawData { get; set; }

        public string Log { get; set; }
    }
}
