using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;

namespace MiniOrderApp.Tests.Fakes;

public class FakeCustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers = new();
    private int _nextId = 1;

    public IEnumerable<Customer> GetCustomers()
    {
        return _customers.ToList();
    }

    public Customer? GetById(int id)
    {
        return _customers.FirstOrDefault(c => c.Id == id);
    }

    public void Add(Customer customer)
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        
        customer.Id = _nextId++;
        _customers.Add(customer);
    }

    public void Update(Customer customer)
    {
        if (customer == null) throw new ArgumentNullException(nameof(customer));
        
        var existing = _customers.FirstOrDefault(c => c.Id == customer.Id);
        if (existing == null)
            throw new InvalidOperationException($"Customer with ID {customer.Id} not found");
        
        _customers.Remove(existing);
        _customers.Add(customer);
    }

    public void Delete(int id)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id);
        if (customer != null)
        {
            _customers.Remove(customer);
        }
    }
}
