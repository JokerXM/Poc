using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoCPoC.Models
{
    public class User
    {

        public User()
        {
            Friends = new List<Friend>();
        }

        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        [ForeignKey("RoleID")]
        public virtual Role role { get; set; }
        public virtual ICollection<Events> Events { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
    }

    public class Role
    {
        public Role(){
        List<User> users = new List<User>();
         }
        public int RoleID { get; set; }
        public string role { get; set; }
        public virtual ICollection<User> users { get; set; }
    }

}