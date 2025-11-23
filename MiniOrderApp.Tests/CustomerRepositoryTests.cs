using Dapper;
using Microsoft.Data.Sqlite;
using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;
using MiniOrderApp.Infrastructure.Repositories;
using MiniOrderApp.Tests.Database;

namespace MiniOrderApp.Tests;

public class CustomerRepositoryTests
{
    private (ICustomerRepository repo, SqliteConnection conn) CreateRepository()
    {
        string connectionString = TestDatabaseFactory.CreateTestDatabase();

        var factory = new SQLiteConnectionFactory(connectionString);
        var repo = new CustomerRepository(factory);

        var conn = new SqliteConnection(connectionString);
        conn.Open();

        // Turn off foreign keys
        conn.Execute("PRAGMA foreign_keys = OFF;");

        return (repo, conn);
    }
    [Fact]
    public void GetCustomers_Should_Return_Customers_From_Database()
    {
        // Arrange
        var (repo, conn) = CreateRepository();

        const string insertSql = @"
            INSERT INTO Customers (Name, Email, Phone)
            VALUES (@Name, @Email, @Phone);
        ";

        conn.Execute(insertSql, new
        {
            Name = "Customer 1",
            Email = "customer1@test.com",
            Phone = "123456789"
        });

        conn.Execute(insertSql, new
        {
            Name = "Customer 2",
            Email = "customer2@test.com",
            Phone = "987654321"
        });

        // Act
        var customers = repo.GetCustomers().ToList();

        // Assert
        Assert.Equal(2, customers.Count);

        bool foundCustomer1 = false;
        bool foundCustomer2 = false;

        foreach (var customer in customers)
        {
            if (customer.Name == "Customer 1")
            {
                foundCustomer1 = true;
            }
            else if (customer.Name == "Customer 2")
            {
                foundCustomer2 = true;
            }

            if (foundCustomer1 && foundCustomer2)
            {
                break;
            }
        }

        Assert.True(foundCustomer1);
        Assert.True(foundCustomer2);
    }
    [Fact]
    public void GetById_Should_Return_Single_Customer()
    {
        // Arrange
        var (repo, conn) = CreateRepository();

        const string insertSql = @"
            INSERT INTO Customers (Name, Email, Phone)
            VALUES (@Name, @Email, @Phone);
            SELECT last_insert_rowid();
        ";

        long id = conn.ExecuteScalar<long>(insertSql, new
        {
            Name = "Single Customer",
            Email = "single@test.com",
            Phone = "123456789"
        });

        // Act
        var customer = repo.GetById((int)id);

        // Assert
        Assert.NotNull(customer);
        Assert.Equal((int)id, customer!.Id);
        Assert.Equal("Single Customer", customer.Name);
    }
    [Fact]
    public void Add_Should_Insert_Customer_In_Database()
    {
        // Arrange
        var (repo, conn) = CreateRepository();

        var customer = new Customer(
            "New Customer",
            "new@test.com",
            "123456789"
        );

        // Act
        repo.Add(customer);

        // Assert
        var customers = repo.GetCustomers().ToList();
        Assert.Single(customers);

        var saved = customers.Single();
        Assert.Equal("New Customer", saved.Name);
        Assert.Equal("new@test.com", saved.Email);
        Assert.Equal("123456789", saved.Phone);
    }
    [Fact]
    public void Delete_Should_Remove_Customer_From_Database()
    {
        // Arrange
        var (repo, conn) = CreateRepository();

        const string insertSql = @"
            INSERT INTO Customers (Name, Email, Phone)
            VALUES (@Name, @Email, @Phone);
            SELECT last_insert_rowid();
        ";

        long id = conn.ExecuteScalar<long>(insertSql, new
        {
            Name = "Customer To Delete",
            Email = "delete@test.com",
            Phone = "123456789"
        });

        // Act
        repo.Delete((int)id);

        // Assert
        var customers = repo.GetCustomers().ToList();
        bool customerExists = false;

        foreach (var c in customers)
        {
            if (c.Id == id)
            {
                customerExists = true;
            }
        }

        if (customerExists)
        {
            Assert.Fail("Customer should have been deleted but still exists.");
        }

    }

}


