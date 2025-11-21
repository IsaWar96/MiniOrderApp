using MiniOrderApp.Infrastructure.Database;
using MiniOrderApp.Infrastructure.Repositories;
using MiniOrderApp.Presentation.UI;
using MiniOrderApp.Domain.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        var connectionString = "Data Source=miniorder.db";

        DbInitializer.Initialize(connectionString);

        var factory = new SQLiteConnectionFactory(connectionString);

        ICustomerRepository customerRepo = new CustomerRepository(factory);
        IOrderRepository orderRepo = new OrderRepository(factory);
        IReturnRepository returnRepo = new ReturnRepository(factory);

        var menu = new MainMenu(customerRepo, orderRepo, returnRepo);
        menu.Start();
    }
}
