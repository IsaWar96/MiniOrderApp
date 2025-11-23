using Dapper;
using Microsoft.Data.Sqlite;

namespace MiniOrderApp.Tests.Database
{
    public static class TestDatabaseFactory
    {
        public static string CreateTestDatabase()
        {
            // Temporary SQLite file
            var dbPath = Path.GetTempFileName();
            var connectionString = $"Data Source={dbPath}";

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var schemaSql = File.ReadAllText("Database/schema.sql");
            connection.Execute(schemaSql);

            return connectionString;
        }
    }
}
