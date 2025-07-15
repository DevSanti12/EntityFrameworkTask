using System;
using System.Linq;
using EntityFrameworkTaskLibrary.Models;
using EntityFrameworkTaskLibrary;
using Xunit;

namespace EntityFrameworkTaskLibraryUnitTests;

public class ProductServiceTests : IDisposable
{
    private readonly TestDbContext _testDbContext;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _testDbContext = new TestDbContext();
        _productService = new ProductService(_testDbContext.Context);
    }

    [Fact]
    public void CreateProduct_Should_Insert_Product_Into_Database()
    {
        // Arrange
        var name = "Test Product";
        var description = "Test Description";
        var weight = 1.5f;
        var height = 2.0f;
        var width = 3.0f;
        var length = 4.0f;

        // Act
        _productService.CreateProduct(name, description, weight, height, width, length);

        // Assert
        var product = _testDbContext.Context.Products.FirstOrDefault(p => p.Name == name);
        Assert.NotNull(product);
        Assert.Equal(description, product.Description);
        Assert.Equal(weight, product.Weight);
        Assert.Equal(height, product.Height);
        Assert.Equal(width, product.Width);
        Assert.Equal(length, product.Length);
    }

    [Fact]
    public void FetchProduct_Should_Return_Product_By_Name()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Product",
            Description = "Test Description",
            Weight = 1.5f,
            Height = 2.0f,
            Width = 3.0f,
            Length = 4.0f
        };

        _testDbContext.Context.Products.Add(product);
        _testDbContext.Context.SaveChanges();

        // Act
        var fetchedProducts = _productService.FetchProduct(product.Name);

        // Assert
        Assert.NotNull(fetchedProducts);
        Assert.Single(fetchedProducts);
        var fetchedProduct = fetchedProducts.First();
        Assert.Equal(product.Name, fetchedProduct.Name);
        Assert.Equal(product.Description, fetchedProduct.Description);
    }

    [Fact]
    public void GetAllProducts_Should_Return_All_Products()
    {
        // Arrange
        var products = new[]
        {
            new Product { Name = "Product 1", Description = "Description 1", Weight = 1.0f, Height = 2.0f, Width = 3.0f, Length = 4.0f },
            new Product { Name = "Product 2", Description = "Description 2", Weight = 2.0f, Height = 3.0f, Width = 4.0f, Length = 5.0f },
            new Product { Name = "Product 3", Description = "Description 3", Weight = 3.0f, Height = 4.0f, Width = 5.0f, Length = 6.0f },
        };
        _testDbContext.Context.Products.AddRange(products);
        _testDbContext.Context.SaveChanges();

        // Act
        var allProducts = _productService.GetAllProducts();

        // Assert
        Assert.NotEmpty(allProducts);
        Assert.Equal(3, allProducts.Count());
        Assert.Contains(allProducts, p => p.Name == "Product 1");
        Assert.Contains(allProducts, p => p.Name == "Product 2");
        Assert.Contains(allProducts, p => p.Name == "Product 3");
    }

    [Fact]
    public void UpdateProduct_Should_Modify_Existing_Product()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Product",
            Description = "Initial Description",
            Weight = 1.5f,
            Height = 2.0f,
            Width = 3.0f,
            Length = 4.0f
        };

        _testDbContext.Context.Products.Add(product);
        _testDbContext.Context.SaveChanges();

        var updatedName = "Updated Product";
        var updatedDescription = "Updated Description";
        var updatedWeight = 2.5f;
        var updatedHeight = 3.0f;
        var updatedWidth = 4.0f;
        var updatedLength = 5.0f;

        // Act
        _productService.UpdateProduct(product.Id, updatedName, updatedDescription, updatedWeight, updatedHeight, updatedWidth, updatedLength);

        // Assert
        var updatedProduct = _testDbContext.Context.Products.Find(product.Id);
        Assert.NotNull(updatedProduct);
        Assert.Equal(updatedName, updatedProduct.Name);
        Assert.Equal(updatedDescription, updatedProduct.Description);
        Assert.Equal(updatedWeight, updatedProduct.Weight);
        Assert.Equal(updatedHeight, updatedProduct.Height);
        Assert.Equal(updatedWidth, updatedProduct.Width);
        Assert.Equal(updatedLength, updatedProduct.Length);
    }

    [Fact]
    public void DeleteProduct_Should_Remove_Product_From_Database()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Product",
            Description = "Test Description",
            Weight = 1.5f,
            Height = 2.0f,
            Width = 3.0f,
            Length = 4.0f
        };

        _testDbContext.Context.Products.Add(product);
        _testDbContext.Context.SaveChanges();

        // Act
        _productService.DeleteProduct(product.Id);

        // Assert
        var deletedProduct = _testDbContext.Context.Products.Find(product.Id);
        Assert.Null(deletedProduct);
    }

    public void Dispose()
    {
        _testDbContext.Dispose();
    }
}
