using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Twitter
{
    [Table("TwitterAccountsToFollow")]
    public class TwitterAccount
    {
        [Key]
        public string Id { get; set; }

        public string FriendlyName { get; set; }
    }

    [Table("TwitterAccountSnapshots")]
    public class TwitterAccountSnapshot
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string TwitterAccountId { get; set; }
        [ForeignKey("TwitterAccountId")]
        public TwitterAccount TwitterAccount { get; set; }

        public DateTime DateOfSnapshot { get; set; }

        public int Followers { get; set; }

        public int Following { get; set; }

        public int Tweets { get; set; }

        public string Log { get; set; }
    }
}

