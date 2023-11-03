using BankingSystem.DBContext;
using BankingSystem.ViewModels;

namespace BankingSystem.DbOperations;

public interface IEmployeeRepository
{
    public Task<int> GetEmployeeBranchByUserId(int userId);
    Task<int> AddEmployee(Employee employee);
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

    public async Task<int> AddEmployee(Employee employee)
    {
        var conn = _context.GetConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO `employee` (`user_id`, `branch_id`, `first_name`, `last_name`, `date_of_birth`, `gender`, `NIC`, `email`) " +
            "VALUES (@user_id, @branch_id, @first_name, @last_name, @date_of_birth, @gender, @NIC, @email);";
        cmd.Parameters.AddWithValue("@branch_id", employee.BranchId);
        cmd.Parameters.AddWithValue("@first_name", employee.FirstName);
        cmd.Parameters.AddWithValue("@last_name", employee.LastName);
        cmd.Parameters.AddWithValue("@date_of_birth", employee.DateOfBirth);
        cmd.Parameters.AddWithValue("@gender", employee.Gender);
        cmd.Parameters.AddWithValue("@NIC", employee.NIC);
        cmd.Parameters.AddWithValue("@email", employee.Email);

        await cmd.ExecuteNonQueryAsync();
        await conn.CloseAsync();

        return 1; // Return any value to indicate successful insertion.
    }


}