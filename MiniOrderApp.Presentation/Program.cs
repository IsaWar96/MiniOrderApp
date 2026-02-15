using Microsoft.EntityFrameworkCore;
using MiniOrderApp.Infrastructure.Database;
using MiniOrderApp.Infrastructure.Repositories;
using MiniOrderApp.Presentation.UI;
using MiniOrderApp.Domain.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        string connectionString = "Data Source=miniorder.db";

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlite(connectionString);

        using var dbContext = new ApplicationDbContext(optionsBuilder.Options);
        dbContext.Database.EnsureCreated();

        ICustomerRepository customerRepo = new CustomerRepository(dbContext);
        IOrderRepository orderRepo = new OrderRepository(dbContext);
        IReturnRepository returnRepo = new ReturnRepository(dbContext);

        var menu = new MainMenu(customerRepo, orderRepo, returnRepo);
        menu.Start();
    }
}
