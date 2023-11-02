using BCrypt.Net;

namespace BankingSystem.Services;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash, string salt);
}

public class PasswordService : IPasswordService
{
    public string HashPassword(string password)
    {
        string salt = "$2a$12$djMzmKOH0UaCAychP/EIV.";
        string hash = BCrypt.Net.BCrypt.HashPassword(password, salt);
        return hash;
    }

    public bool VerifyPassword(string password, string hash, string salt)
    {
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
        return hashedPassword == hash;
    }
}
