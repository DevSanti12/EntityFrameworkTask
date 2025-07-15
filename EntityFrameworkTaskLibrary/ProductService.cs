using System;
using Dapper;
using EntityFrameworkTaskLibrary.DataAccess;
using EntityFrameworkTaskLibrary.Interfaces;
using EntityFrameworkTaskLibrary.Models;

namespace EntityFrameworkTaskLibrary;

public class ProductService : IProductOperations
{
    private readonly ProductContext _context;

    public ProductService(ProductContext context)
    {
        _context = context;
    }

    // Create a product
    public void CreateProduct(string name, string description, float weight, float height, float width, float length)
    {
        var product = new Product
        {
            Name = name,
            Description = description,
            Weight = weight,
            Height = height,
            Width = width,
            Length = length
        };

        _context.Products.Add(product);
        _context.SaveChanges();
        Console.WriteLine("Product created successfully.");
    }

    // Get all products
    public IEnumerable<Product> GetAllProducts()
    {
        return _context.Products.ToList();
    }

    // Fetch product by name
    public IEnumerable<Product> FetchProduct(string name)
    {
        return _context.Products.Where(p => p.Name == name).ToList();
    }

    // Update product
    public void UpdateProduct(int id, string name, string description, float weight, float height, float width, float length)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            Console.WriteLine($"No product found with ID {id}.");
            return;
        }

        product.Name = name;
        product.Description = description;
        product.Weight = weight;
        product.Height = height;
        product.Width = width;
        product.Length = length;

        _context.Products.Update(product);
        _context.SaveChanges();
        Console.WriteLine($"Product with ID {id} updated successfully.");
    }

    // Delete product
    public void DeleteProduct(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            Console.WriteLine($"Product with ID {id} not found.");
            return;
        }

        _context.Products.Remove(product);
        _context.SaveChanges();
        Console.WriteLine($"Product with ID {id} deleted successfully.");
    }
}
