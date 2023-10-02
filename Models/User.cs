namespace BankingSystem.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    
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