using Market.Models.Model;
using Market.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        // GET: Search
        services services = new services();
        public ActionResult Index()
        {
            ViewBag.categories = services.GetCategories();
            ViewBag.sup = services.getAllSupplier();
            return View(services.GetProducts());
        }
        [HttpPost]
        public JsonResult SearchByName(string Prefix)
        {
            List<String> product = services.SearchProductByName(Prefix);
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchByCategoryAndPrice(int CategoryId, int PriceP ,int PriceA, string order)
        {
            return PartialView("_Getall", services.SearchByCategoryAndPrice(CategoryId, PriceP, PriceA, order));
        }
        public ActionResult SearchByCategory(int id)
        {
            return PartialView("_Getall", services.GetProdectByCategoryId(id));
        }
        public ActionResult SearchUseingName(string Name)
        {
            return PartialView("_Getall", services.SearchUseingName(Name));
        }
        public ActionResult GetALLProduct()
        {
            return PartialView("_Getall", services.GetProducts());
        }


    }

}