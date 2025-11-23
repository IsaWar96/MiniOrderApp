using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;

namespace MiniOrderApp.Presentation.UI;

public class OrderMenu
{
    private readonly IOrderRepository _orders;
    private readonly ICustomerRepository _customers;

    public OrderMenu(IOrderRepository orders, ICustomerRepository customers)
    {
        _orders = orders;
        _customers = customers;
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Orders \n -------");
            Console.WriteLine("1. Create");
            Console.WriteLine("2. Update Status");
            Console.WriteLine("3. Delete");
            Console.WriteLine("4. List");
            Console.WriteLine("0. Back");
            Console.Write("Choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Create();
                    break;
                case "2":
                    UpdateStatus();
                    break;
                case "3":
                    Delete();
                    break;
                case "4":
                    List();
                    break;
                case "0":
                    return;
            }
        }
    }

    private void Create()
    {

        var customers = _customers.GetCustomers().ToList();
        foreach (var c in customers)
            Console.WriteLine($"{c.Id}. {c.Name}");

        Console.Write("Customer ID: ");
        var custId = int.Parse(Console.ReadLine() ?? "1");

        var order = new Order(custId, DateTime.Today, 0);

        Console.Write("How many products?: ");
        var count = int.Parse(Console.ReadLine() ?? "1");

        for (int i = 0; i < count; i++)
        {
            Console.Write("Name: ");
            var name = Console.ReadLine()!;
            Console.Write("Quantity: ");
            var qty = int.Parse(Console.ReadLine()!);
            Console.Write("Price: ");
            var price = decimal.Parse(Console.ReadLine()!);

            order.AddItem(new OrderItem(name, qty, price));
        }

        _orders.Add(order);

        Console.WriteLine("Order added.");
        Console.ReadKey();
    }

    private void UpdateStatus()
    {
        var list = _orders.GetOrders().ToList();
        foreach (var o in list)
            Console.WriteLine($"Order-ID: {o.Id}. Status: {o.Status}");

        Console.Write("Choose Order ID: ");
        var id = int.Parse(Console.ReadLine()!);

        Console.WriteLine("1. Created");
        Console.WriteLine("2. Paid");
        Console.WriteLine("3. Returned");

        var choice = Console.ReadLine();

        OrderStatus status;

        switch (choice)
        {
            case "1":
                status = OrderStatus.Created;
                break;

            case "2":
                status = OrderStatus.Paid;
                break;

            case "3":
                status = OrderStatus.Returned;
                break;

            default:
                status = OrderStatus.Created;
                break;
        }

        var order = _orders.GetById(id);
        order!.SetStatus(status);

        _orders.Update(order);

        Console.WriteLine($"Updated Status to {status}.");
        Console.ReadKey();
    }

    private void Delete()
    {
        var list = _orders.GetOrders();
        foreach (var o in list)
            Console.WriteLine($"Order-ID: {o.Id}. Total: {o.TotalAmount}");

        Console.Write("Choose ID to delete: ");
        var id = int.Parse(Console.ReadLine()!);

        _orders.Delete(id);

        Console.WriteLine($"Deleted: ({id}).");
        Console.ReadKey();
    }

    private void List()
    {
        var list = _orders.GetOrders();
        foreach (var o in list)
            Console.WriteLine($"Order {o.Id} - Customer {o.CustomerId} - Total {o.TotalAmount}");

        Console.ReadKey();
    }
}
