using Microsoft.EntityFrameworkCore;
namespace E_commerce.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        // DbSets go here, e.g.:
        // public DbSet<Product> Products { get; set; }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Faqs> Faqs { get; set; }

    }
}
