namespace BankingSystem.Models;

public enum UserType
{
    Customer,
    Employee,
    Manager,
    Admin
}

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public UserType UserType { get; set; }
    public DateTime LastLoginTimestamp { get; set; }
}