using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;

namespace MiniOrderApp.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly SQLiteConnectionFactory _factory;
    public OrderRepository(SQLiteConnectionFactory factory)
    {
        _factory = factory;
    }
    public void Add(Order order)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Order? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Order> GetOrders()
    {
        throw new NotImplementedException();
    }

    public void Update(Order order)
    {
        throw new NotImplementedException();
    }
}
