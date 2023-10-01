﻿using System.Collections;
using BankingSystem.DBContext;
using BankingSystem.Models;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace BankingSystem.DbOperations;

public interface IUserRepository : IRepository<User>
{
    Task<User> AuthenticateUser(string username, string password);
    Task<IEnumerable<UserRole>> GetUserRoles(int userId);
}

public class UserRepository : Repository<User>, IUserRepository
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

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<User> AuthenticateUser(string username, string password)
    {
        var connection = _dbContext.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT * FROM user 
                        WHERE Username = @u AND PasswordHash = SHA(@p);";
        cmd.Parameters.AddWithValue("u", username);
        cmd.Parameters.AddWithValue("p", password);
        var reader = await cmd.ExecuteReaderAsync();
        var res = await Read<User>(reader);
        return res.ElementAt(0);
    }

    public async Task<IEnumerable<UserRole>> GetUserRoles(int userId)
    {
        var connection = _dbContext.GetConnection();
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = @"SELECT ur.UserRoleId, ur.Name
                        FROM user_user_roles JOIN user_role ur 
                        ON ur.UserRoleId = user_user_roles.UserRoleId 
                        WHERE UserId = @u;";
        cmd.Parameters.AddWithValue("u", userId);
        var reader = await cmd.ExecuteReaderAsync();
        var res = await Read<UserRole>(reader);
        return res;
    }
}
