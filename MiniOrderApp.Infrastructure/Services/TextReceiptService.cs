using MiniOrderApp.Domain;

namespace MiniOrderApp.Infrastructure.Services;

public class TextReceiptService
{
    public string SaveReceipt(Order order, Customer customer, IEnumerable<OrderItem> items, Return? returnInfo)
    {
        var baseDir = AppContext.BaseDirectory;
        var receiptsDir = Path.Combine(baseDir, "Receipts");
        Directory.CreateDirectory(receiptsDir);

        var fileName = $"Receipt_Order_{order.Id}.txt";
        var filePath = Path.Combine(receiptsDir, fileName);

        using var writer = new StreamWriter(filePath, false);

        writer.WriteLine($"ORDER RECEIPT #{order.Id}");
        writer.WriteLine($"Date: {order.OrderDate:yyyy-MM-dd}");
        writer.WriteLine($"Status: {order.Status}");
        writer.WriteLine();

        writer.WriteLine("Customer:");
        writer.WriteLine($"  {customer.Name}");
        writer.WriteLine($"  {customer.Email}");
        writer.WriteLine($"  {customer.Phone}");
        writer.WriteLine();

        writer.WriteLine("Items:");
        foreach (var item in items)
        {
            writer.WriteLine(
                $"  - {item.ProductName} ({item.Quantity} x {item.UnitPrice}) = {item.LineTotal}");
        }

        writer.WriteLine();
        writer.WriteLine($"Total: {order.TotalAmount}");

        if (returnInfo != null)
        {
            writer.WriteLine();
            writer.WriteLine("Return info:");
            writer.WriteLine($"  Reason: {returnInfo.Reason}");
            writer.WriteLine($"  Refunded: {returnInfo.RefundedAmount}");
            writer.WriteLine($"  Date: {returnInfo.ReturnDate:yyyy-MM-dd}");
        }

        return filePath;
    }
}
