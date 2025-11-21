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
        IOrderRepository orderRepo = new OrderRepository(factory);

        var customers = customerRepo.GetCustomers().ToList();

        if (customers.Count == 0)
        {
            Console.WriteLine("There is no customers yet. Create a customer first");
        }

        Console.WriteLine("Choose customer for the order: ");
        foreach (var c in customers)
        {
            Console.WriteLine($"{c.Id}. {c.Name}, {c.Email}");
        }

        Console.Write("Customer-ID: ");
        var custIdStr = Console.ReadLine();
        if (!int.TryParse(custIdStr, out var custId))
        {
            Console.WriteLine("Invalid customer-ID");
            return;
        }

        Console.Write("How many products does the order have?: ");
        var countText = Console.ReadLine();
        if (!int.TryParse(countText, out var count) || count < 1)
        {
            Console.WriteLine("Invalid Amount");
            return;
        }
        var cOrder = new Order(
            customerId: custId,
            orderDate: DateTime.Today,
            totalAmount: 0
        );

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"\nProdukt {i + 1}:");
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? "";

            Console.Write("Quantity: ");
            var qnty = Console.ReadLine() ?? "";
            int quantity = int.Parse(qnty);

            Console.Write("Price per unit: ");
            var prperU = Console.ReadLine() ?? "";
            decimal price = decimal.Parse(prperU);

            var item = new OrderItem(name, quantity, price);
            cOrder.AddItem(item);
        }
        orderRepo.Add(cOrder);

        // Get all orders
        var orders = orderRepo.GetOrders();

        foreach (var order in orders)
        {
            Console.WriteLine($"Order {order.Id} - Customer {order.CustomerId} - Total {order.TotalAmount}");
        }

        Console.WriteLine("Done. Press Any key to continue.");
        Console.ReadKey();
    }
}