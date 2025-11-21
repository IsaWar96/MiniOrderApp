
using System.Data;
using Dapper;
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
        using IDbConnection db = _factory.Create();

        const string sql = @"
        INSERT INTO Orders (CustomerId, OrderDate, Status, TotalAmount)
        VALUES (@CustomerId, @OrderDate, @Status, @TotalAmount);";

        db.Execute(sql, new
        {
            order.CustomerId,
            OrderDate = order.OrderDate.ToString("yyyy-MM-dd"),
            Status = order.Status.ToString(),
            order.TotalAmount
        });
    }

    public IEnumerable<Order> GetOrders()
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"
        SELECT 
            OrderId    AS Id,
            CustomerId,
            OrderDate,
            Status,
            TotalAmount
        FROM Orders;
        ";

        return db.Query<Order>(sql);
    }

    public Order? GetById(int id) => throw new NotImplementedException();
    public void Update(Order order) => throw new NotImplementedException();
    public void Delete(int id) => throw new NotImplementedException();
}
