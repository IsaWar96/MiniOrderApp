using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;
using MiniOrderApp.Infrastructure.Repositories;
internal class Program
{
    private static void Main(string[] args)
    {

        var connectionString = "Data Source=miniorder.db";

        // Initiera databasen
        DbInitializer.Initialize(connectionString);

        var factory = new SQLiteConnectionFactory(connectionString);
        IOrderRepository orderRepo = new OrderRepository(factory);

        Console.WriteLine("Creating new order for customer 1...");

        Console.Write("How many products does the order have? ");
        var countText = Console.ReadLine();
        if (!int.TryParse(countText, out int count) || count < 1)
        {
            Console.WriteLine("Error: Invalid amount.");
            return;
        }

        var order = new Order(
            customerId: 1,
            orderDate: DateTime.Today,
            totalAmount: 0
        );

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"\nProdukt {i + 1}:");

            Console.Write("Name: ");
            var name = Console.ReadLine() ?? "";

            Console.Write("Amount: ");
            var qtyText = Console.ReadLine() ?? "";
            int qty = int.Parse(qtyText);

            Console.Write("Price per unit: ");
            var priceText = Console.ReadLine() ?? "";
            decimal price = decimal.Parse(priceText);

            var item = new OrderItem(name, qty, price);
            order.AddItem(item);  // totalen uppdateras
        }

        // 3. Spara order + items
        orderRepo.Add(order);

        Console.WriteLine("\nOrder saved!");

        // 4. Skriv ut alla orders
        var orders = orderRepo.GetOrders();
        foreach (var o in orders)
        {
            Console.WriteLine($"Order {o.Id} - Customer {o.CustomerId} - Total {o.TotalAmount}");
        }

        Console.WriteLine("Done. Press Any key to continue...");
        Console.ReadKey();
    }
}