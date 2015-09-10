using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace PoCPoC.Models
{
    public class Friend
    {
        [Key]
        public int FriendID { get; set; }

        public int UserID { get; set; }
        public string FriendName { get; set; }

        public int Friend_ID { get; set; }
        [ForeignKey("Friend_ID")]
        public virtual User User { get; set; }
    }
}