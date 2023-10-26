using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;

namespace BankingSystem.Services;

public interface IUserService
{
    Task<bool> IsInRole(string username, UserType userType);
}

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly IUserRepository _userRepository;

    public UserService(AppDbContext dbContext, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
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
}