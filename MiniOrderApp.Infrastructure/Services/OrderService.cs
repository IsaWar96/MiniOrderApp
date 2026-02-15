using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;

namespace MiniOrderApp.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ApplicationDbContext _context;

    public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, ApplicationDbContext context)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetOrdersAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.", nameof(id));
        }

        var order = await _orderRepository.GetByIdAsync(id);

        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {id} not found.");
        }

        return order;
    }

    public async Task<Order> CreateOrderAsync(int customerId, List<OrderItem> items)
    {
        if (customerId <= 0)
        {
            throw new ArgumentException("Customer ID must be greater than zero.", nameof(customerId));
        }

        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
        }

        if (items == null || !items.Any())
        {
            throw new ArgumentException("Order must have at least one item.", nameof(items));
        }

        var totalAmount = items.Sum(item => item.Quantity * item.UnitPrice);
        var order = new Order(customerId, DateTime.UtcNow, totalAmount);
        
        foreach (var item in items)
        {
            order.AddItem(item);
        }

        await _orderRepository.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> UpdateOrderAsync(int id, int customerId, DateTime orderDate, OrderStatus status)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.", nameof(id));
        }

        var existingOrder = await _orderRepository.GetByIdAsync(id);

        if (existingOrder == null)
        {
            throw new KeyNotFoundException($"Order with ID {id} not found.");
        }

        if (customerId <= 0)
        {
            throw new ArgumentException("Customer ID must be greater than zero.", nameof(customerId));
        }

        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
        }

        existingOrder.UpdateDetails(customerId, orderDate, status);

        await _orderRepository.UpdateAsync(existingOrder);
        await _context.SaveChangesAsync();
        return existingOrder;
    }

    public async Task DeleteOrderAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.", nameof(id));
        }

        var order = await _orderRepository.GetByIdAsync(id);

        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {id} not found.");
        }

        await _orderRepository.DeleteAsync(id);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(int orderId)
    {
        if (orderId <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.", nameof(orderId));
        }

        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");
        }

        return await _orderRepository.GetItemsForOrderAsync(orderId);
    }

    public async Task MarkOrderAsReturnedAsync(int orderId)
    {
        if (orderId <= 0)
        {
            throw new ArgumentException("Order ID must be greater than zero.", nameof(orderId));
        }

        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order == null)
        {
            throw new KeyNotFoundException($"Order with ID {orderId} not found.");
        }

        order.MarkAsReturned();

        await _orderRepository.UpdateAsync(order);
        await _context.SaveChangesAsync();
    }
}
