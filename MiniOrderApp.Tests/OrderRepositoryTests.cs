using Dapper;
using Microsoft.Data.Sqlite;
using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;
using MiniOrderApp.Infrastructure.Repositories;
using MiniOrderApp.Tests.Database;

namespace MiniOrderApp.Tests;

public class OrderRepositoryTests
{
    // Creates a repository plus and open SQLite connection
    private (IOrderRepository repo, SqliteConnection conn) CreateRepository()
    {
        string connectionString = TestDatabaseFactory.CreateTestDatabase();

        var factory = new SQLiteConnectionFactory(connectionString);
        var repo = new OrderRepository(factory);

        var conn = new SqliteConnection(connectionString);
        conn.Open();

        // Turn off foreign keys in the database
        conn.Execute("PRAGMA foreign_keys = OFF;");

        return (repo, conn);
    }

    [Fact]
    public void GetOrders_Should_Return_Order_From_Database()
    {
        // Arrange
        var (repo, conn) = CreateRepository();

        const string insertSql = @"
                INSERT INTO Orders (CustomerId, OrderDate, Status, TotalAmount)
                VALUES (@CustomerId, @OrderDate, @Status, @TotalAmount);
                SELECT last_insert_rowid();";

        // Places order directly in the database
        long orderId = conn.ExecuteScalar<long>(insertSql, new
        {
            CustomerId = 1,
            OrderDate = DateTime.Today.ToString("yyyy-MM-dd"),
            Status = "Created",
            TotalAmount = 200m
        });

        // Act
        var orders = repo.GetOrders().ToList();
        var order = orders.Single();

        // Assert
        Assert.Equal((int)orderId, order.Id);
        Assert.Equal(1, order.CustomerId);
        Assert.Equal(OrderStatus.Created, order.Status);
    }
    [Fact]
    public void Add_Should_Insert_Order_In_Database()
    {
        // Arrange
        var (repo, conn) = CreateRepository();

        // create a test customer
        const string insertCustomerSql = @"
            INSERT INTO Customers (Name, Email, Phone)
            VALUES (@Name, @Email, @Phone);
            SELECT last_insert_rowid();";

        long customerId = conn.ExecuteScalar<long>(insertCustomerSql, new
        {
            Name = "Test Customer",
            Email = "test@example.com",
            Phone = "123456789"
        });

        // Create a order in the domain layer
        var order = new Order(
            (int)customerId,
            DateTime.Today,
            200m
        );

        // Act
        repo.Add(order);

        // Assert
        var orders = repo.GetOrders().ToList();

        Assert.Single(orders);
        var saved = orders.Single();

        Assert.Equal((int)customerId, saved.CustomerId);
        Assert.Equal(OrderStatus.Created, saved.Status);
        Assert.Equal(200m, saved.TotalAmount);
    }
    [Fact]
    public void Delete_Should_Remove_Order_From_Database()
    {
        // Arrange
        var (repo, conn) = CreateRepository();

        const string insertSql = @"
            INSERT INTO Orders (CustomerId, OrderDate, Status, TotalAmount)
            VALUES (@CustomerId, @OrderDate, @Status, @TotalAmount);
            SELECT last_insert_rowid();";

        long orderId = conn.ExecuteScalar<long>(insertSql, new
        {
            CustomerId = 1,
            OrderDate = DateTime.Today.ToString("yyyy-MM-dd"),
            Status = OrderStatus.Created,
            TotalAmount = 200m
        });

        // Act
        repo.Delete((int)orderId);

        // Assert
        var orders = repo.GetOrders().ToList();
        Assert.Empty(orders);
    }
    [Fact]
    public void MarkAsReturned_Should_Update_Status_To_Returned()
    {
        // Arrange
        var (repo, conn) = CreateRepository();

        const string insertSql = @"
            INSERT INTO Orders (CustomerId, OrderDate, Status, TotalAmount)
            VALUES (@CustomerId, @OrderDate, @Status, @TotalAmount);
            SELECT last_insert_rowid();";

        long orderId = conn.ExecuteScalar<long>(insertSql, new
        {
            CustomerId = 1,
            OrderDate = DateTime.Today.ToString("yyyy-MM-dd"),
            Status = OrderStatus.Created,
            TotalAmount = 200m
        });

        // Act
        repo.MarkAsReturned((int)orderId);

        // Assert
        var updated = repo.GetById((int)orderId);

        Assert.NotNull(updated);
        Assert.Equal(OrderStatus.Returned, updated!.Status);
    }
}

