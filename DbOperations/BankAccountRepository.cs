using BankingSystem.DBContext;
using BankingSystem.ViewModels;

namespace BankingSystem.DbOperations;

public interface IBankAccountRepository
{
    public Task<IEnumerable<SavingsPlanViewModel>> GetSavingsPlans();
    Task AddSavingsAccount(AddBankAccountViewModel model, string accNo, int branchId);
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

    public async Task AddSavingsAccount(AddBankAccountViewModel model, string accNo, int branchId)
    {
        var conn = _context.GetConnection();
        using var cmd =  conn.CreateCommand();
        cmd.CommandText = "CALL add_savings_account(@acc_no, @customer_id, @branch_id, @initial_deposit, @open_date, @savings_plan_id)";
        cmd.Parameters.AddWithValue("@acc_no", accNo);
        cmd.Parameters.AddWithValue("@customer_id", model.CustomerId);
        cmd.Parameters.AddWithValue("@branch_id", branchId);
        cmd.Parameters.AddWithValue("@initial_deposit", model.InitialDeposit);
        cmd.Parameters.AddWithValue("@open_date", DateTime.Now.Date);
        cmd.Parameters.AddWithValue("@savings_plan_id", model.SavingsPlanId);
        await cmd.ExecuteNonQueryAsync();
    }
}