using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;

namespace MiniOrderApp.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ApplicationDbContext _context;

    public CustomerService(ICustomerRepository customerRepository, ApplicationDbContext context)
    {
        _customerRepository = customerRepository;
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetCustomersAsync();
        if (customers == null)
        {
            throw new ArgumentException("Customers cannot be null", nameof(customers));
        }
        return customers;
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
        var customer = new Customer(name, email, phone);
        if (customer == null)
        {
            throw new ArgumentException("Customer cannot be null", nameof(customer));
        }

        await _customerRepository.AddAsync(customer);
        await _context.SaveChangesAsync();
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

        existingCustomer.UpdateDetails(name, email, phone);

        await _customerRepository.UpdateAsync(existingCustomer);
        await _context.SaveChangesAsync();

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
        await _context.SaveChangesAsync();
    }
}
