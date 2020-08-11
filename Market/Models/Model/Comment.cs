using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Market.Models.Model
{
    public class Comment
    {
        public int Id { get; set; }
        public string comment { get; set; }
        public int Reating { get; set; }
        [ForeignKey("customer")]
        public string CustomerId { get; set; }      
        public virtual Customer customer { get; set; }
        [ForeignKey("product")]
        public int ProductId { get; set; }
        public virtual Product product { get; set; }
        public  DateTime dateTime { get; set; }
       
    }
}