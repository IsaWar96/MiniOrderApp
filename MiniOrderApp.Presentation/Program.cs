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
        // ICustomerRepository customerRepo = new CustomerRepository(factory);
        IOrderRepository orderRepo = new OrderRepository(factory);

        var orders = orderRepo.GetOrders().ToList();
        foreach (var o in orders)
        {
            Console.WriteLine($"{o.Id}. Customer {o.CustomerId}, Status: {o.Status}, Total: {o.TotalAmount}");
        }

        Console.Write("Enter ID on order you want to delete: ");
        var orderIdText = Console.ReadLine();
        if (!int.TryParse(orderIdText, out var orderId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        orderRepo.Delete(orderId);
        Console.WriteLine("Order deleted.");
    }
}