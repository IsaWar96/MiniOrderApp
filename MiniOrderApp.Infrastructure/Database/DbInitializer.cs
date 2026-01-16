using Microsoft.Data.Sqlite;

namespace MiniOrderApp.Infrastructure.Database;

// Initializes database and creates tables on first run
public static class DbInitializer
{
    public static void Initialize(string connectionString)
    {
        using var conn = new SqliteConnection(connectionString);
        conn.Open();

        CreateTables(conn);
        AddTestData(conn);
    }

    private static void CreateTables(SqliteConnection conn)
    {
        string createCustomersTable = @"
            CREATE TABLE IF NOT EXISTS Customers (
                CustomerId INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Email TEXT NOT NULL,
                Phone TEXT
            );";

        string createOrdersTable = @"
            CREATE TABLE IF NOT EXISTS Orders (
                OrderId INTEGER PRIMARY KEY AUTOINCREMENT,
                CustomerId INTEGER NOT NULL,
                OrderDate TEXT NOT NULL,
                Status TEXT NOT NULL,
                TotalAmount REAL NOT NULL,
                FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
            );";

        string createOrderItemsTable = @"
            CREATE TABLE IF NOT EXISTS OrderItems (
                OrderItemId INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderId INTEGER NOT NULL,
                ProductName TEXT NOT NULL,
                Quantity INTEGER NOT NULL,
                UnitPrice REAL NOT NULL,
                FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
            );";

        string createReturnsTable = @"
            CREATE TABLE IF NOT EXISTS Returns (
                ReturnId INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderId INTEGER NOT NULL UNIQUE,
                ReturnDate TEXT NOT NULL,
                Reason TEXT,
                RefundedAmount REAL NOT NULL,
                FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
            );";

        ExecuteCommand(conn, createCustomersTable);
        ExecuteCommand(conn, createOrdersTable);
        ExecuteCommand(conn, createOrderItemsTable);
        ExecuteCommand(conn, createReturnsTable);
    }

    private static void AddTestData(SqliteConnection conn)
    {
        using var checkCmd = conn.CreateCommand();
        checkCmd.CommandText = "SELECT COUNT(*) FROM Customers";
        var count = checkCmd.ExecuteScalar();
        
        if (Convert.ToInt32(count) > 0)
            return;

        string insertCustomer = @"
            INSERT INTO Customers (Name, Email, Phone) 
            VALUES ('Test Customer', 'test@example.com', '0700000000');";
        ExecuteCommand(conn, insertCustomer);

        string insertOrder = @"
            INSERT INTO Orders (CustomerId, OrderDate, Status, TotalAmount) 
            VALUES (1, '2025-01-01', 'Created', 300);";
        ExecuteCommand(conn, insertOrder);

        string insertOrderItems = @"
            INSERT INTO OrderItems (OrderId, ProductName, Quantity, UnitPrice) 
            VALUES 
                (1, 'Product A', 1, 100),
                (1, 'Product B', 2, 100);";
        ExecuteCommand(conn, insertOrderItems);
    }

    private static void ExecuteCommand(SqliteConnection conn, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
}
