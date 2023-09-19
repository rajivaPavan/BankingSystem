using MySqlConnector;

namespace BankingSystem.DBContext;

public class AppDbContext
{
    private readonly MySqlConnection _connection;

    public AppDbContext(string connectionString)
    {
        _connection = new MySqlConnection(connectionString);
    }
    
    public MySqlConnection GetConnection() => _connection;
}