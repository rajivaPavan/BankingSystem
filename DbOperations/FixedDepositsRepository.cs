using BankingSystem.DBContext;
using BankingSystem.ViewModels;
using MySqlConnector;

namespace BankingSystem.DbOperations;

public interface IFixedDepositsRepository
{
    public Task<List<FdPlanViewModel>> GetFdPlansAsync();
    public Task AddFixedDepositAsync(AddFixedDepositViewModel addFixedDepositViewModel);
    Task<List<FixedDepositViewModel>?> GetFixedDeposits();
}

public class FixedDepositsRepository : IFixedDepositsRepository
{
    private readonly AppDbContext _context;

    public FixedDepositsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FdPlanViewModel>> GetFdPlansAsync()
    {
        var conn = _context.GetConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM fd_plan";
        var reader = await cmd.ExecuteReaderAsync();
        var fdPlans = new List<FdPlanViewModel>();
        while (await reader.ReadAsync())
        {
            var fdPlan = new FdPlanViewModel
            {
                Id = reader.GetInt32("fd_plan_id"),
                InterestRate = reader.GetDouble("interest"),
                DurationInMonths = reader.GetSByte("duration")
            };
            fdPlans.Add(fdPlan);
        }

        await conn.CloseAsync();

        return fdPlans;
    }

    public async Task AddFixedDepositAsync(AddFixedDepositViewModel addFixedDepositViewModel)
    {
        var conn = _context.GetConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "CALL add_fixed_deposit(@acc_no, @fd_plan_id, @amount)";
        cmd.Parameters.AddWithValue("@acc_no", addFixedDepositViewModel.SavingsAccountNumber);
        cmd.Parameters.AddWithValue("@fd_plan_id", addFixedDepositViewModel.FdPlanId);
        cmd.Parameters.AddWithValue("@amount", addFixedDepositViewModel.Amount);
        try
        {
            await cmd.ExecuteNonQueryAsync();
        }
        catch (MySqlException e)
        {
            if (e.SqlState == "45000")
            {
                await conn.CloseAsync();
                throw new Exception(e.Message);
            }
        }
        await conn.CloseAsync();
    }

    public async Task<List<FixedDepositViewModel>?> GetFixedDeposits()
    {
        var conn = _context.GetConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT fd.*, p.*, 
            DATE_ADD(opening_date, interval duration month) as maturity_date 
            from fixed_deposit as fd JOIN fd_plan as p 
                on fd.fd_plan_id = p.fd_plan_id;";
        var reader = await cmd.ExecuteReaderAsync();
        var fixedDeposits = new List<FixedDepositViewModel>();
        while (await reader.ReadAsync())
        {
            var fixedDeposit = new FixedDepositViewModel
            {
                FdNo = reader.GetInt32("fd_no"),
                SavingsAccountNumber = reader.GetString("savings_account_no"),
                Amount = reader.GetDouble("amount"),
                InterestRate = reader.GetDouble("interest"),
                DurationInMonths = reader.GetInt32("duration"),
                OpenDate = reader.GetDateTime("opening_date"),
                MaturityDate = reader.GetDateTime("maturity_date")
            };
            fixedDeposits.Add(fixedDeposit);
        }
        await conn.CloseAsync();
        return fixedDeposits;
    }
}