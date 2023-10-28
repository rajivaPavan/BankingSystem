using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;

namespace BankingSystem.Services;

public interface IUserService
{
    Task<bool> IsInRole(string username, UserType userType);
    Task<bool> IndividualHasUserAccount(string nic, string bankAccountNumber);
    Task<bool> RegisterUser(User user, string password);
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
    
    public async Task<bool> IndividualHasUserAccount(string nic, string bankAccountNumber)
    {
        var conn =  _dbContext.GetConnection();
        await conn.OpenAsync();
        bool res = false;
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = 
                "select check_individual_exists_has_user_account(@nic, @account_no, '', @is_individual);";
            cmd.Parameters.AddWithValue("@nic", nic);
            cmd.Parameters.AddWithValue("@account_no", bankAccountNumber);
            cmd.Parameters.AddWithValue("@is_individual", true);
            res = (bool) await cmd.ExecuteScalarAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            await conn.CloseAsync();
        }
        return (bool) res;
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