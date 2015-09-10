using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Resources;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoCPoC.Models
{
    public class Contribution
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quanlity { get; set; }
        public int TypeID { get; set; }
        [ForeignKey("TypeID")]
        public virtual ContributionType contributiontype { get; set; }

        public int E_ID { get; set; }
        [ForeignKey("E_ID")]
        public virtual Events E { get; set; }
    }
    public class ContributionType
    {
        
        public ContributionType()
        {
            List<Contribution> contributions = new List<Contribution>();
        }
        [Key]
        public int ID { get; set; }
        public string type { get; set; }
        public virtual ICollection<Contribution> contributions { get; set; }

    }


}