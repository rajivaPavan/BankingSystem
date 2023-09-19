using BankingSystem.DbOperations;

namespace BankingSystem.Services;

public interface IAuthenticationService
{
    bool Login(string username, string password);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public bool Login(string username, string password)
    {
        throw new NotImplementedException();
    }
}