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

        // Get all orders
        var orders = orderRepo.GetOrders().ToList();

        if (orders.Count == 0)
        {
            Console.WriteLine("There are no orders to return.");
            return;
        }

        Console.WriteLine("Orders:");
        foreach (var o in orders)
        {
            Console.WriteLine($"Order {o.Id} - Customer {o.CustomerId} - Status {o.Status} - Total {o.TotalAmount}");
        }

        Console.Write("\nEnter the Order ID you want to return: ");
        var orderIdStr = Console.ReadLine();
        if (!int.TryParse(orderIdStr, out var orderId))
        {
            Console.WriteLine("Invalid order ID.");
            return;
        }

        Order? orderToReturn = null;

        foreach (var o in orders)
        {
            if (o.Id == orderId)
            {
                orderToReturn = o;
                break;
            }
        }

        if (orderToReturn == null)
        {
            Console.WriteLine("Order not found.");
            return;
        }

        // 3. Reason + refund
        Console.Write("Return reason: ");
        var reason = Console.ReadLine() ?? "";

        Console.Write("Refunded amount: ");
        var refundStr = Console.ReadLine();
        if (!decimal.TryParse(refundStr, out var refundedAmount))
        {
            Console.WriteLine("Invalid amount.");
            return;
        }

        var returnInfo = new Return(
            orderId: orderId,
            returnDate: DateTime.Today,
            reason: reason,
            refundedAmount: refundedAmount
        );

        returnRepo.AddReturn(returnInfo);
        orderRepo.MarkAsReturned(orderId);

        Console.WriteLine("\nOrder returned successfully!");

        // 5. Visa uppdaterade orders
        var updatedOrders = orderRepo.GetOrders();
        Console.WriteLine("\nUpdated orders:");
        foreach (var o in updatedOrders)
        {
            Console.WriteLine($"Order {o.Id} - Customer {o.CustomerId} - Status {o.Status} - Total {o.TotalAmount}");
        }

        Console.WriteLine("\nDone. Press any key to exit.");
        Console.ReadKey();
    }
}