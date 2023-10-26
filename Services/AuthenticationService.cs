using System.Security.Claims;
using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BankingSystem.Services;

public interface IAuthenticationService
{
    Task<bool> Login(string username, string password);
    Task Logout();
}

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _dbContext;

    public AuthenticationService(AppDbContext dbContext,
        IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<bool> Login(string username, string password)
    {
        // check in db
        await _dbContext.GetConnection().OpenAsync();
        var user = await _userRepository.AuthenticateUser(username, password);

        if (user == null)
        {
            await _dbContext.GetConnection().CloseAsync();
            return false;
        }
        
        await _userRepository.SignInAsync(user);
        await _dbContext.GetConnection().CloseAsync();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
        };

        claims.AddRange(new[] {new Claim(ClaimTypes.Role, user.UserType.ToString())});
        
        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
            IssuedUtc = DateTimeOffset.UtcNow,
        };

        await _httpContextAccessor.HttpContext!.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity), 
            authProperties);

        return true;
    }

    public async Task Logout()
    {
        await _httpContextAccessor.HttpContext!.SignOutAsync();
    }
}