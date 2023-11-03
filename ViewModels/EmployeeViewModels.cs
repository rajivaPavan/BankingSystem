using System.ComponentModel.DataAnnotations;



namespace BankingSystem.ViewModels;
public class SelfRegistrationEmployeeViewModel
{
    [Required]
    [Display(Name = "National Identity Card Number (NIC)")]
    public string NIC { get; set; } = string.Empty;

    [Display(Name = "Employee ID")]
    public int? EmployeeId { get; set; }

    [Required]
    [Display(Name = "Mobile Number")]
    public string MobileNumber { get; set; } = string.Empty;
}

public class FinalizeSelfRegisterEmployeeViewModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    [Required]
    public string OTP { get; set; }
}

public class EmployeeViewModel
{
    public int BranchId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Gender { get; set; }
    public string NIC { get; set; }
    public string Email { get; set; }
}

public class Employee
{
    public int UserId { get; set; } // Always set as 0
    public int BranchId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Gender { get; set; }
    public string NIC { get; set; }
    public string Email { get; set; }
    public string Phone { get; internal set; }
}