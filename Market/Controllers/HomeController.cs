using Market.Models;
using Market.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            AllProductViewModeWithCategeroy model = new AllProductViewModeWithCategeroy();
            model.Mobiles = context.products.Where(c => c.CategoryId == 1).ToList();
            model.Sports = context.products.Where(c => c.CategoryId == 4).ToList();
            model.Home = context.products.Where(c => c.CategoryId == 3).ToList();
            model.Electronics = context.products.Where(c => c.CategoryId == 2).ToList();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}