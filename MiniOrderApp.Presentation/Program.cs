using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;
using MiniOrderApp.Infrastructure.Repositories;
using MiniOrderApp.Infrastructure.Services;
internal class Program
{
    private static void Main(string[] args)
    {

        var connectionString = "Data Source=miniorder.db";

        DbInitializer.Initialize(connectionString);

        var factory = new SQLiteConnectionFactory(connectionString);
        ICustomerRepository customerRepo = new CustomerRepository(factory);

        var customers = customerRepo.GetCustomers().ToList();
        foreach (var c in customers)
        {
            Console.WriteLine($"{c.Id}. {c.Name} ({c.Email})");
        }

        Console.Write("Enter the ID of the customer you want to update: ");
        var idText = Console.ReadLine();
        if (!int.TryParse(idText, out var customerId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var customerToUpdate = customerRepo.GetById(customerId);
        if (customerToUpdate is null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }

        Console.Write("New name: ");
        var newName = Console.ReadLine() ?? "";

        Console.Write("New email: ");
        var newEmail = Console.ReadLine() ?? "";

        Console.Write("New phonenumber: ");
        var newPhone = Console.ReadLine() ?? "";

        var updatedCustomer = new Customer(newName, newEmail, newPhone);
        updatedCustomer.Id = customerToUpdate.Id;

        customerRepo.Update(updatedCustomer);

        Console.WriteLine("Customer updated.");
    }
}