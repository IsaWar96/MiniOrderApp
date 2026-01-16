using System.Data;
using Microsoft.Data.Sqlite;

namespace MiniOrderApp.Infrastructure.Database;

// Creates database connections
public class SQLiteConnectionFactory
{
    private readonly string _connectionString;

    public SQLiteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection Create()
    {
        return new SqliteConnection(_connectionString);
    }
}
