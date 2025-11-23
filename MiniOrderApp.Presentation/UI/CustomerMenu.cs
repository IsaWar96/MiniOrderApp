using MiniOrderApp.Domain;
using MiniOrderApp.Domain.Interfaces;

namespace MiniOrderApp.Presentation.UI;

public class CustomerMenu
{
    private readonly ICustomerRepository _customers;

    public CustomerMenu(ICustomerRepository customers)
    {
        _customers = customers;
    }

    public void Show()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Customers \n ------");
            Console.WriteLine("1. Add");
            Console.WriteLine("2. Update");
            Console.WriteLine("3. Delete");
            Console.WriteLine("4. List");
            Console.WriteLine("0. Back");
            Console.Write("Choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Add();
                    break;
                case "2":
                    Update();
                    break;
                case "3":
                    Delete();
                    break;
                case "4":
                    List();
                    break;
                case "0":
                    return;
                default:
                    break;
            }
        }
    }

    private void Add()
    {
        Console.Write("Name: ");
        var name = Console.ReadLine() ?? "";
        Console.Write("Email: ");
        var email = Console.ReadLine() ?? "";
        Console.Write("Phone: ");
        var phone = Console.ReadLine() ?? "";

        var c = new Customer(name, email, phone);
        _customers.Add(c);

        Console.WriteLine("Customer added.");
        Console.ReadKey();
    }

    private void Update()
    {
        var list = _customers.GetCustomers().ToList();
        foreach (var c in list)
            Console.WriteLine($"{c.Id}. {c.Name}");

        Console.Write("Customer-ID: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
            return;

        var customer = _customers.GetById(id);
        if (customer == null)
        {
            Console.WriteLine("Customer was not found.");
            Console.ReadKey();
            return;
        }

        Console.Write("New name: ");
        var name = Console.ReadLine() ?? "";
        Console.Write("New email: ");
        var email = Console.ReadLine() ?? "";
        Console.Write("New phone: ");
        var phone = Console.ReadLine() ?? "";

        var updated = new Customer(name, email, phone) { Id = id };
        _customers.Update(updated);

        Console.WriteLine("Updated Customer.");
        Console.ReadKey();
    }

    private void Delete()
    {
        var list = _customers.GetCustomers().ToList();
        foreach (var c in list)
            Console.WriteLine($"{c.Id}. {c.Name}");

        Console.Write("Customer-ID you want to delete: ");
        if (int.TryParse(Console.ReadLine(), out var id))
        {
            _customers.Delete(id);
            Console.WriteLine("Deleted.");
        }

        Console.ReadKey();
    }

    private void List()
    {
        var list = _customers.GetCustomers();

        Console.WriteLine("Customer List \n--------");
        foreach (var c in list)
            Console.WriteLine($"{c.Id}. {c.Name} - {c.Email} - ({c.Phone})");

        Console.ReadKey();
    }
}
