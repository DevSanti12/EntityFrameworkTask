using EntityFrameworkTaskLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace EntityFrameworkTaskLibrary.DataAccess;

public class ProductContext: DbContext
{
    public ProductContext() { }

    public ProductContext(DbContextOptions<ProductContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the current working directory
                .AddJsonFile("appsettings.json") // Add the appsettings.json file
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            // Provide a fallback connection string for design-time tool use
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    // Define the relationship between `Order` and `Product`
    //    modelBuilder.Entity<Order>()
    //        .HasOne(o => o.ProductId) // Specify that Order has one Product
    //        .WithMany(p => p.Id)
    //        .HasForeignKey(o => o.ProductId); // Ensure no duplication

    //    base.OnModelCreating(modelBuilder);
    //}

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
}
