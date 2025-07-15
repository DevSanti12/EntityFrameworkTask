using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTaskLibraryUnitTests;

using System;
using System.Linq;
using EntityFrameworkTaskLibrary.Models;
using EntityFrameworkTaskLibrary;
using Xunit;

public class OrderServiceTests : IDisposable
{
    private readonly TestDbContext _testDbContext;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _testDbContext = new TestDbContext();
        _orderService = new OrderService(_testDbContext.Context);
    }

    [Fact]
    public void CreateOrder_Should_Insert_Order_Into_Database()
    {
        // Arrange
        var product = new Product { Name = "Test Product", Description = "Desc", Weight = 1.5f, Height = 2.0f, Width = 2.0f, Length = 3.0f };
        _testDbContext.Context.Products.Add(product);
        _testDbContext.Context.SaveChanges();

        var status = OrderStatus.InProgress;
        var createdDate = DateTime.Now;
        var updatedDate = createdDate.AddMinutes(5);

        // Act
        _orderService.CreateOrder(status, createdDate, updatedDate, product.Id);

        // Assert
        var order = _testDbContext.Context.Orders.FirstOrDefault(o => o.Status == status && o.ProductId == product.Id);
        Assert.NotNull(order);
        Assert.Equal(product.Id, order.ProductId);
        Assert.Equal(status, order.Status);
    }

    [Fact]
    public void FetchOrderById_Should_Return_Correct_Order()
    {
        // Arrange
        var product = new Product { Name = "Test Product", Description = "Desc", Weight = 1.5f, Height = 2.0f, Width = 2.0f, Length = 3.0f };
        var order = new Order
        {
            Status = OrderStatus.InProgress,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            Product = product
        };
        _testDbContext.Context.Products.Add(product);
        _testDbContext.Context.Orders.Add(order);
        _testDbContext.Context.SaveChanges();

        // Act
        var fetchedOrder = _orderService.FetchOrderById(order.Id);

        // Assert
        Assert.NotNull(fetchedOrder);
        Assert.Equal(order.Id, fetchedOrder.Id);
        Assert.Equal(OrderStatus.InProgress, fetchedOrder.Status);
    }

    [Fact]
    public void FetchOrdersByStatus_Should_Return_Correct_Orders()
    {
        // Arrange
        var product = new Product { Name = "Test Product", Description = "Desc", Weight = 1.5f, Height = 2.0f, Width = 2.0f, Length = 3.0f };
        var orders = new[]
        {
            new Order { Status = OrderStatus.InProgress, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, Product = product },
            new Order { Status = OrderStatus.Done, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, Product = product },
            new Order { Status = OrderStatus.InProgress, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now, Product = product }
        };
        _testDbContext.Context.Products.Add(product);
        _testDbContext.Context.Orders.AddRange(orders);
        _testDbContext.Context.SaveChanges();

        // Act
        var inProgressOrders = _orderService.FetchOrdersByStatus(OrderStatus.InProgress);

        // Assert
        Assert.NotNull(inProgressOrders);
        Assert.Equal(2, inProgressOrders.Count());
        Assert.All(inProgressOrders, o => Assert.Equal(OrderStatus.InProgress, o.Status));
    }

    [Fact]
    public void UpdateOrder_Should_Modify_Existing_Order()
    {
        // Arrange
        var product = new Product { Name = "Test Product", Description = "Desc", Weight = 1.5f, Height = 2.0f, Width = 2.0f, Length = 3.0f };
        var order = new Order
        {
            Status = OrderStatus.InProgress,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            Product = product
        };
        _testDbContext.Context.Products.Add(product);
        _testDbContext.Context.Orders.Add(order);
        _testDbContext.Context.SaveChanges();

        // Act
        var newStatus = OrderStatus.Done;
        _orderService.UpdateOrder(order.Id, newStatus, order.CreatedDate, DateTime.Now, product.Id);

        // Assert
        var updatedOrder = _testDbContext.Context.Orders.Find(order.Id);
        Assert.NotNull(updatedOrder);
        Assert.Equal(newStatus, updatedOrder.Status);
    }

    [Fact]
    public void DeleteOrder_Should_Remove_Order_From_Database()
    {
        // Arrange
        var product = new Product { Name = "Test Product", Description = "Desc", Weight = 1.5f, Height = 2.0f, Width = 2.0f, Length = 3.0f };
        var order = new Order
        {
            Status = OrderStatus.InProgress,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            Product = product
        };
        _testDbContext.Context.Products.Add(product);
        _testDbContext.Context.Orders.Add(order);
        _testDbContext.Context.SaveChanges();

        // Act
        _orderService.DeleteOrder(order.Id);

        // Assert
        var deletedOrder = _testDbContext.Context.Orders.Find(order.Id);
        Assert.Null(deletedOrder);
    }

    [Fact]
    public void FetchFilteredOrders_Should_Return_Correct_Orders()
    {
        // Arrange
        var product = new Product { Name = "Test Product", Description = "Desc", Weight = 1.5f, Height = 2.0f, Width = 2.0f, Length = 3.0f };
        var orders = new[]
        {
            new Order { Status = OrderStatus.InProgress, CreatedDate = new DateTime(2023, 1, 15), UpdatedDate = DateTime.Now, Product = product },
            new Order { Status = OrderStatus.Done, CreatedDate = new DateTime(2023, 2, 20), UpdatedDate = DateTime.Now, Product = product },
            new Order { Status = OrderStatus.InProgress, CreatedDate = new DateTime(2022, 8, 10), UpdatedDate = DateTime.Now, Product = product }
        };
        _testDbContext.Context.Products.Add(product);
        _testDbContext.Context.Orders.AddRange(orders);
        _testDbContext.Context.SaveChanges();

        // Act
        var filteredOrders = _orderService.FetchFilteredOrders(year: 2023, status: OrderStatus.InProgress);

        // Assert
        Assert.NotEmpty(filteredOrders);
        Assert.Single(filteredOrders);
        Assert.All(filteredOrders, o => Assert.Equal(OrderStatus.InProgress, o.Status));
    }

    [Fact]
    public void DeleteOrdersInBulk_Should_Remove_Matching_Orders()
    {
        // Arrange
        var product = new Product { Name = "Test Product", Description = "Desc", Weight = 1.5f, Height = 2.0f, Width = 2.0f, Length = 3.0f };
        var orders = new[]
        {
            new Order { Status = OrderStatus.InProgress, CreatedDate = new DateTime(2023, 1, 15), UpdatedDate = DateTime.Now, Product = product },
            new Order { Status = OrderStatus.Done, CreatedDate = new DateTime(2023, 2, 20), UpdatedDate = DateTime.Now, Product = product },
            new Order { Status = OrderStatus.InProgress, CreatedDate = new DateTime(2022, 8, 10), UpdatedDate = DateTime.Now, Product = product }
        };
        _testDbContext.Context.Products.Add(product);
        _testDbContext.Context.Orders.AddRange(orders);
        _testDbContext.Context.SaveChanges();

        // Act
        _orderService.DeleteOrdersInBulk(year: 2023);

        // Assert
        var remainingOrders = _testDbContext.Context.Orders.ToList();
        Assert.Single(remainingOrders);
        Assert.Equal(2022, remainingOrders.First().CreatedDate.Year);
    }

    public void Dispose()
    {
        _testDbContext.Dispose();
    }
}
