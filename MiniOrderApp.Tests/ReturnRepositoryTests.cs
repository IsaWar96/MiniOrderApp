using Dapper;
using Microsoft.Data.Sqlite;
using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;
using MiniOrderApp.Infrastructure.Repositories;
using MiniOrderApp.Tests.Database;

namespace MiniOrderApp.Tests;

public class ReturnRepositoryTests
{
    private (IReturnRepository repo, SqliteConnection conn) CreateRepository()
    {
        string connectionString = TestDatabaseFactory.CreateTestDatabase();

        var factory = new SQLiteConnectionFactory(connectionString);
        var repo = new ReturnRepository(factory);

        var conn = new SqliteConnection(connectionString);
        conn.Open();

        //Turn off foreign keys
        conn.Execute("PRAGMA foreign_keys = OFF;");

        return (repo, conn);
    }
    [Fact]
    public void AddReturn_Then_GetByOrderId_Should_Return_Same_Data()
    {
        // Arrange
        var (repo, conn) = CreateRepository();

        const string insertOrderSql = @"
            INSERT INTO Orders (CustomerId, OrderDate, Status, TotalAmount)
            VALUES (1, '2025-01-01', 'Created', 100);
            SELECT last_insert_rowid();
        ";

        long orderId = conn.ExecuteScalar<long>(insertOrderSql);

        var returnInfo = new Return(
            (int)orderId,
            DateTime.Today,
            "Test reason",
            100m
        );

        // Act
        repo.AddReturn(returnInfo);

        var loaded = repo.GetByOrderId((int)orderId);

        // Assert
        Assert.NotNull(loaded);
        Assert.Equal((int)orderId, loaded!.OrderId);
        Assert.Equal("Test reason", loaded.Reason);
        Assert.Equal(100m, loaded.RefundedAmount);
    }


    [Fact]
    public void GetByOrderId_Should_Return_Null_When_No_Return_Exists()
    {
        // Arrange
        var (repo, conn) = CreateRepository();

        int orderId = 999;

        // Act
        var loaded = repo.GetByOrderId(orderId);

        // Assert
        Assert.Null(loaded);
    }
}

