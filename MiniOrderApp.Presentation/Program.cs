using MiniOrderApp.Infrastructure.Database;
using MiniOrderApp.Infrastructure.Repositories;
using MiniOrderApp.Domain.Interfaces;
using MiniOrderApp.Domain;
internal class Program
{
    private static void Main(string[] args)
    {
        var connectionstring = "Data Source=miniorder.db";

        DbInitializer.Initialize(connectionstring);


        var factory = new SQLiteConnectionFactory(connectionstring);
        IOrderRepository orderRepo = new OrderRepository(factory);

        var orders = orderRepo.GetOrders();

        foreach (var o in orders)
        {
            Console.WriteLine($"Order {o.Id} - Customer: {o.CustomerId} - Total: {o.TotalAmount}");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}