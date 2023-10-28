using System.ComponentModel.DataAnnotations;

namespace BankingSystem.ViewModels;

public class SelfRegistrationViewModel
{
    [Required]
    [Display(Name = "National Identity Card Number (NIC)")]
    public string NIC { get; set; } = string.Empty;
    
    [Display(Name = "Bank Account Number ()")]
    public string BankAccountNumber { get; set; }
    
    [Display(Name = "Business Registration Number")]
    public string BusinessRegNo { get; set; }
    
    [Display(Name = "Employee ID")]
    public int? EmployeeId { get; set; }
    
    [Required]
    [Display(Name = "Mobile Number")]
    public string MobileNumber { get; set; } = string.Empty;
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