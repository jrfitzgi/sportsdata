using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Facebook
{
    [Table("FacebookAccountsToFollow")]
    public class FacebookAccount
    {
        [Key]
        public string Id { get; set; }

        public string FriendlyName { get; set; }
    }

    [Table("FacebookAccountSnapshots")]
    public class FacebookAccountSnapshot
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FacebookAccountId { get; set; }
        [ForeignKey("FacebookAccountId")]
        public FacebookAccount FacebookAccount { get; set; }

        public DateTime DateOfSnapshot { get; set; }

        public int TotalLikes { get; set; }

        public int PeopleTalkingAboutThis { get; set; }

        public DateTime MostPopularWeek { get; set; }

        public string MostPopularCity { get; set; }

        public int MostPopularAgeGroup { get; set; }

        public string Log { get; set; }
    }
}

