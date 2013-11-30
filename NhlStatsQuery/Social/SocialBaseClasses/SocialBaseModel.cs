//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace SportsData.Social
//{
//    public abstract class SocialBaseAccount
//    {
//        [Key]
//        public virtual string Id { get; set; }

//        public virtual string FriendlyName { get; set; }
//    }

//    public class SocialBaseSnapshot
//    {
//        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public virtual int Id { get; set; }

//        public virtual DateTime DateOfSnapshot { get; set; }

//        public virtual string Log { get; set; }
//    }
//}

