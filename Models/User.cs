namespace BankingSystem.Models;

public enum UserType
{
    Individual,
    Organization
}

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public UserType UserType { get; set; }
    public DateTime LastLoginTimestamp { get; set; }
}

public class UserRole
{
    public int UserRoleId { get; set; }
    public string Name { get; set; }
}

public class UserUserRole
{
    public int UserId { get; set; }
    public int UserRoleId { get; set; }
}