using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Interfaces;

namespace MiniOrderApp.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;

    public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }

    public IEnumerable<Order> GetAllOrders()
    {
        return _orderRepository.GetOrders();
    }

    public Order GetOrderById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.", nameof(id));
        }

        var order = _orderRepository.GetById(id);
        
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {id} not found.");
        }

        return order;
    }

    public Order CreateOrder(Order order)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order), "Order cannot be null.");
        }

        if (order.CustomerId <= 0)
        {
            throw new ArgumentException("Customer ID must be greater than zero.", nameof(order.CustomerId));
        }

        var customer = _customerRepository.GetById(order.CustomerId);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {order.CustomerId} not found.");
        }

        if (order.Items == null || !order.Items.Any())
        {
            throw new ArgumentException("Order must have at least one item.", nameof(order.Items));
        }

        foreach (var item in order.Items)
        {
            if (string.IsNullOrWhiteSpace(item.ProductName))
            {
                throw new ArgumentException("Product name is required for all order items.");
            }

            if (item.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero for all order items.");
            }

            if (item.UnitPrice < 0)
            {
                throw new ArgumentException("Unit price cannot be negative for order items.");
            }
        }

        order.TotalAmount = order.Items.Sum(item => item.Quantity * item.UnitPrice);
        order.OrderDate = DateTime.Now;
        order.Status = OrderStatus.Created;

        _orderRepository.Add(order);
        return order;
    }

    public Order UpdateOrder(int id, Order order)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.", nameof(id));
        }

        if (order == null)
        {
            throw new ArgumentNullException(nameof(order), "Order cannot be null.");
        }

        var existingOrder = _orderRepository.GetById(id);
        
        if (existingOrder == null)
        {
            throw new KeyNotFoundException($"Order with ID {id} not found.");
        }

        if (order.CustomerId <= 0)
        {
            throw new ArgumentException("Customer ID must be greater than zero.", nameof(order.CustomerId));
        }

        var customer = _customerRepository.GetById(order.CustomerId);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {order.CustomerId} not found.");
        }

        if (order.Items == null || !order.Items.Any())
        {
            throw new ArgumentException("Order must have at least one item.", nameof(order.Items));
        }

        foreach (var item in order.Items)
        {
            if (string.IsNullOrWhiteSpace(item.ProductName))
            {
                throw new ArgumentException("Product name is required for all order items.");
            }

            if (item.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero for all order items.");
            }

            if (item.UnitPrice < 0)
            {
                throw new ArgumentException("Unit price cannot be negative for order items.");
            }
        }

        order.TotalAmount = order.Items.Sum(item => item.Quantity * item.UnitPrice);
        order.Id = id;

        _orderRepository.Update(order);
        return order;
    }

    public void DeleteOrder(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.", nameof(id));
        }

        var order = _orderRepository.GetById(id);
        
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {id} not found.");
        }

        _orderRepository.Delete(id);
    }

    public IEnumerable<OrderItem> GetOrderItems(int orderId)
    {
        if (orderId <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.", nameof(orderId));
        }

        var order = _orderRepository.GetById(orderId);
        
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");
        }

        return _orderRepository.GetItemsForOrder(orderId);
    }

    public void MarkOrderAsReturned(int orderId)
    {
        if (orderId <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.", nameof(orderId));
        }

        var order = _orderRepository.GetById(orderId);
        
        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");
        }

        if (order.Status == OrderStatus.Returned)
        {
            throw new InvalidOperationException($"Order with ID {orderId} is already marked as returned.");
        }

        _orderRepository.MarkAsReturned(orderId);
    }
}
