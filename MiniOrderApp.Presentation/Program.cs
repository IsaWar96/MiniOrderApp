using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Infrastructure.Database;
using Microsoft.Data.Sqlite;
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

        // 1. Fråga användaren om totalbelopp
        Console.WriteLine("Create new order for CustomerId = 1");
        Console.Write("Input total amount: ");

        var totalText = Console.ReadLine();

        if (!decimal.TryParse(totalText, out var totalAmount))
        {
            Console.WriteLine("Ogiltigt belopp. Avslutar.");
            return;
        }

        // 2. Skapa Order via konstruktorn (sätter OrderDate och Status internt)
        var newOrder = new Order(
            customerId: 1,
            orderDate: DateTime.Today,
            totalAmount: totalAmount
        );

        // 3. Spara ordern
        orderRepo.Add(newOrder);
        Console.WriteLine("Order sparad!\n");

        // 4. Hämta och skriv ut alla orders
        var orders = orderRepo.GetOrders();

        Console.WriteLine("All orders in the database");
        foreach (var o in orders)
        {
            Console.WriteLine($"Order {o.Id} - Customer: {o.CustomerId} - Date: {o.OrderDate:yyyy-MM-dd} - Total: {o.TotalAmount}");
        }

        Console.WriteLine("Done. Press Any key to continue...");
        Console.ReadKey();
    }
}