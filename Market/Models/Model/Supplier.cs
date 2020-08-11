using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Market.Models.Model
{
    public class Supplier
    {
        
        [ForeignKey("user")]
        [Key]
        public string supID { get; set; }
        public virtual  ApplicationUser user { get; set; }
       
        public string Company{ get; set; }
        public virtual ICollection<Product> products { get; set; }

    }
}