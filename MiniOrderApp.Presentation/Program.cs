using MiniOrderApp.Infrastructure.Database;
using MiniOrderApp.Infrastructure.Repositories;
internal class Program
{
    private static void Main(string[] args)
    {
        var conn = new SQLiteConnectionFactory("Data Source=miniorder.db");
        var orderRepo = new OrderRepository(conn);
    }
}