using System.ComponentModel.DataAnnotations;

namespace BankingSystem.ViewModels;

public class TransactionsViewModel
{
    [Required(ErrorMessage = "Account Number is required")]
    [Display(Name = "Account Number")]
    public string AccountNumber { get; set; }

    [Required(ErrorMessage = "Amount is required")]
    [Display(Name = "Amount")]
    public double Amount { get; set; }
}