using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;

namespace MiniOrderApp.Tests.Fakes;

public class FakeOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();
    private readonly List<OrderItem> _orderItems = new();
    private int _nextOrderId = 1;
    private int _nextOrderItemId = 1;

    public IEnumerable<Order> GetOrders()
    {
        var orders = _orders.ToList();
        foreach (var order in orders)
        {
            order.Items.Clear();
            order.Items.AddRange(_orderItems.Where(item => item.OrderId == order.Id));
        }
        return orders;
    }

    public Order? GetById(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order != null)
        {
            order.Items.Clear();
            order.Items.AddRange(_orderItems.Where(item => item.OrderId == order.Id));
        }
        return order;
    }

    public void Add(Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        
        order.Id = _nextOrderId++;
        _orders.Add(order);
        
        // Add order items (create copies to avoid reference issues)
        var itemsToAdd = order.Items.ToList();
        order.Items.Clear();
        
        foreach (var item in itemsToAdd)
        {
            var itemCopy = new OrderItem(item.ProductName, item.Quantity, item.UnitPrice)
            {
                Id = _nextOrderItemId++,
                OrderId = order.Id
            };
            _orderItems.Add(itemCopy);
            order.Items.Add(itemCopy);
        }
    }

    public void Update(Order order)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        
        var existing = _orders.FirstOrDefault(o => o.Id == order.Id);
        if (existing == null)
            throw new InvalidOperationException($"Order with ID {order.Id} not found");
        
        _orders.Remove(existing);
        
        // Remove existing order items
        _orderItems.RemoveAll(item => item.OrderId == order.Id);
        
        _orders.Add(order);
        
        // Add updated order items (create copies to avoid reference issues)
        var itemsToUpdate = order.Items.ToList();
        order.Items.Clear();
        
        foreach (var item in itemsToUpdate)
        {
            if (item.Id == 0)
            {
                item.Id = _nextOrderItemId++;
            }
            item.OrderId = order.Id;
            _orderItems.Add(item);
            order.Items.Add(item);
        }
    }

    public void Delete(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order != null)
        {
            _orders.Remove(order);
        }
        
        // Remove associated order items
        _orderItems.RemoveAll(item => item.OrderId == id);
    }

    public IEnumerable<OrderItem> GetItemsForOrder(int orderId)
    {
        return _orderItems.Where(item => item.OrderId == orderId).ToList();
    }

    public void MarkAsReturned(int orderId)
    {
        var order = _orders.FirstOrDefault(o => o.Id == orderId);
        if (order != null)
        {
            order.SetStatus(OrderStatus.Returned);
        }
    }
}
