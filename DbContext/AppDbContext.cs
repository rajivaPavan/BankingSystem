using System.Reflection;
using MySqlConnector;

namespace BankingSystem.DBContext;

public class AppDbContext : IDisposable
{
    private readonly MySqlConnection _connection;

    public AppDbContext(string connectionString)
    {
        _connection = new MySqlConnection(connectionString);
    }
    
    public void Dispose() => _connection.Dispose();
    
    public MySqlConnection GetConnection() => _connection;
}