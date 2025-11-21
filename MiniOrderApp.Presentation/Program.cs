using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;
using MiniOrderApp.Infrastructure.Repositories;
internal class Program
{
    private static void Main(string[] args)
    {

        var connectionString = "Data Source=miniorder.db";

        DbInitializer.Initialize(connectionString);

        var factory = new SQLiteConnectionFactory(connectionString);
        ICustomerRepository customerRepo = new CustomerRepository(factory);
        IReturnRepository returnRepo = new ReturnRepository(factory);
        IOrderRepository orderRepo = new OrderRepository(factory);

        var customers = customerRepo.GetCustomers().ToList();

        Console.Write("Enter Order ID to view details: ");
        var idText = Console.ReadLine();

        if (!int.TryParse(idText, out var orderId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var order = orderRepo.GetById(orderId);
        if (order == null)
        {
            Console.WriteLine("Order not found.");
            return;
        }

        var customer = customerRepo.GetById(order.CustomerId);
        var items = orderRepo.GetItemsForOrder(orderId);
        var returnInfo = returnRepo.GetByOrderId(orderId);

        Console.WriteLine($"\nOrder {order.Id}");
        Console.WriteLine($"Date: {order.OrderDate:yyyy-MM-dd}");
        Console.WriteLine($"Status: {order.Status}");
        Console.WriteLine($"Total: {order.TotalAmount}");

        Console.WriteLine("\nCustomer:");
        Console.WriteLine($"{customer!.Name} ({customer.Email})");

        Console.WriteLine("\nItems:");
        foreach (var item in items)
        {
            Console.WriteLine($" - {item.ProductName} ({item.Quantity} x {item.UnitPrice}) = {item.LineTotal}");
        }

        if (returnInfo != null)
        {
            Console.WriteLine("\nReturn info:");
            Console.WriteLine($"Reason: {returnInfo.Reason}");
            Console.WriteLine($"Refunded: {returnInfo.RefundedAmount}");
        }

        Console.WriteLine("\nDone. Press any key...");
        Console.ReadKey();

    }
}