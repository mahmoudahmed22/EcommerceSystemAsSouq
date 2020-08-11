using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Market.Models.Model
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TotalCost { get; set; }
        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }

    }
}