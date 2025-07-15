using System;
using EntityFrameworkTaskLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkTaskLibraryUnitTests;
public class TestDbContext : IDisposable
{
    public ProductContext Context { get; private set; }

    public TestDbContext()
    {
        var options = new DbContextOptionsBuilder<ProductContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use unique database for each test run
        .Options;

        Context = new ProductContext(options);

        SeedDatabase();
    }

    public void Dispose()
    {
        Context?.Dispose();
    }

    // Seed initial data (if needed for tests)
    private void SeedDatabase()
    {
        // You can seed your database for specific tests here
    }
}