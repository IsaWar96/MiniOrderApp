namespace MiniOrderApp.Domain.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task<Customer> GetCustomerByIdAsync(int id);
    Task<Customer> CreateCustomerAsync(string name, string email, string phone);
    Task<Customer> UpdateCustomerAsync(int id, string name, string email, string phone);
    Task DeleteCustomerAsync(int id);
}
