using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;

namespace MiniOrderApp.Presentation.UI;

public class ReturnMenu
{
    private readonly IReturnRepository _returns;
    private readonly IOrderRepository _orders;

    public ReturnMenu(IReturnRepository returns, IOrderRepository orders)
    {
        _returns = returns;
        _orders = orders;
    }

    public void Show()
    {
        Console.Clear();
        Console.WriteLine("Return Order\n------------");

        var orders = _orders.GetOrders().ToList();
        if (orders.Count == 0)
        {
            Console.WriteLine("No orders found. Please create an order first.");
            Console.ReadKey();
            return;
        }

        foreach (var o in orders)
            Console.WriteLine($"Order ID: {o.Id} - Status: {o.Status} - Total: {o.TotalAmount:C}");

        Console.Write("\nOrder ID to return: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Error: Invalid ID. Please enter a number.");
            Console.ReadKey();
            return;
        }

        var order = _orders.GetById(id);
        if (order == null)
        {
            Console.WriteLine($"Error: Order with ID {id} was not found.");
            Console.ReadKey();
            return;
        }

        if (order.Status == OrderStatus.Returned)
        {
            Console.WriteLine($"Error: Order {id} is already marked as returned.");
            Console.ReadKey();
            return;
        }

        Console.Write("Reason for return: ");
        var reason = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Refunded amount: ");
        if (!decimal.TryParse(Console.ReadLine(), out var refund) || refund < 0)
        {
            Console.WriteLine("Error: Refunded amount must be a valid number (0 or greater).");
            Console.ReadKey();
            return;
        }

        try
        {
            var returnInfo = new Return(id, DateTime.Today, reason, refund);
            _returns.AddReturn(returnInfo);
            _orders.MarkAsReturned(id);

            Console.WriteLine($"\nOrder {id} marked as returned. Refunded amount: {refund:C}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        Console.ReadKey();
    }
}
