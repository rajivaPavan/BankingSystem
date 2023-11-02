using BankingSystem.DBContext;
using BankingSystem.ViewModels;

namespace BankingSystem.DbOperations;

public interface ITransferRepository
{
    Task AddTransfer(TransferViewModel model);
}

public class TransferRepository : ITransferRepository
{
    private readonly AppDbContext _context;

    public TransferRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddTransfer(TransferViewModel model)
    {
        var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "CALL add_new_transfer(@from_account_no, @to_account_no, @amount, @reference)";
        cmd.Parameters.AddWithValue("@from_account_no", model.SenderAccountId);
        cmd.Parameters.AddWithValue("@to_account_no", model.RecipientAccountId);
        cmd.Parameters.AddWithValue("@amount", model.Amount);
        cmd.Parameters.AddWithValue("@reference", model.Reference);
        await cmd.ExecuteNonQueryAsync();
    }
}