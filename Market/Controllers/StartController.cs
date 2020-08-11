using Market.Models;
using Market.Models.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Controllers
{
    public class StartController : Controller
    {
        // GET: Start
        ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            context.Roles.Add(new IdentityRole("Admin"));
            context.Roles.Add(new IdentityRole("Supplier"));
            context.Roles.Add(new IdentityRole("Customer"));

            List<Category> categories = new List<Category>();
            categories.Add(new Category() { Name = "Mobile&Tablets" });
            categories.Add(new Category() { Name = "Electronics" });
            categories.Add(new Category() { Name = "Home,Kitchen,Tools" });
            categories.Add(new Category() { Name = "Sports&Fitness" });
            context.categories.AddRange(categories);
            context.SaveChanges();
            return Content("ok");
        }
    }
}