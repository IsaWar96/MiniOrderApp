using MiniOrderApp.Infrastructure.Database;
using MiniOrderApp.Infrastructure.Repositories;
using MiniOrderApp.Presentation.UI;
using MiniOrderApp.Domain.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        // Connection string - database file will be saved as "miniorder.db" in the same folder as the program
        string connectionString = "Data Source=miniorder.db";

        DbInitializer.Initialize(connectionString);
        var factory = new SQLiteConnectionFactory(connectionString);

        ICustomerRepository customerRepo = new CustomerRepository(factory);
        IOrderRepository orderRepo = new OrderRepository(factory);
        IReturnRepository returnRepo = new ReturnRepository(factory);

        var menu = new MainMenu(customerRepo, orderRepo, returnRepo);
        menu.Start();
    }
}
