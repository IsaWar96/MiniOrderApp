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
        Console.Clear();
        Console.WriteLine("Create Order\n------------");

        var customers = _customers.GetCustomers().ToList();
        if (customers.Count == 0)
        {
            Console.WriteLine("Error: No customers found. Please add a customer first.");
            Console.ReadKey();
            return;
        }

        foreach (var c in customers)
            Console.WriteLine($"{c.Id}. {c.Name}");

        Console.Write("\nCustomer ID: ");
        if (!int.TryParse(Console.ReadLine(), out var custId))
        {
            Console.WriteLine("Error: Invalid ID. Please enter a number.");
            Console.ReadKey();
            return;
        }

        var customer = _customers.GetById(custId);
        if (customer == null)
        {
            Console.WriteLine($"Error: Customer with ID {custId} was not found.");
            Console.ReadKey();
            return;
        }

        Console.Write("How many products?: ");
        if (!int.TryParse(Console.ReadLine(), out var count) || count <= 0)
        {
            Console.WriteLine("Error: Please enter a valid number greater than 0.");
            Console.ReadKey();
            return;
        }

        var order = new Order(custId, DateTime.Today, 0);

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"\nProduct {i + 1}:");
            Console.Write("  Name: ");
            var name = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Error: Product name cannot be empty.");
                Console.ReadKey();
                return;
            }

            Console.Write("  Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out var qty) || qty <= 0)
            {
                Console.WriteLine("Error: Quantity must be a number greater than 0.");
                Console.ReadKey();
                return;
            }

            Console.Write("  Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out var price) || price < 0)
            {
                Console.WriteLine("Error: Price must be a valid number (0 or greater).");
                Console.ReadKey();
                return;
            }

            try
            {
                order.AddItem(new OrderItem(name, qty, price));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
                return;
            }
        }

        try
        {
            order.RecalculateTotal();
            _orders.Add(order);
            Console.WriteLine("\nOrder created successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        Console.ReadKey();
    }

    private void UpdateStatus()
    {
        Console.Clear();
        Console.WriteLine("Update Order Status\n-------------------");

        var list = _orders.GetOrders().ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("No orders found. Please create an order first.");
            Console.ReadKey();
            return;
        }

        foreach (var o in list)
            Console.WriteLine($"Order ID: {o.Id}. Status: {o.Status}");

        Console.Write("\nChoose Order ID: ");
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

        try
        {
            order.SetStatus(status);
            _orders.Update(order);
            Console.WriteLine($"\nOrder status updated to {status}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        Console.ReadKey();
    }

    private void Delete()
    {
        Console.Clear();
        Console.WriteLine("Delete Order\n------------");

        var list = _orders.GetOrders().ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("No orders found. Nothing to delete.");
            Console.ReadKey();
            return;
        }

        foreach (var o in list)
            Console.WriteLine($"Order ID: {o.Id}. Total: {o.TotalAmount:C}");

        Console.Write("\nOrder ID to delete: ");
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

        Console.WriteLine($"\nAre you sure you want to delete Order {id} (Total: {order.TotalAmount:C})? (y/n): ");
        var confirm = Console.ReadLine()?.Trim().ToLower();

        if (confirm == "y" || confirm == "yes")
        {
            try
            {
                _orders.Delete(id);
                Console.WriteLine("Order deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Delete cancelled.");
        }

        Console.ReadKey();
    }

    private void List()
    {
        Console.Clear();
        Console.WriteLine("Order List\n----------");

        var list = _orders.GetOrders().ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("No orders found. Please create an order first.");
        }
        else
        {
            foreach (var o in list)
            {
                var customer = _customers.GetById(o.CustomerId);
                var customerName = customer?.Name ?? $"ID {o.CustomerId}";
                Console.WriteLine($"Order {o.Id} - Customer: {customerName} - Status: {o.Status} - Total: {o.TotalAmount:C}");
            }
        }

        Console.ReadKey();
    }
}
