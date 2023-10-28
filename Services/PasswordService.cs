using BCrypt.Net;

namespace BankingSystem.Services;

public interface IPasswordService
{
    (string Hash, string Salt) HashPassword(string password);
    bool VerifyPassword(string password, string hash, string salt);
}


public class PasswordService : IPasswordService
{
    public (string Hash, string Salt) HashPassword(string password)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt(12); // Generate a random salt with a cost factor of 12
        string hash = BCrypt.Net.BCrypt.HashPassword(password, salt);
        return (hash, salt);
    }

    public bool VerifyPassword(string password, string hash, string salt)
    {
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
        return hashedPassword == hash;
    }
}
