using BankingSystem.DBContext;
using BankingSystem.ViewModels;

namespace BankingSystem.DbOperations;

public interface IBankAccountRepository
{
    public Task<IEnumerable<SavingsPlanViewModel>> GetSavingsPlans();
}

public class BankAccountRepository : IBankAccountRepository
{
    private readonly AppDbContext _context;

    public BankAccountRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<SavingsPlanViewModel>> GetSavingsPlans()
    {
        var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM savings_plan";
        using var reader = await cmd.ExecuteReaderAsync();
        var savingsPlans = new List<SavingsPlanViewModel>();
        while (reader.Read())
        {
            var savingsPlan = new SavingsPlanViewModel()
            {
                SavingsPlanId = reader.GetInt32("savings_plan_id"),
                Name = reader.GetString("name"),
                InterestRate = reader.GetDouble("interest"),
                MinimumBalance = reader.GetInt16("minimum"),
                MaxWithdrawals = reader.GetInt32("max_withdrawals")
            };
            
            savingsPlans.Add(savingsPlan);
        }
        
        return savingsPlans;
    }
}