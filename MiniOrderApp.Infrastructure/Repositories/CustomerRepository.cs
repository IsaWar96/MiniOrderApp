using System.Data;
using Dapper;
using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;

namespace MiniOrderApp.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly SQLiteConnectionFactory _factory;
    public CustomerRepository(SQLiteConnectionFactory factory)
    {
        _factory = factory;
    }
    public IEnumerable<Customer> GetCustomers()
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"
        SELECT
            CustomerId AS Id,
            Name,
            Email,
            Phone
        FROM Customers;
        ";

        return db.Query<Customer>(sql);
    }
    public void Add(Customer customer)
    {
        using var db = _factory.Create();

        const string sql = @"
        INSERT INTO Customers (Name, Email, Phone)
        VALUES (@Name, @Email, @Phone);
        ";

        db.Execute(sql, new
        {
            customer.Name,
            customer.Email,
            customer.Phone
        });
    }

    public void Delete(int id)
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"DELETE FROM Customers WHERE CustomerId = @Id;";

        db.Execute(sql, new { Id = id });
    }

    public Customer? GetById(int id)
    {
        using var db = _factory.Create();

        const string sql = @"
        SELECT
            CustomerId AS Id,
            Name,
            Email,
            Phone
        FROM Customers
        WHERE CustomerId = @Id;
        ";

        return db.QueryFirstOrDefault<Customer>(sql, new { Id = id });
    }


    public void Update(Customer customer)
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"
        UPDATE Customers
        SET Name = @Name,
            Email = @Email,
            Phone = @Phone
        WHERE CustomerId = @Id;
        ";

        db.Execute(sql, new
        {
            customer.Name,
            customer.Email,
            customer.Phone,
            customer.Id
        });
    }
}
