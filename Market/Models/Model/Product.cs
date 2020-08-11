using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Models.Model
{
    public class Product
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please Enter Name of Product")]
        [Remote("IsProductNameAvailable", "Product", ErrorMessage = "Name already exists")]
        public string Name { get; set; }
        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
        [Required]
        public int Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime dateTime { get; set; }
        public string Description { get; set; }
        [ForeignKey("category")]
        public int CategoryId { get; set; }
        public virtual Category category { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Comment> comments { get; set; }
        [ForeignKey("supplier")]
        public string supplierID { get; set; }
        public virtual Supplier supplier { get; set; }
    }
}