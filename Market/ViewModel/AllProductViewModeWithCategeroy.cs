using Market.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.ViewModel
{
    public class AllProductViewModeWithCategeroy
    {
        public List<Product> Home { get; set; }
        public List<Product> Mobiles { get; set; }
        public List<Product> Sports { get; set; }
        public List<Product> Electronics { get; set; }
    }
}