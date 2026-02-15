namespace MiniOrderApp.Domain.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order?> GetByIdAsync(int id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(int id);

    Task<IEnumerable<OrderItem>> GetItemsForOrderAsync(int orderId);
}

