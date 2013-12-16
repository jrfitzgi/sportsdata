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
    [Table("HtmlBlobs")]
    public class HtmlBlobModel
    {
        [Key]
        [Required]
        public string Url { get; set; }

        [MaxLength]
        public string RawData { get; set; }

        public HtmlBlobType Type { get; set; }
    }
}
