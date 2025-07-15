using EntityFrameworkTaskLibrary.Models;
namespace EntityFrameworkTaskLibrary.Interfaces
{
    public interface IOrderOperations
    {
        public void CreateOrder(OrderStatus status, DateTime createdDate, DateTime updatedDate, int productid);

        public IEnumerable<Order> FetchOrdersByStatus(OrderStatus status);

        public IEnumerable<Order> GetAllOrders();

        public void UpdateOrder(int id, OrderStatus status, DateTime createdDate, DateTime updatedDate, int productid);

        public void DeleteOrder(int id);

        public void DeleteOrdersInBulk(int? year = null, int? month = null, OrderStatus? status = null, int? productId = null);

        public IEnumerable<Order> FetchFilteredOrders(int? year = null, int? month = null, OrderStatus? status = null, int? productId = null);
    }
}
