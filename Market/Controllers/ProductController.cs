using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Market.Hubs;
using Market.Models;
using Market.Models.Model;
using Market.Services;
using Market.ViewModel;
using Microsoft.AspNet.SignalR;

namespace Market.Controllers
{
    [System.Web.Mvc.Authorize(Roles = "Supplier,Admin")]
    public class ProductController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Product
        services services = new services();
        public ActionResult Index()
        {
           
            return View(services.GetAllProductbySuppler());
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.categories = services.GetCategories();           
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int idP = services.InsertProduct(product);
                    CreateProductViewModel model = new CreateProductViewModel() { idpro = idP };

                    
                    IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ProductHob>();
                    if (product.CategoryId == 1)
                    {
                        hubContext.Clients.All.NewProductAddToMobile(product.Name);
                    }
                    else if (product.CategoryId == 3)
                    {
                        hubContext.Clients.All.NewProductAddToHome(product.Name);
                    }
                    else if (product.CategoryId == 4)
                    {
                        hubContext.Clients.All.NewProductAddToSport(product.Name);
                    }
                    else if (product.CategoryId == 2)
                    {
                        hubContext.Clients.All.NewProductAddToElec(product.Name);
                    }


                    return View("AddImage", model);                  
                }
                else
                    return View();
            }
            catch
            {
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult AddImage(CreateProductViewModel model)
        {
            for (int i=0;i<model.files.Count;i++)
            {
                if (model.files[i] != null)
                {
                    Image image = new Image();
                    string path = Path.GetFileName(model.files[i].FileName);
                    image.Url = "/Uploads/ProductImages/" + path;
                    image.ProductId = model.idpro;
                    path = Path.Combine(Server.MapPath("~/Uploads/ProductImages/"), path);
                    model.files[i].SaveAs(path);
                    services.AddImageToProduct(image);
                }
            }
           
            return RedirectToAction("Index");
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            Product product= services.GetProdectById(id);
            ViewBag.categories = services.GetCategories();
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    services.UpdateProduct(id, product);
                    CreateProductViewModel model = new CreateProductViewModel() { idpro = id };
                    return View("updateImages",model);                    
                }
                else
                    return View();
            }catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult updateImage(CreateProductViewModel model)
        {
            services.DeleteImagesFromProduct(model.idpro);
            for (int i = 0; i < model.files.Count; i++)
            {
                if (model.files[i] != null)
                {
                    Image image = new Image();
                    string path = Path.GetFileName(model.files[i].FileName);
                    image.Url = "/Uploads/ProductImages/" + path;
                    image.ProductId = model.idpro;
                    path = Path.Combine(Server.MapPath("~/Uploads/ProductImages/"), path);
                    model.files[i].SaveAs(path);
                    services.AddImageToProduct(image);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
           Product product= services.GetProdectById(id);
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Product product)
        {
            try
            {
                List<Image> images = services.DeleteImagesFromProduct(id);
                foreach (var item in images)
                {
                    string filePath = Server.MapPath(item.Url);
                    if (System.IO.File.Exists(filePath))
                           System.IO.File.Delete(filePath);
                }    
                
                services.DeleteProduct(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
     
       
        public JsonResult IsProductNameAvailable(string Name)
        {
            return Json(!services.SearchProductName(Name),JsonRequestBehavior.AllowGet);
        }
        //==============================================================================================
        //==============================================================================================
        public ActionResult AddToCart(int prodid)
        {
            if (Session["cart"] == null)
            {
                var cart = new List<Item>();
                var product = services.GetProdectById(prodid);//context.products.Find(prodid);
                if (product.Quantity > 1)
                {
                    cart.Add(new Item()
                    {
                        Product = product,
                        Quantity = 1
                    });

                    Session["cart"] = cart;
                }
            }
            else
            {

                List<Item> cart = (List<Item>)Session["cart"];
                var product = services.GetProdectById(prodid);//context.products.Find(prodid);
                var c = cart.Find(s => s.Product.ID == prodid);
                if (c != null)
                {
                    int prev = c.Quantity;
                    cart.Remove(c);
                    if (product.Quantity > 1)
                    {
                        cart.Add(new Item()
                        {
                            Product = product,
                            Quantity = prev + 1
                        });
                    }

                }
                else
                {
                    if (product.Quantity > 1)
                    {
                        cart.Add(new Item()
                        {
                            Product = product,
                            Quantity = 1
                        });


                        Session["cart"] = cart;
                    }
                }

            }
            return RedirectToAction("Index");
        }
        public ActionResult RemoveToCart(int prodid)
        {

            List<Item> cart = (List<Item>)Session["cart"];
            foreach (var item in cart)
            {
                if (item.Product.ID == prodid)
                {
                    cart.Remove(item);
                    break;
                }
            }

            Session["cart"] = cart;


            return RedirectToAction("Index");
        }
        public ActionResult CheckOut()
        {
            return View();
        }
        public ActionResult BuyItem()
        {  if (Session["cart"] != null)
            {
                foreach (var item in (List<Item>)Session["cart"])
                {
                    Product CartProduct = services.GetProdectById(item.Product.ID); //context.products.Find(item.Product.ID);
                    var QuantityProduct = item.Quantity;
                    CartProduct.Quantity = CartProduct.Quantity - QuantityProduct;
                    if (CartProduct.Quantity > 0)
                    {
                        services.context.SaveChanges();
                    }

                    Session["cart"] = null;

                }
            }
            return RedirectToAction("Index");
        }

    }
}
