using MiniOrderApp.Domain;

namespace MiniOrderApp.Infrastructure.Interfaces;

public interface ICustomerService
{
    IEnumerable<Customer> GetAllCustomers();
    Customer GetCustomerById(int id);
    Customer CreateCustomer(Customer customer);
    Customer UpdateCustomer(int id, Customer customer);
    void DeleteCustomer(int id);
}
