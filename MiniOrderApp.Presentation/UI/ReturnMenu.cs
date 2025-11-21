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
        Console.WriteLine("=== Return Order ===");

        var orders = _orders.GetOrders();
        foreach (var o in orders)
            Console.WriteLine($"{o.Id}. {o.Status} - Total {o.TotalAmount}");

        Console.Write("Order ID to return: ");
        var id = int.Parse(Console.ReadLine()!);

        Console.Write("Reason: ");
        var reason = Console.ReadLine() ?? "";
        Console.Write("Refunded amount: ");
        var refund = decimal.Parse(Console.ReadLine()!);

        var ret = new Return(id, DateTime.Today, reason, refund);

        _returns.AddReturn(ret);
        _orders.MarkAsReturned(id);

        Console.WriteLine("Returned.");
        Console.ReadKey();
    }
}
