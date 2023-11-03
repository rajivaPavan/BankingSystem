using System.Data;
using BankingSystem.DBContext;
using BankingSystem.Models;
using MySqlConnector;

namespace BankingSystem.DbOperations;

public interface IUserRepository : IRepository<User>
{
    Task<User?> AuthenticateUser(string username, string passwordHash);
    Task SignInAsync(User user);
    Task<bool> IsInRole(string username, int userType);
    Task<int> GetCustomerIdByUserId(int userUserId);
}

public class UserRepository : Repository, IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int id)
    {
        var connection = _dbContext.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"DELETE FROM user WHERE user_id = @u;";
        cmd.Parameters.AddWithValue("u", id);
        await cmd.ExecuteNonQueryAsync();
    }

    private async Task<User?> ReadUserAsync(MySqlDataReader reader)
    {
        
        // using reader make a new user
        if (await reader.ReadAsync() == false)
            return null;
        var user = new User();
        user.UserId = reader.GetInt32("user_id");
        user.UserName = reader.GetString("user_name");
        user.UserType = (UserType) reader.GetInt16("user_type");
        user.LastLoginTimestamp = reader.GetDateTime("last_login_timestamp");
        return user;
    }

    public async Task<User?> AuthenticateUser(string username, string passwordHash)
    {
        var connection = _dbContext.GetConnection();
        MySqlCommand cmd = new MySqlCommand("authenticate_user", connection);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@p_user_name", username);
        cmd.Parameters.AddWithValue("@p_password_hash", passwordHash);
        // return parameters
        cmd.Parameters.Add(new MySqlParameter("@o_user_type", MySqlDbType.Int32));
        cmd.Parameters["@o_user_type"].Direction = ParameterDirection.Output;
        cmd.Parameters.Add(new MySqlParameter("@o_user_id", MySqlDbType.Int32));
        cmd.Parameters["@o_user_id"].Direction = ParameterDirection.Output;
        
        await cmd.ExecuteNonQueryAsync();
        
        var userId = Convert.ToInt32(cmd.Parameters["@o_user_id"].Value);
        var userType = Convert.ToInt32(cmd.Parameters["@o_user_type"].Value);
        if (userId == -1)
            return null;

        return new User()
        {
            UserId = userId,
            UserName = username,
            UserType = (UserType) userType
        };
    }

    public async Task SignInAsync(User user)
    {
        var connection = _dbContext.GetConnection();
        await using var cmd = connection.CreateCommand();
        // set last login time
        cmd.CommandText = @"UPDATE user SET last_login_timestamp = @l WHERE user_id = @u;";
        cmd.Parameters.AddWithValue("l", DateTime.Now);
        cmd.Parameters.AddWithValue("o", new Random().Next(100000, 999999));
        cmd.Parameters.AddWithValue("u", user.UserId);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<bool> IsInRole(string username, int userType)
    {
        var connection = _dbContext.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "CALL has_usertype(@u, @t)";
        cmd.Parameters.AddWithValue("u", username);
        cmd.Parameters.AddWithValue("t", userType);
        return (bool) (await cmd.ExecuteScalarAsync() ?? false);
    }

    public async Task<int> GetCustomerIdByUserId(int userUserId)
    {
        var connection = _dbContext.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT customer_id FROM individual WHERE user_id = @u;";
        cmd.Parameters.AddWithValue("u", userUserId);
        var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync() == false)
            return -1;
        return reader.GetInt32(reader.GetOrdinal("customer_id"));
    }
}
