using BankingSystem.DBContext;
using BankingSystem.Models;
using MySqlConnector;

namespace BankingSystem.DbOperations;

public interface IBankRepository
{
    public Task<List<Account>> GetAccounts(int customerId);
}

public class BankRepository : IBankRepository
{
    private readonly AppDbContext _context;

    public BankRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<List<Account>> GetAccounts(int customerId)
    {
        List<Account> accounts = new List<Account>();
        using var conn = _context.GetConnection();
        
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT account_no, balance, account_type FROM bank_account WHERE customer_id = @CustomerId";
        cmd.Parameters.AddWithValue("@CustomerId", customerId);

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
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