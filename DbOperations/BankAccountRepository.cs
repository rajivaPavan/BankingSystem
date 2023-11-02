using BankingSystem.DBContext;
using BankingSystem.ViewModels;
using MySqlConnector;

namespace BankingSystem.DbOperations;

public interface IBankAccountRepository
{
    Task<IEnumerable<SavingsPlanViewModel>> GetSavingsPlans();
    Task AddSavingsAccount(AddBankAccountViewModel model, string accNo, int branchId);
    Task<bool> HasAccountInBranch(int customerId, int branchId, BankAccountType type);
    Task<IEnumerable<SavingsPlanViewModel>> GetSavingsPlansForCustomer(int customerId);
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
            var savingsPlan = SavingsPlanViewModel(reader);
            savingsPlans.Add(savingsPlan);
        }
        
        return savingsPlans;
    }

    private static SavingsPlanViewModel SavingsPlanViewModel(MySqlDataReader reader)
    {
        var savingsPlan = new SavingsPlanViewModel()
        {
            SavingsPlanId = reader.GetInt32("savings_plan_id"),
            Name = reader.GetString("name"),
            InterestRate = reader.GetDouble("interest"),
            MinimumBalance = reader.GetInt16("minimum"),
            MaxWithdrawals = reader.GetInt32("max_withdrawals")
        };
        return savingsPlan;
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

    public async Task<bool> HasAccountInBranch(int customerId, int branchId, BankAccountType type)
    {
        var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT EXISTS(SELECT * FROM bank_account " +
                          "WHERE customer_id = @customer_id " +
                          "AND branch_id = @branch_id " +
                          "AND account_type = @t)";
        cmd.Parameters.AddWithValue("@customer_id", customerId);
        cmd.Parameters.AddWithValue("@branch_id", branchId);
        cmd.Parameters.AddWithValue("@t", (sbyte)(type));
        var result = await cmd.ExecuteScalarAsync();
        return (long) result == 1;
    }

    public async Task<IEnumerable<SavingsPlanViewModel>> GetSavingsPlansForCustomer(int customerId)
    {
        var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT timestampdiff(YEAR, date_of_birth, CURDATE()) as age FROM individual WHERE customer_id = @c";
        cmd.Parameters.AddWithValue("@c", customerId);
        // read date of birth
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync() == false)
            throw new Exception("Customer not found");
        var age = reader.GetInt32("age");
        await reader.CloseAsync();
        cmd.CommandText = "SELECT * FROM savings_plan WHERE age_lower_bound <= @age AND age_upper_bound >= @age";
        cmd.Parameters.AddWithValue("@age", age);
        var reader2 = await cmd.ExecuteReaderAsync();
        var savingsPlans = new List<SavingsPlanViewModel>();
        while (await reader2.ReadAsync())
        {
            var savingsPlan = SavingsPlanViewModel(reader2);
            savingsPlans.Add(savingsPlan);
        }

        return savingsPlans;
    }
}

public enum BankAccountType
{
    Current = 0,
    Savings = 1
}