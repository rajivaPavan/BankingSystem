using System.ComponentModel.DataAnnotations;

namespace BankingSystem.ViewModels;

public class FdPlanViewModel
{
    public int Id { get; set; }
    public double InterestRate { get; set; }
    public int DurationInMonths { get; set; }
}

public class AddFixedDepositViewModel
{
    public List<FdPlanViewModel>? FdPlans { get; set; }
    
    [Required]
    [Display(Name = "Fixed Deposit Plan")]
    public int FdPlanId { get; set; }
    
    [Required]
    [Display(Name = "Fixed Deposit Amount")]
    public double Amount { get; set; }
    
    [Required]
    [Display(Name = "Savings Account Number")]
    [MaxLength(16)]
    public string SavingsAccountNumber { get; set; }
}