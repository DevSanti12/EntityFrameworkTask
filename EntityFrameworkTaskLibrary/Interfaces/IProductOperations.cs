using EntityFrameworkTaskLibrary.Models;

namespace EntityFrameworkTaskLibrary.Interfaces;
public interface IProductOperations
{
    void CreateProduct(string name, string description, float weight, float height, float width, float length);
    IEnumerable<Product> FetchProduct(string name);
    IEnumerable<Product> GetAllProducts();
    void UpdateProduct(int id, string name, string description, float weight, float height, float width, float length);
    void DeleteProduct(int id);
}

