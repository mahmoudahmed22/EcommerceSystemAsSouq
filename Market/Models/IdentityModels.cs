using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Market.Models.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Market.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Image { get; set; }
        public string FullName { get; set; }
        //public byte[] Userphoto { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }

        public virtual DbSet<Product> products { get; set; }
        public virtual DbSet<Category> categories { get; set; }
        public virtual DbSet<Comment> comments { get; set; }
        public virtual DbSet<Customer> customers { get; set; }
        public virtual DbSet<Image> images { get; set; }
        public virtual DbSet<Order> orders { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Supplier> suppliers { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

       
    }
    public class ApplicationDBInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            List<IdentityRole> roles = new List<IdentityRole>();
            roles.Add(new IdentityRole() {Name= "Admin" });
            roles.Add(new IdentityRole() { Name = "Supplier" });
            roles.Add(new IdentityRole() { Name = "Customer" });
            RoleManager<IdentityRole> manager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            foreach (IdentityRole item in roles)
            {
                manager.Create(item);
            }
            List<Category> categories = new List<Category>();
            categories.Add(new Category() { Name = "Mobiles" });
            categories.Add(new Category() { Name = "Televisions" });
            categories.Add(new Category() { Name = "Laptops" });
            context.categories.AddRange(categories);
            base.Seed(context);
        }
    }
}