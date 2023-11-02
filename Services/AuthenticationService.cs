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
    private readonly IEmployeeRepository _employeeRepository;
    private readonly AppDbContext _dbContext;

    public AuthenticationService(AppDbContext dbContext,
        IUserRepository userRepository, 
        IHttpContextAccessor httpContextAccessor,
        IEmployeeRepository employeeRepository
        )
    {
        _dbContext = dbContext;
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _employeeRepository = employeeRepository;
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

        string? branchId = null;
        string? customerId = null;
        if (user.UserType is UserType.Employee or UserType.Manager)
        {
            // get branch from employee table 
            var id = await _employeeRepository.GetEmployeeBranchByUserId(user.UserId);
            if (id != -1)
                branchId = id.ToString();
        }
        else if(user.UserType is UserType.Customer)
        {

        }
        
        await _dbContext.GetConnection().CloseAsync();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, user.UserType.ToString())
        };

        if (branchId != null)
        {
            // add BranchId claim
            claims.Add(new Claim("BranchId", branchId));
        }
        if (customerId != null)
        {
            claims.Add(new Claim("Customer", customerId));
        }
        
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