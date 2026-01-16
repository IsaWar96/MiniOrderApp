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
            string[] options = { "Customers", "Orders", "Returns", "Exit" };
            int choice = MenuHelper.ShowArrowMenu("Mini OrderApp\n=========", options);

            if (choice == -1 || choice == 3)
                return;

            switch (choice)
            {
                case 0:
                    new CustomerMenu(_customers).Show();
                    break;
                case 1:
                    new OrderMenu(_orders, _customers).Show();
                    break;
                case 2:
                    new ReturnMenu(_returns, _orders).Show();
                    break;
            }
        }
    }
}
