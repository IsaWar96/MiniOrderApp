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
            FROM Customers;";

        return db.Query<Customer>(sql);
    }

    public Customer? GetById(int id)
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"
            SELECT
                CustomerId AS Id,
                Name,
                Email,
                Phone
            FROM Customers
            WHERE CustomerId = @Id;";

        return db.QueryFirstOrDefault<Customer>(sql, new { Id = id });
    }

    public void Add(Customer customer)
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"
            INSERT INTO Customers (Name, Email, Phone)
            VALUES (@Name, @Email, @Phone);";

        db.Execute(sql, new
        {
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone
        });
    }

    public void Update(Customer customer)
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"
            UPDATE Customers
            SET Name = @Name,
                Email = @Email,
                Phone = @Phone
            WHERE CustomerId = @Id;";

        db.Execute(sql, new
        {
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Id = customer.Id
        });
    }

    public void Delete(int id)
    {
        using IDbConnection db = _factory.Create();

        const string sql = @"DELETE FROM Customers WHERE CustomerId = @Id;";

        db.Execute(sql, new { Id = id });
    }
}
