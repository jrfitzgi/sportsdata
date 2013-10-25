using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Social
{
    [Table("TwitterAccountsToFollow")]
    public class TwitterAccount : SocialBaseAccount
    {
    }

    [Table("TwitterAccountSnapshots")]
    public class TwitterSnapshot : SocialBaseSnapshot
    {
        public string TwitterAccountId { get; set; }
        [ForeignKey("TwitterAccountId")]
        public TwitterAccount TwitterAccount { get; set; }

        public int Followers { get; set; }

        public int Following { get; set; }

        public int Tweets { get; set; }
    }
}

