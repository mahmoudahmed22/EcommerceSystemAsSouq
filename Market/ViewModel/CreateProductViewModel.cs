using Market.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.ViewModel
{
    public class CreateProductViewModel
    {
        public List<HttpPostedFileBase> files { get; set; }
        //public HttpPostedFileBase file2 { get; set; }
        //public HttpPostedFileBase file3 { get; set; }
        public int  idpro { get; set; }

    }
}