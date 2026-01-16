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
            string[] options = { "Create", "Update Status", "Delete", "List", "Back" };
            int choice = MenuHelper.ShowArrowMenu("Orders\n-------", options);

            if (choice == -1 || choice == 4)
                return;

            switch (choice)
            {
                case 0:
                    Create();
                    break;
                case 1:
                    UpdateStatus();
                    break;
                case 2:
                    Delete();
                    break;
                case 3:
                    List();
                    break;
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
        if (!int.TryParse(Console.ReadLine(), out var id))
            return;

        string[] statusOptions = { "Created", "Paid", "Returned" };
        int choice = MenuHelper.ShowArrowMenu("Select Status\n------------", statusOptions);

        if (choice == -1)
            return;

        OrderStatus status = choice switch
        {
            0 => OrderStatus.Created,
            1 => OrderStatus.Paid,
            2 => OrderStatus.Returned,
            _ => OrderStatus.Created
        };

        var order = _orders.GetById(id);
        if (order == null)
            return;

        order.SetStatus(status);
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
