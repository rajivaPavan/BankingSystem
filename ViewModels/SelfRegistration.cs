using System.ComponentModel.DataAnnotations;

namespace BankingSystem.ViewModels;

public class SelfRegistrationViewModel
{
    [Required]
    public string NIC { get; set; } = string.Empty;
    
    [Required]
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