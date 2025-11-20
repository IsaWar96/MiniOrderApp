using Microsoft.Data.Sqlite;

namespace MiniOrderApp.Infrastructure.Database;

public static class DbInitializer
{
    public static void Initialize(string connectionstring)
    {
        // Get file name from connectionstring.
        var builder = new SqliteConnectionStringBuilder(connectionstring);
        var dbFile = builder.DataSource;

        // Check if database already exists.
        bool exists = File.Exists(dbFile);

        // Opens connection (creates file if it does not exist).
        using var conn = new SqliteConnection(connectionstring);
        conn.Open();

        // If file does not exist create tables and insert data.
        if (!exists)
        {
            RunSchema(conn);
            SeedData(conn);
        }
    }
    private static void RunSchema(SqliteConnection conn)
    {
        var baseDir = AppContext.BaseDirectory;
        var schemaPath = Path.Combine(baseDir, "schema.sql");

        if (!File.Exists(schemaPath))
            throw new ArgumentException("schema.sql not found", nameof(schemaPath));

        var sql = File.ReadAllText(schemaPath);

        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
    private static void SeedData(SqliteConnection conn)
    {
        using var cmd = conn.CreateCommand();

        cmd.CommandText = @" INSERT INTO Customers (Name, Email, Phone) VALUES
        ('Test Customer', 'test@example.com', '0700000000');

        INSERT INTO Orders (CustomerId, OrderDate, Status, TotalAmount) VALUES
        (1, '2025-01-01', 'Created', 300);

        INSERT INTO OrderItems (OrderId, ProductName, Quantity, UnitPrice) VALUES
        (1, 'Product A', 1, 100),
        (1, 'Product B', 2, 100);
        ";

        cmd.ExecuteNonQuery();
    }
}
