using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Market.Models;
using Market.Models.Model;
using Market.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Market.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ApplicationUsersController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext context = new ApplicationDbContext();
        RoleManager<IdentityRole> rolemanager;
        
        public ApplicationUsersController()
        {
            rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        }

        public ApplicationUsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
      
        
        public ActionResult Index()
        {

           IdentityRole ad= rolemanager.Roles.FirstOrDefault(c => c.Id == "1");
            return View(UserManager.Users.ToList());
        }

        // GET: Admin/ApplicationUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = UserManager.Users.FirstOrDefault(c=>c.Id==id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Admin/ApplicationUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Image,FullName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
              
                UserManager.Create(applicationUser);
               
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: Admin/ApplicationUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = UserManager.Users.FirstOrDefault(c => c.Id == id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Admin/ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Image,FullName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                context.Entry(applicationUser).State = EntityState.Modified;
                
               context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: Admin/ApplicationUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = UserManager.Users.FirstOrDefault(c => c.Id == id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Admin/ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  ActionResult DeleteConfirmed(string id)
        {
            var user =  UserManager.FindById(id);          
            var logins = user.Logins;            
           var rolesForUser =  UserManager.GetRoles(id);

           
                foreach (var login in logins.ToList())
                {
                 var t=    UserManager.RemoveLogin(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result =  UserManager.RemoveFromRole(user.Id, item);
                    }
                }
               Supplier supplier=  context.suppliers.FirstOrDefault(c=>c.supID==user.Id);
               Customer customer= context.customers.FirstOrDefault(c => c.CusID == user.Id);
                services services = new services();
                if(supplier!=null)
                {
                    List<Product> products = context.products.Where(c => c.supplierID == supplier.supID).ToList();
                    foreach (var item in products)
                    {
                        services.DeleteProduct(item.ID);

                    }
                    context.suppliers.Remove(supplier);
                    context.SaveChanges();
                }
                if (customer != null)
                {
                    List<Comment> comments = context.comments.Where(c => c.CustomerId == customer.CusID).ToList();
                    foreach (var item in comments)
                    {
                        context.comments.Remove(item);
                        context.SaveChanges();
                    }
                    List<Order> orders = context.orders.Where(c => c.CustomerId == customer.CusID).ToList();
                    foreach (var item in orders)
                    {
                        List<OrderDetails> orderDetails = context.OrderDetails.Where(c=>c.OrderId==item.Id).ToList();
                        foreach (var i in orderDetails)
                        {
                            context.OrderDetails.Remove(i);
                            context.SaveChanges();
                        }
                        context.orders.Remove(item);
                          context.SaveChanges();
                    }
                   context.customers.Remove(customer);
                   context.SaveChanges();
            }
                //Delete User
                
                 UserManager.Delete(user);

                

        
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
