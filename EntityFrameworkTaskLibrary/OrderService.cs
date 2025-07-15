using System;
using System.Data;
using EntityFrameworkTaskLibrary.DataAccess;
using EntityFrameworkTaskLibrary.Interfaces;
using EntityFrameworkTaskLibrary.Models;

namespace EntityFrameworkTaskLibrary;

public class OrderService: IOrderOperations
{
    private readonly ProductContext _context;

    public OrderService(ProductContext context)
    {
        _context = context;
    }

    // Create a new order
    public void CreateOrder(OrderStatus status, DateTime createdDate, DateTime updatedDate, int productId)
    {
        var order = new Order
        {
            Status = status,
            CreatedDate = createdDate,
            UpdatedDate = updatedDate,
            ProductId = productId
        };

        _context.Orders.Add(order);
        _context.SaveChanges();
        Console.WriteLine("Order created successfully.");
    }

    // Delete order
    public void DeleteOrder(int id)
    {
        var order = _context.Orders.Find(id);
        if (order == null)
        {
            Console.WriteLine($"Order with ID {id} not found.");
            return;
        }

        _context.Orders.Remove(order);
        _context.SaveChanges();
        Console.WriteLine($"Order with ID {id} deleted successfully.");
    }

    // Fetch orders by status
    public IEnumerable<Order> FetchOrdersByStatus(OrderStatus status)
    {
        return _context.Orders.Where(o => o.Status == status).ToList();
    }

    // Fetch order by ID
    public Order FetchOrderById(int id)
    {
        return _context.Orders.Find(id);
    }

    // Get all orders
    public IEnumerable<Order> GetAllOrders()
    {
        return _context.Orders.ToList();
    }

    // Update order
    public void UpdateOrder(int id, OrderStatus status, DateTime createdDate, DateTime updatedDate, int productId)
    {
        var order = _context.Orders.Find(id);
        if (order == null)
        {
            Console.WriteLine($"Order with ID {id} not found.");
            return;
        }

        order.Status = status;
        order.CreatedDate = createdDate;
        order.UpdatedDate = updatedDate;
        order.ProductId = productId;

        _context.Orders.Update(order);
        _context.SaveChanges();
        Console.WriteLine($"Order with ID {id} updated successfully.");
    }

    // Fetch filtered orders
    public IEnumerable<Order> FetchFilteredOrders(int? year = null, int? month = null, OrderStatus? status = null, int? productId = null)
    {
        var query = _context.Orders.AsQueryable();

        if (year.HasValue)
            query = query.Where(o => o.CreatedDate.Year == year.Value);

        if (month.HasValue)
            query = query.Where(o => o.CreatedDate.Month == month.Value);

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        if (productId.HasValue)
            query = query.Where(o => o.ProductId == productId.Value);

        return query.ToList();
    }

    // Delete orders in bulk by filter
    public void DeleteOrdersInBulk(int? year = null, int? month = null, OrderStatus? status = null, int? productId = null)
    {
        var query = _context.Orders.AsQueryable();

        if (year.HasValue)
            query = query.Where(o => o.CreatedDate.Year == year.Value);

        if (month.HasValue)
            query = query.Where(o => o.CreatedDate.Month == month.Value);

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        if (productId.HasValue)
            query = query.Where(o => o.ProductId == productId.Value);

        _context.Orders.RemoveRange(query);
        _context.SaveChanges();
        Console.WriteLine("Orders deleted successfully in bulk.");
    }
}