using BankingSystem.Constants;
using BankingSystem.Controllers;
using BankingSystem.DBContext;
using BankingSystem.ViewModels;
using MySqlConnector;

namespace BankingSystem.DbOperations;

public interface ITransactionsRepository
{
    Task AddTransaction(TransactionsViewModel model, TransactionType transactionType);
}

public class TransactionsRepository : ITransactionsRepository
{
    private readonly AppDbContext _context;

    public TransactionsRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddTransaction(TransactionsViewModel model, TransactionType transactionType)
    {
        var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "call add_new_transaction(@acc_no, @type, @amount, 'transaction-ref')";
        cmd.Parameters.AddWithValue("@acc_no", model.AccountNumber);
        cmd.Parameters.AddWithValue("@type", (sbyte)transactionType);
        cmd.Parameters.AddWithValue("@amount", model.Amount);
        try
        {
            await cmd.ExecuteNonQueryAsync();
        }
        catch (MySqlException e)
        {
            if (e.SqlState == "45000")
            {
                throw;
            }
        }
    }
}