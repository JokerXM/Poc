using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Resources;
using System.ComponentModel.DataAnnotations.Schema;


namespace PoCPoC.Models
{
    public class Events
    {
        public Events()
        {
            this.Users = new List<User>();
            Contributions = new List<Contribution>();
        }
        
        [Key]
        public int E_ID { get; set; }

        public string Createuser { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        
        public string Description { get; set; }
        

        [Display(Name = "Event Time")]
        public DateTime DateAndTime { get; set; }


        public int StatusID { get; set; }

        [ForeignKey("StatusID")]
        public virtual Status status { get; set; }



        public int TypeID { get; set; }
        [ForeignKey("TypeID")]
        public virtual EType type { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Contribution> Contributions { get; set; }
    }

    public class EType
    {
        public EType()
        {
            List<Events> events = new List<Events>();
        }
        [Key]
        public int TypeID { get; set; }       
        public string type { get; set; }
        public virtual ICollection<Events> events { get; set; }
    }

    public class Status
    {

        public Status()
        {
            List<Events> events = new List<Events>();
        }

        [Key]
        public int StatusID { get; set; }
        public string status { get; set; }
        public virtual ICollection<Events> events { get; set; }
    }

}