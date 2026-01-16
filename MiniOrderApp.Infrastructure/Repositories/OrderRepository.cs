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

    public IEnumerable<Order> GetOrders()
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"
            SELECT 
                OrderId AS Id,
                CustomerId,
                OrderDate,
                Status,
                TotalAmount
            FROM Orders;";

        return db.Query<Order>(sql);
    }

    public Order? GetById(int id)
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"
            SELECT
                OrderId AS Id,
                CustomerId,
                OrderDate,
                Status,
                TotalAmount
            FROM Orders
            WHERE OrderId = @Id;";

        return db.QueryFirstOrDefault<Order>(sql, new { Id = id });
    }

    public IEnumerable<OrderItem> GetItemsForOrder(int orderId)
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"
            SELECT
                OrderItemId AS Id,
                OrderId,
                ProductName,
                Quantity,
                UnitPrice
            FROM OrderItems
            WHERE OrderId = @OrderId;";

        return db.Query<OrderItem>(sql, new { OrderId = orderId });
    }

    public void Add(Order order)
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"
            INSERT INTO Orders (CustomerId, OrderDate, Status, TotalAmount)
            VALUES (@CustomerId, @OrderDate, @Status, @TotalAmount);";

        db.Execute(sql, new
        {
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate.ToString("yyyy-MM-dd"),
            Status = order.Status.ToString(),
            TotalAmount = order.TotalAmount
        });

        int orderId = db.ExecuteScalar<int>("SELECT last_insert_rowid();");

        const string sqlItems = @"
            INSERT INTO OrderItems (OrderId, ProductName, Quantity, UnitPrice)
            VALUES (@OrderId, @ProductName, @Quantity, @UnitPrice);";

        foreach (var item in order.Items)
        {
            db.Execute(sqlItems, new
            {
                OrderId = orderId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            });
        }
    }

    public void Update(Order order)
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"
            UPDATE Orders
            SET CustomerId = @CustomerId,
                OrderDate = @OrderDate,
                Status = @Status,
                TotalAmount = @TotalAmount
            WHERE OrderId = @Id;";

        db.Execute(sql, new
        {
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate.ToString("yyyy-MM-dd"),
            Status = order.Status.ToString(),
            TotalAmount = order.TotalAmount,
            Id = order.Id
        });
    }

    public void Delete(int id)
    {
        using IDbConnection db = _factory.Create();

        const string deleteItemsSql = @"DELETE FROM OrderItems WHERE OrderId = @Id;";
        db.Execute(deleteItemsSql, new { Id = id });

        const string deleteOrderSql = @"DELETE FROM Orders WHERE OrderId = @Id;";
        db.Execute(deleteOrderSql, new { Id = id });
    }

    public void MarkAsReturned(int orderId)
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"
            UPDATE Orders
            SET Status = @Status
            WHERE OrderId = @OrderId;";

        db.Execute(sql, new
        {
            Status = OrderStatus.Returned.ToString(),
            OrderId = orderId
        });
    }
}
