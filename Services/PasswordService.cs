using BCrypt.Net;

namespace BankingSystem.Services;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash, string salt);
}


public class PasswordService : IPasswordService
{
    private readonly string _salt = "abcdefg12345";

    public string HashPassword(string password)
    {
        string hash = BCrypt.Net.BCrypt.HashPassword(password, _salt);
        return hash;
    }

    public bool VerifyPassword(string password, string hash, string salt)
    {
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
        return hashedPassword == hash;
    }
}
