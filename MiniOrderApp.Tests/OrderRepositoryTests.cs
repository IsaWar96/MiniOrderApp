using Dapper;
using Microsoft.Data.Sqlite;
using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;
using MiniOrderApp.Infrastructure.Repositories;
using MiniOrderApp.Tests.Database;

namespace MiniOrderApp.Tests
{
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
    }
}
