using System.Data;
using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using MySqlConnector;

namespace BankingSystem.Services;

public interface IUserService
{
    Task<bool> IsInRole(string username, UserType userType);
    Task<int> IndividualValidationForRegistration(string nic, string bankAccountNumber, string checkMobileNumber);
    Task<bool> RegisterIndividualUser(User user, string password, int individualId);
    Task RegisterEmployeeUser(User user, string password, int employeeId);
}

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly IUserRepository _userRepository;
    private readonly IIndividualRepository _individualRepository;

    public UserService(AppDbContext dbContext, IUserRepository userRepository, 
        IIndividualRepository individualRepository)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
        _individualRepository = individualRepository;
    }
    
    /// <summary>
    /// Get user by id
    /// </summary>
    /// <param name="id">user_id</param>
    /// <returns>User</returns>
    public async Task<User?> GetUserById(int id)
    {
        await _dbContext.GetConnection().OpenAsync();
        var user = await _userRepository.GetByIdAsync(id);
        await _dbContext.GetConnection().CloseAsync();
        return user;
    }

    /// <summary>
    /// Check if user in role
    /// </summary>
    /// <param name="username">user_name</param>
    /// <param name="userType">user_type</param>
    /// <returns>bool</returns>
    public async Task<bool> IsInRole(string username, UserType userType)
    {
        await _dbContext.GetConnection().OpenAsync();
        // integer value of user_type
        var res = await _userRepository.IsInRole(username, (int)userType);
        await _dbContext.GetConnection().CloseAsync();
        return res;
    }

    /// <param name="nic"></param>
    /// <param name="bankAccountNumber"></param>
    /// <param name="checkMobileNumber"></param>
    /// <returns>individual_id if valid else -1</returns>
    public async Task<int> IndividualValidationForRegistration(string nic, string bankAccountNumber, string checkMobileNumber)
    {
        var conn =  _dbContext.GetConnection();
        var individualId = -1;

        var mobileNumber = "";
        try
        {
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT user_id,
                   individual_id,
                   mobile_number
            FROM (SELECT customer_id FROM bank_account AS b WHERE b.account_no = @acc_no) AS ba
                     JOIN customer AS c ON ba.customer_id = c.id
                     JOIN individual AS i ON c.id = i.customer_id
            WHERE i.nic = @nic";
            
            cmd.Parameters.AddWithValue("@acc_no", bankAccountNumber);
            cmd.Parameters.AddWithValue("@nic", nic);

            var userId = -1;
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                if (!reader.IsDBNull(reader.GetOrdinal("user_id")))
                    userId = reader.GetInt32(0);
                if (userId != -1)
                    return -1;
                if (!reader.IsDBNull(reader.GetOrdinal("individual_id")))
                    individualId = reader.GetInt32(1);
                else
                {
                    throw new Exception("Individual does not exist");
                }
                // if individual exists so does mobile number
                if(individualId == -1)
                    return -1;
                
                mobileNumber = reader.GetString(2);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            await conn.CloseAsync();
        }
        if(checkMobileNumber != mobileNumber)
            throw new Exception("Invalid mobile number");
        
        return individualId;
        
    }

    public async Task<bool> RegisterIndividualUser(User user, string password, int individualId)
    {
        var conn = _dbContext.GetConnection();
        await conn.OpenAsync();
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "CALL register_individual_user(@user_name, @password_hash, @individual_id);";
        
            cmd.Parameters.AddWithValue("@user_name", user.UserName);
            cmd.Parameters.AddWithValue("@password_hash", password);
            cmd.Parameters.AddWithValue("individual_id", individualId);
        
            await cmd.ExecuteNonQueryAsync();
        }
        catch (MySqlException e)
        {
            if (e.SqlState == "45000")
            {
                await conn.CloseAsync();
                throw new Exception("Individual already has a user account");
            }
        }
        finally
        {
            await conn.CloseAsync();
        }
        
        return true;
    }

    public async Task RegisterEmployeeUser(User user, string password, int employeeId)
    {
        var conn = _dbContext.GetConnection();
        await conn.OpenAsync();
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "CALL register_banker_user(@user_name, @password_hash, @employee_id);";
            cmd.Parameters.AddWithValue("@user_name", user.UserName);
            cmd.Parameters.AddWithValue("@password_hash", password);
            cmd.Parameters.AddWithValue("employee_id", employeeId);
            await cmd.ExecuteNonQueryAsync();
        }
        catch (MySqlException e)
        {
            if (e.SqlState == "45000")
            {
                await conn.CloseAsync();
                throw new Exception("Employee already has a user account");
            }
        }
        finally
        {
            await conn.CloseAsync();
        }
    }
}