using BankingSystem.DBContext;
using BankingSystem.ViewModels;
using MySqlConnector;

namespace BankingSystem.DbOperations;

public interface IFixedDepositsRepository
{
    public Task<List<FdPlanViewModel>> GetFdPlansAsync();
    public Task AddFixedDepositAsync(AddFixedDepositViewModel addFixedDepositViewModel);
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
}