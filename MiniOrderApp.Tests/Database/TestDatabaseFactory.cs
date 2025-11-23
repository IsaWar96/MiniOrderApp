using Dapper;
using Microsoft.Data.Sqlite;

namespace MiniOrderApp.Tests.Database
{
    public static class TestDatabaseFactory
    {
        public static string CreateTestDatabase()
        {
            // Create a unique test database every iteration
            string dbPath = Path.Combine(
                Path.GetTempPath(),
                $"testdb_{Guid.NewGuid()}.db"
            );

            string connectionString = $"Data Source={dbPath}";

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            string schemaSql = File.ReadAllText("Database/schema.sql");
            connection.Execute(schemaSql);

            return connectionString;
        }
    }
}
