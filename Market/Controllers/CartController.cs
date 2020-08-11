using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Market.Models;
using Market.Models.Model;
using Market.Services;
using Market.ViewModel;

namespace Market.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CartController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        services services = new services();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddToCart(int ID)
        {
            if (Session["cart"] == null)
            {
                var cart = new List<Item>();
                var product = services.GetProdectById(ID);//context.products.Find(prodid);
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
                var product = services.GetProdectById(ID);//context.products.Find(prodid);
                var c = cart.Find(s => s.Product.ID == ID);
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
            return RedirectToAction("Index","Home");
        }
        public ActionResult RemoveToCart(int ID)
        {

            List<Item> cart = (List<Item>)Session["cart"];
            foreach (var item in cart)
            {
                if (item.Product.ID == ID)
                {
                    cart.Remove(item);
                    break;
                }
            }

            Session["cart"] = cart;


            return RedirectToAction("Index", "Home");
        }
        public ActionResult CheckOut()
        {
            return View();
        }
        public ActionResult BuyItem(string id)
        { if (Session["cart"] != null)
            {
                Order order = new Order();
                order.Date = DateTime.Now;
                order.CustomerId = id;
                bool flag = true;

                foreach (var item in (List<Item>)Session["cart"])
                {
                    Product CartProduct = services.GetProdectById(item.Product.ID); //context.products.Find(item.Product.ID);
                   
                              
                    var QuantityProduct = item.Quantity;
                    CartProduct.Quantity = CartProduct.Quantity - QuantityProduct;
                    if (CartProduct.Quantity > 0)
                    {
                        order.TotalCost += item.Product.Price*item.Quantity;
                        OrderDetails orderDetails = new OrderDetails();
                        orderDetails.ProductId = item.Product.ID;
                        orderDetails.TotalPrice= item.Product.Price * item.Quantity;
                        orderDetails.Quantity = item.Quantity;
                        orderDetails.Price = item.Product.Price;
                        orderDetails.order = order;
                        if (flag)
                        {
                            context.orders.Add(order);
                            flag = false;
                        }
                        
                        context.SaveChanges();
                        context.OrderDetails.Add(orderDetails);
                        context.SaveChanges();
                        services.context.SaveChanges();
                    }

                    Session["cart"] = null;

                }
               
                
            }
            return RedirectToAction("Index", "Home");
        }
    }
}