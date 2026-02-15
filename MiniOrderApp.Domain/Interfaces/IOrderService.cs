using MiniOrderApp.Domain;

namespace MiniOrderApp.Domain.Interfaces;

public interface IOrderService
{
    IEnumerable<Order> GetAllOrders();
    Order GetOrderById(int id);
    Order CreateOrder(Order order);
    Order UpdateOrder(int id, Order order);
    void DeleteOrder(int id);
    IEnumerable<OrderItem> GetOrderItems(int orderId);
    void MarkOrderAsReturned(int orderId);
}
