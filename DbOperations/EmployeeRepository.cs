using BankingSystem.DBContext;

namespace BankingSystem.DbOperations;

public interface IEmployeeRepository
{
    public Task<int> GetEmployeeBranchByUserId(int userId);
}

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> GetEmployeeBranchByUserId(int userId)
    {
        var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT branch_id FROM employee WHERE user_id = @user_id";
        cmd.Parameters.AddWithValue("@user_id", userId);
        var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync() == false)
            return -1;
        return reader.GetInt32(reader.GetOrdinal("branch_id"));
    }
}