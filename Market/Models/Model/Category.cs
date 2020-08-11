using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Models.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "it should not be more than 20 characters Long")]
        [MinLength(3, ErrorMessage = "Must be at least 3 characters Long")]
        [Display (Name = "Category")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
}