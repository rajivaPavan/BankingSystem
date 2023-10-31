using System.ComponentModel.DataAnnotations;

namespace BankingSystem.ViewModels;

public abstract class SelfRegistrationViewModel
{
    [Required]
    [Display(Name = "National Identity Card Number (NIC)")]
    public string NIC { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "Mobile Number")]
    public string MobileNumber { get; set; } = string.Empty;
}

public class CustomerSelfRegistrationViewModel : SelfRegistrationViewModel
{
    
    [Required]
    [Display(Name = "Bank Account Number ()")]
    public string BankAccountNumber { get; set; }
    
}

public class EmployeeSelfRegistrationViewModel : SelfRegistrationViewModel
{
    [Display(Name = "Employee ID")]
    public int? EmployeeId { get; set; }
}

public class FinalizeSelfRegisterViewModel
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