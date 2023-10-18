namespace BankingSystem.Models;

public class Customer
{
    public int Id { get; set; }
    public CustomerType Type { get; set; }
}

public enum CustomerType
{
    Individual,
    Organization
}

public enum Gender
{
    Male,
    Female
}

public class Individual
{
    public int IndividualId { get; set; }
    public string NIC { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int? CustomerId { get; set; }
    public int? UserId { get; set; }
    public string Email { get; set; }
    public Gender Gender { get; set; }
    public string MobileNumber { get; set; }
    public string HomeNumber { get; set; }
    public string Address { get; set; }
}