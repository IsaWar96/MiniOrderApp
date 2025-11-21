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
        IOrderRepository orderRepo = new OrderRepository(factory);

        // Get all orders
        var orders = orderRepo.GetOrders();

        foreach (var order in orders)
        {
            Console.WriteLine($"Order {order.Id} - Customer {order.CustomerId} - Total {order.TotalAmount}");

            // Get items for this order
            var items = orderRepo.GetItemsForOrder(order.Id);

            foreach (var item in items)
            {
                Console.WriteLine($"   - {item.ProductName} ({item.Quantity} x {item.UnitPrice}) = {item.LineTotal}");
            }

            Console.WriteLine();
        }

        Console.WriteLine("Done. Press Any key to continue.");
        Console.ReadKey();
    }
}