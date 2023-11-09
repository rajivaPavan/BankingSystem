using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;


namespace BankingSystem.Services;

public interface Emp_UserService
{
    Task<bool> EmployeeHasUserAccount(string nic);
    Task<bool> RegisterUser(User user, string password);
}
public class EmployeeService : Emp_UserService
{
    private readonly AppDbContext _dbContext;

    public async Task<bool> EmployeeHasUserAccount(string nic)
    {
        var conn = _dbContext.GetConnection();
        await conn.OpenAsync();
        bool res = false;
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText =
                "select check_individual_exists_has_user_account(@nic);";
            cmd.Parameters.AddWithValue("@nic", nic);
            res = (bool)await cmd.ExecuteScalarAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            await conn.CloseAsync();
        }
        return (bool)res;
    }

    public async Task<bool> RegisterUser(User user, string password)
    {
        var conn = _dbContext.GetConnection();
        await conn.OpenAsync();
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO user (user_name, user_type, password_hash) 
                            VALUES (@user_name, @user_type, @password_hash)";

            cmd.Parameters.AddWithValue("@user_name", user.UserName);
            cmd.Parameters.AddWithValue("@user_type", user.UserType);
            cmd.Parameters.AddWithValue("@password_hash", password);

            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        finally
        {
            await conn.CloseAsync();
        }

        return true;
    }
}
