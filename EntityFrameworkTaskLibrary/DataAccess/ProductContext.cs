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
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
}
