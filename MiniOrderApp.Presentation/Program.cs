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
        IOrderRepository orderRepo = new OrderRepository(factory);

        var orders = orderRepo.GetOrders().ToList();
        foreach (var o in orders)
        {
            Console.WriteLine($"{o.Id}. Customer {o.CustomerId}, Status: {o.Status}, Total: {o.TotalAmount}");
        }

        Console.Write("Enter ID on order you want to update: ");
        var orderIdText = Console.ReadLine();
        if (!int.TryParse(orderIdText, out var orderId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var orderToUpdate = orderRepo.GetById(orderId);
        if (orderToUpdate is null)
        {
            Console.WriteLine("Order not found.");
            return;
        }

        Console.WriteLine("Choose new status:");
        Console.WriteLine("1. Created");
        Console.WriteLine("2. Paid");
        Console.WriteLine("3. Returned");
        var statusChoice = Console.ReadLine();

        OrderStatus newStatus = orderToUpdate.Status;

        switch (statusChoice)
        {
            case "1":
                newStatus = OrderStatus.Created;
                break;
            case "2":
                newStatus = OrderStatus.Paid;
                break;
            case "3":
                newStatus = OrderStatus.Returned;
                break;
            default:
                Console.WriteLine("Invalid choice.");
                return;
        }

        orderToUpdate.SetStatus(newStatus);

        orderRepo.Update(orderToUpdate);

        Console.WriteLine("Order updated.");
    }
}