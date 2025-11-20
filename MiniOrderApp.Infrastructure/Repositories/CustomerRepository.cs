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
    public void Add(Customer customer)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Customer? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Customer> GetCustomers()
    {
        throw new NotImplementedException();
    }

    public void Update(Customer customer)
    {
        throw new NotImplementedException();
    }
}
