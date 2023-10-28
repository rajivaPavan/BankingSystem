using System.Data;
using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using MySqlConnector;

namespace BankingSystem.Services;

public interface IUserService
{
    Task<bool> IsInRole(string username, UserType userType);
    Task<int> IndividualHasUserAccount(string nic, string bankAccountNumber);
    Task<bool> RegisterIndividualUser(User user, string password, int individualId);
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
    
    public async Task<int> IndividualHasUserAccount(string nic, string bankAccountNumber)
    {
        var conn =  _dbContext.GetConnection();
        int res = -1;
        try
        {
            await conn.OpenAsync();
            // Create a MySqlCommand to call the stored procedure
            MySqlCommand cmd = new MySqlCommand("individual_exists_has_user_account", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Set input parameters
            cmd.Parameters.AddWithValue("@p_nic", nic);
            cmd.Parameters.AddWithValue("@p_bankAccount", bankAccountNumber);

            // Create output parameters
            cmd.Parameters.Add(new MySqlParameter("@o_user_id", MySqlDbType.Int32));
            cmd.Parameters["@o_user_id"].Direction = ParameterDirection.Output;

            cmd.Parameters.Add(new MySqlParameter("@o_individual_id", MySqlDbType.Int32));
            cmd.Parameters["@o_individual_id"].Direction = ParameterDirection.Output;

            await cmd.ExecuteNonQueryAsync();

            int o_user_id = Convert.ToInt32(cmd.Parameters["@o_user_id"].Value);
            int o_individual_id = Convert.ToInt32(cmd.Parameters["@o_individual_id"].Value);

            if (o_user_id == -1)
                res= o_individual_id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            await conn.CloseAsync();
        }
        return res;
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