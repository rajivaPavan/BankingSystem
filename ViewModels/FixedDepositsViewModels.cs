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

public class FixedDepositViewModel
{
    public int FdNo { get; set; }
    public string SavingsAccountNumber { get; set; }
    public double Amount { get; set; }
    public double InterestRate { get; set; }
    public int DurationInMonths { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime MaturityDate { get; set; }
}

public class ViewFixedDepositsViewModel
{
    public List<FixedDepositViewModel>? FixedDeposits { get; set; }
}