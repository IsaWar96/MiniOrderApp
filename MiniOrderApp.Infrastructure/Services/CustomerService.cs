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

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetCustomersAsync();
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Customer ID must be greater than zero.", nameof(id));
        }

        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found.");
        }

        return customer;
    }

    public async Task<Customer> CreateCustomerAsync(string name, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Customer name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Customer email is required.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("Customer phone is required.", nameof(phone));
        }

        var customer = new Customer(name, email, phone);
        await _customerRepository.AddAsync(customer);
        return customer;
    }

    public async Task<Customer> UpdateCustomerAsync(int id, string name, string email, string phone)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid id", nameof(id));
        }

        var existingCustomer = await _customerRepository.GetByIdAsync(id);

        if (existingCustomer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Customer name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Customer email is required.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("Customer phone is required.", nameof(phone));
        }

        existingCustomer.UpdateDetails(name, email, phone);

        await _customerRepository.UpdateAsync(existingCustomer);

        return existingCustomer;
    }

    public async Task DeleteCustomerAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Customer ID must be greater than zero.", nameof(id));
        }

        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {id} not found.");
        }

        await _customerRepository.DeleteAsync(id);
    }
}
