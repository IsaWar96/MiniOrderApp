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

        Console.Write("Enter ID of customer you want to delete: ");
        var idText = Console.ReadLine();
        if (!int.TryParse(idText, out var customerId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        customerRepo.Delete(customerId);
        Console.WriteLine("Customer deleted.");
    }
}