using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Market.Models.Model
{
    public class OrderDetails
    {
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public int Price { get; set; }
        public int Id { get; set; }
        [ForeignKey("order")]
        public int OrderId { get; set; }
        public virtual Order order { get; set; }
        [ForeignKey("product")]
        public int ProductId { get; set; }
        public virtual Product product { get; set; }
       

    }
}