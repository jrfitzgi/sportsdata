using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Social
{
    [Table("FacebookAccountsToFollow")]
    public class FacebookAccount : SocialBaseAccount
    {
    }

    [Table("FacebookAccountSnapshots")]
    public class FacebookSnapshot : SocialBaseSnapshot
    {
        public string FacebookAccountId { get; set; }
        [ForeignKey("FacebookAccountId")]
        public FacebookAccount FacebookAccount { get; set; }

        public int TotalLikes { get; set; }

        public int PeopleTalkingAboutThis { get; set; }

        public DateTime MostPopularWeek { get; set; }

        public string MostPopularCity { get; set; }

        public string MostPopularAgeGroup { get; set; }
    }
}

