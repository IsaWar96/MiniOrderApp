using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;

namespace MiniOrderApp.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public IEnumerable<Customer> GetAllCustomers()
    {
        return _customerRepository.GetCustomers();
    }

    public Customer GetCustomerById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Customer ID must be greater than zero.", nameof(id));
        }

        var customer = _customerRepository.GetById(id);
        
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found.");
        }

        return customer;
    }

    public Customer CreateCustomer(Customer customer)
    {
        if (customer == null)
        {
            throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(customer.Name))
        {
            throw new ArgumentException("Customer name is required.", nameof(customer.Name));
        }

        if (string.IsNullOrWhiteSpace(customer.Email))
        {
            throw new ArgumentException("Customer email is required.", nameof(customer.Email));
        }

        if (string.IsNullOrWhiteSpace(customer.Phone))
        {
            throw new ArgumentException("Customer phone is required.", nameof(customer.Phone));
        }

        _customerRepository.Add(customer);
        return customer;
    }

    public Customer UpdateCustomer(int id, Customer customer)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Customer ID must be greater than zero.", nameof(id));
        }

        if (customer == null)
        {
            throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");
        }

        var existingCustomer = _customerRepository.GetById(id);
        
        if (existingCustomer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found.");
        }

        if (string.IsNullOrWhiteSpace(customer.Name))
        {
            throw new ArgumentException("Customer name is required.", nameof(customer.Name));
        }

        if (string.IsNullOrWhiteSpace(customer.Email))
        {
            throw new ArgumentException("Customer email is required.", nameof(customer.Email));
        }

        if (string.IsNullOrWhiteSpace(customer.Phone))
        {
            throw new ArgumentException("Customer phone is required.", nameof(customer.Phone));
        }

        customer.Id = id;
        _customerRepository.Update(customer);
        return customer;
    }

    public void DeleteCustomer(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Customer ID must be greater than zero.", nameof(id));
        }

        var customer = _customerRepository.GetById(id);
        
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found.");
        }

        _customerRepository.Delete(id);
    }
}
