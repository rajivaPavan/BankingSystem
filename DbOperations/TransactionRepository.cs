using BankingSystem.DBContext;
using BankingSystem.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingSystem.DbOperations;

public interface ITransactionRepository : IRepository<Transaction>
{
    List<Transaction> GetAccountTransactions(string accountNumber);
}

public class TransactionRepository : Repository, ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Transaction> GetAccountTransactions(string accountNumber)
    {
        List<Transaction> transactions = new List<Transaction>();

        using var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM Transactions WHERE AccountNumber = @AccountNumber";
        cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Transaction transaction = new Transaction
            {
                Id = reader.ToString(),
                AccountNumber = reader["AccountNumber"].ToString(),
                
            };

            transactions.Add(transaction);
        }

        return transactions;
    }

    public Task<Transaction> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(Transaction entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Transaction entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Transaction>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}
