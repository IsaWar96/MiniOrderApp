namespace MiniOrderApp.Domain.Interfaces;

public interface ICustomerRepository
{
    IEnumerable<Customer> GetCustomers();
    Customer? GetById(int id);
    void Add(Customer customer);
    void Update(Customer customer);
    void Delete(int id);
}

