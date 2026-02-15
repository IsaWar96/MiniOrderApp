namespace MiniOrderApp.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetCustomersAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
}

