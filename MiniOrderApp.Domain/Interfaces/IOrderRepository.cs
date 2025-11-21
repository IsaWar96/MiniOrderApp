namespace MiniOrderApp.Domain.Interfaces;

public interface IOrderRepository
{
    IEnumerable<Order> GetOrders();
    Order? GetById(int id);
    void Add(Order order);
    void Update(Order order);
    void Delete(int id);

    IEnumerable<OrderItem> GetItemsForOrder(int orderId);

    void MarkAsReturned(int orderId);
}

