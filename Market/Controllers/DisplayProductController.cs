using Market.Models;
using Market.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Controllers
{
    [Authorize]
    public class DisplayProductController : Controller
    {
        // GET: DisplayProduct
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult ProductDetails(int id)
        {
            ProductDetalisViewModel model = new ProductDetalisViewModel();
            model.product = context.products.FirstOrDefault(c => c.ID == id);
            model.comments = context.comments.Where(c => c.ProductId == id).ToList();
            return View("ProductDetails", model);
        }

        public ActionResult DisplayComment(int id)
        {
           
            return PartialView("_DisplayComment", context.comments.Where(c=>c.ProductId==id).OrderByDescending(c=>c.dateTime).ToList());
        }
        public ActionResult GetAllMobileProducts()
        {
            return View(context.products.Where(c=>c.category.Name=="Mobile&Tablets").ToList());
        }
        public ActionResult GetAllHomeProducts()
        {
            return View(context.products.Where(c => c.category.Name == "Home,Kitchen,Tools").ToList());
        }
        public ActionResult GetAllSportProducts()
        {
            return View(context.products.Where(c => c.category.Name == "Sports&Fitness").ToList());
        }
        public ActionResult GetAllElectronicsProducts()
        {
            return View(context.products.Where(c => c.category.Name == "Electronics").ToList());
        }
    }
}