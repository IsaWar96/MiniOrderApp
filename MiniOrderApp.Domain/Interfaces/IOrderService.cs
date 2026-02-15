using MiniOrderApp.Domain;

namespace MiniOrderApp.Domain.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order> GetOrderByIdAsync(int id);
    Task<Order> CreateOrderAsync(int customerId, List<OrderItem> items);
    Task<Order> UpdateOrderAsync(int id, int customerId, DateTime orderDate, OrderStatus status);
    Task DeleteOrderAsync(int id);
    Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId);
    Task MarkOrderAsReturnedAsync(int orderId);
}
