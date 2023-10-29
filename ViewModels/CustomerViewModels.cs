using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BankingSystem.DbOperations;
using BankingSystem.Models;

namespace BankingSystem.ViewModels;

public class IndividualSearchViewModel
{
    [Required]
    [DisplayName("Search by NIC")]
    public string Nic { get; set; }
    
    [DisplayName("Search for child individual")]
    public bool CheckForChild { get; set; }  
    
    /// <summary>
    /// Used by view to show results.
    /// </summary>
    public bool Found { get; set; }

    /// <summary>
    /// Search result.
    /// </summary>
    public IEnumerable<IndividualViewModel> Result { get; set; }
    
}

public class IndividualViewModel
{
    public int IndividualId { get; set; }
    public int CustomerId { get; set; }
    public string NIC { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public Gender Gender { get; set; }
    public string MobileNumber { get; set; }
    public string? HomeNumber { get; set; }
    public string Address { get; set; }

    public int BranchId { get; set; }
    public string BranchName { get; set; }
    public string? BankAccountNumber { get; set; }
    public BankAccountType BankAccountType { get; set; }
    public double BankAccountBalance { get; set; }
}


public class IndividualBankAccountsViewModelForEmployee
{
    public List<IndividualViewModel> BankAccounts { get; set; }
    public bool CanMakeSavingsAcc { get; set; }
    public bool CanMakeCurrentAcc { get; set; }
    public int CustomerId { get; set; }
}