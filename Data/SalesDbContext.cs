using Microsoft.EntityFrameworkCore;
using SalesAnalysis.Models;

namespace SalesAnalysis.Data
{
    public class SalesDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }

        // Constructor to accept DbContextOptions and pass to the base class
        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships (if needed)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductID)
                .IsRequired();
        }
    }
}
