using Market.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market.Models.Model;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.IO;

namespace Market.Services
{
    public  class services
    {
          public ApplicationDbContext context;
          UserManager<ApplicationUser> UserManager { get; set; }

        public services()
        {
            context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


        }

        public  List<Product> GetAllProductbySuppler()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var query = context.products.Where(c => c.supplierID == user.Id).ToList();
            //var user = context.Users.FindById(User.Identity.GetUserId());
            return (query);   
            
        }
        public  Category GetCategorieById(int id)
        {           
            return (context.categories.FirstOrDefault(c => c.Id == id));
        }
        public  List<Category> GetCategories()
        {
            return context.categories.ToList();
        }
        public  bool SearchProductName(string Name)
        {
           
            return context.products.Any(x => x.Name == Name);
        }    

      
        public  void addUserTOSupplier(string IdUser,string company)
        {          
           Supplier supplier = new Supplier() { supID = IdUser };
           if (company != null)
              supplier.Company = company;
            context.suppliers.Add(supplier);
            context.SaveChanges();
        }
        public  void addUserTOCustomer(string IdUser, string addrees)
        { 
            Customer customer = new Customer() { CusID = IdUser };
            if (addrees != null)
                customer.Address = addrees;
            context.customers.Add(customer);
            context.SaveChanges();
        }
        public  Supplier GetSupplierByID(string id)
        {
            return context.suppliers.FirstOrDefault(c=>c.supID==id);
        }
        public  int InsertProduct(Product p)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Product product = new Product();
            product.Name = p.Name;
            product.Price = p.Price;
            product.Quantity = p.Quantity;
            product.category = GetCategorieById(p.CategoryId);
            product.dateTime = DateTime.Now;
            product.supplier = GetSupplierByID(user.Id);
            if (p.Description != null)
                product.Description = p.Description;
            context.products.Add(product);
            context.SaveChanges();
            return product.ID;
        }
        public  void AddImageToProduct(Image image)
        {
            context.images.Add(image);
            context.SaveChanges();
        }
        public  Product GetProdectById(int id)
        {
            return context.products.FirstOrDefault(c => c.ID == id);   
        }
        public  int UpdateProduct(int id,Product p)
        {
            Product product= context.products.FirstOrDefault(c => c.ID == id);
            product.Name = p.Name;
            product.Price = p.Price;
            product.Quantity = p.Quantity;
            product.category = GetCategorieById(p.CategoryId);           
            if (p.Description != null)
                product.Description = p.Description;            
            context.SaveChanges();
            return product.ID;
        }
        public  List<Image> DeleteImagesFromProduct(int id)
        {
            List<Image>images=context.images.Where(c => c.ProductId == id).ToList();
            context.images.RemoveRange(images);
            context.SaveChanges();
            return images;
        }
        public  void DeleteProduct(int id)
        {          
            //delete comments
              DeleteCommentsFromProduct(id);
            //delete order details 
             DeleteOrderDetailsFromProduct(id);
            Product product = context.products.FirstOrDefault(c=>c.ID==id);
            context.products.Remove(product);
            context.SaveChanges();            
        }
        public  void DeleteCommentsFromProduct(int id)
        {
            List<Comment> comments = context.comments.Where(c => c.ProductId == id).ToList();
            context.comments.RemoveRange(comments);
            context.SaveChanges();
        }
        public  void DeleteOrderDetailsFromProduct(int id)
        {
            List<OrderDetails> orderDetails = context.OrderDetails.Where(c => c.ProductId == id).ToList();
            context.OrderDetails.RemoveRange(orderDetails);
            context.SaveChanges();
        }
        public  List<Product> GetProducts()
        {
            return context.products.ToList();
        }
        public  List<string> getAllSupplier()
        {
            return context.suppliers.Select(c=>c.user.UserName).ToList();
        }
        public  List<string> SearchProductByName(string Name)
        {
            return context.products.Where(c=>c.Name.StartsWith(Name)).Select(c=>c.Name).ToList();
        }
        public  List<Product> SearchByCategoryAndPrice(int CategoryId,int PriceP,int PriceA, string orderBy)
        {
            if (orderBy == "Descending")        
               return context.products.Where(c => c.CategoryId == CategoryId && c.Price >= PriceP && c.Price <= PriceA).OrderByDescending(c => c.Price).ToList();         
            else 
               return context.products.Where(c => c.CategoryId == CategoryId && c.Price >= PriceP && c.Price <= PriceA).OrderBy(c=>c.Price).ToList();

            
        }
        public List<Product> GetProdectByCategoryId(int id)
        {            
          return context.products.Where(c => c.CategoryId == id).ToList();
        }
        public List<Product> SearchUseingName(string Name)
        {
            return context.products.Where(c => c.Name == Name).ToList();
        }

    }
}