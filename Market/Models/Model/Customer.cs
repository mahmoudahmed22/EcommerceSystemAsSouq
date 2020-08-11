using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Market.Models.Model
{
    public class Customer
    {
       
        [ForeignKey("ApplicationUser")]
        [Key]
        public string CusID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string  Address { get; set; }
        public virtual ICollection<Order> orders { get; set; }
        public virtual ICollection<Comment>  comments { get; set; }

    }
}