using Market.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.ViewModel
{
    public class ProductDetalisViewModel
    {
        public Product product { get; set; }
        public List<Comment> comments { get; set; }
    }
}