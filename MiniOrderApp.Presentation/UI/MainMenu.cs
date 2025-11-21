using MiniOrderApp.Domain.Interfaces;

namespace MiniOrderApp.Presentation.UI;

public class MainMenu
{
    private readonly ICustomerRepository _customers;
    private readonly IOrderRepository _orders;
    private readonly IReturnRepository _returns;

    public MainMenu(ICustomerRepository customers, IOrderRepository orders, IReturnRepository returns)
    {
        _customers = customers;
        _orders = orders;
        _returns = returns;
    }
    public void Start()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Mini OrderApp \n=========");
            Console.WriteLine("1. Customers");
            Console.WriteLine("2. Orders");
            Console.WriteLine("3. Returns");
            Console.WriteLine("0. Exit");

            Console.Write("Choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    new CustomerMenu(_customers).Show();
                    break;
                case "2":
                    new OrderMenu(_orders, _customers).Show();
                    break;
                case "3":
                    new ReturnMenu(_returns, _orders).Show();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    Console.ReadKey();
                    break;
            }
        }
    }
}
