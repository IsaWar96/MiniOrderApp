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
            string[] options = { "Add", "Update", "Delete", "List", "Back" };
            int choice = MenuHelper.ShowArrowMenu("Customers\n------", options);

            if (choice == -1 || choice == 4)
                return;

            switch (choice)
            {
                case 0:
                    Add();
                    break;
                case 1:
                    Update();
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

    private void Add()
    {
        Console.Clear();
        Console.WriteLine("Add Customer\n------------");

        Console.Write("Name: ");
        var name = Console.ReadLine()?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Error: Name is required and cannot be empty.");
            Console.ReadKey();
            return;
        }

        Console.Write("Email: ");
        var email = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Phone: ");
        var phone = Console.ReadLine()?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(phone))
        {
            Console.WriteLine("Error: Phone is required and cannot be empty.");
            Console.ReadKey();
            return;
        }

        try
        {
            var customer = new Customer(name, email, phone);
            _customers.Add(customer);
            Console.WriteLine("\nCustomer added successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
        }

        Console.ReadKey();
    }

    private void Update()
    {
        Console.Clear();
        Console.WriteLine("Update Customer\n---------------");

        var list = _customers.GetCustomers().ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("No customers found. Please add a customer first.");
            Console.ReadKey();
            return;
        }

        foreach (var c in list)
            Console.WriteLine($"{c.Id}. {c.Name}");

        Console.Write("\nCustomer ID: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Error: Invalid ID. Please enter a number.");
            Console.ReadKey();
            return;
        }

        var customer = _customers.GetById(id);
        if (customer == null)
        {
            Console.WriteLine($"Error: Customer with ID {id} was not found.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nCurrent: {customer.Name} - {customer.Email} - {customer.Phone}");
        Console.Write("New name: ");
        var name = Console.ReadLine()?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Error: Name is required and cannot be empty.");
            Console.ReadKey();
            return;
        }

        Console.Write("New email: ");
        var email = Console.ReadLine()?.Trim() ?? "";

        Console.Write("New phone: ");
        var phone = Console.ReadLine()?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(phone))
        {
            Console.WriteLine("Error: Phone is required and cannot be empty.");
            Console.ReadKey();
            return;
        }

        try
        {
            var updated = new Customer(name, email, phone) { Id = id };
            _customers.Update(updated);
            Console.WriteLine("\nCustomer updated successfully!");
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
        Console.WriteLine("Delete Customer\n---------------");

        var list = _customers.GetCustomers().ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("No customers found. Nothing to delete.");
            Console.ReadKey();
            return;
        }

        foreach (var c in list)
            Console.WriteLine($"{c.Id}. {c.Name}");

        Console.Write("\nCustomer ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Error: Invalid ID. Please enter a number.");
            Console.ReadKey();
            return;
        }

        var customer = _customers.GetById(id);
        if (customer == null)
        {
            Console.WriteLine($"Error: Customer with ID {id} was not found.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nAre you sure you want to delete {customer.Name}? (y/n): ");
        var confirm = Console.ReadLine()?.Trim().ToLower();

        if (confirm == "y" || confirm == "yes")
        {
            _customers.Delete(id);
            Console.WriteLine("Customer deleted successfully.");
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
        Console.WriteLine("Customer List\n-------------");

        var list = _customers.GetCustomers().ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("No customers found. Please add a customer first.");
        }
        else
        {
            foreach (var c in list)
                Console.WriteLine($"{c.Id}. {c.Name} - {c.Email} - {c.Phone}");
        }

        Console.ReadKey();
    }
}
