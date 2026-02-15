using Microsoft.EntityFrameworkCore;
using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;

namespace MiniOrderApp.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Order> GetOrders()
    {
        return _context.Orders.Include(o => o.Items).ToList();
    }

    public Order? GetById(int id)
    {
        return _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == id);
    }

    public IEnumerable<OrderItem> GetItemsForOrder(int orderId)
    {
        return _context.OrderItems.Where(i => i.OrderId == orderId).ToList();
    }

    public void Add(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
    }

    public void Update(Order order)
    {
        _context.Orders.Update(order);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var order = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
    }

    public void MarkAsReturned(int orderId)
    {
        var order = _context.Orders.Find(orderId);
        if (order != null)
        {
            order.SetStatus(OrderStatus.Returned);
            _context.SaveChanges();
        }
    }
}
