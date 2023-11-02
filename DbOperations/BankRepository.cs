using BankingSystem.DBContext;
using BankingSystem.Models;
using MySqlConnector;

namespace BankingSystem.DbOperations;

public interface IBankRepository
{
    public List<Account> GetAccounts(string customerId);
}

public class BankRepository : IBankRepository
{
    private readonly AppDbContext _context;

    public BankRepository(AppDbContext context)
    {
        _context = context;
    }


    public List<Account> GetAccounts(string customerId)
    {
        List<Account> accounts = new List<Account>();
        using var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT AccountNumber, Balance, AccountType FROM BankAccount WHERE CustomerId = @CustomerId";
        cmd.Parameters.AddWithValue("@CustomerId", customerId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Account account = new Account()
            {
                AccountNumber = reader.GetString(0),
                Balance = reader.GetDouble(1),
                AccountType = (AccountType)reader.GetSByte(2)
            };

            accounts.Add(account);
        }

        return accounts;
    }


}