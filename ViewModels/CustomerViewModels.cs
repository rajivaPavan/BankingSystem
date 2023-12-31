﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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



public class OrganizationIndividualViewModel
{
    public int CustomerId { get; set; }
    public int IndividualId { get; set; }
    
    [Display(Name = "Organization Registration Number")]
    public string OrganizationRegNo { get; set; }
    public string Name { get; set; }
    
    [Display(Name = "Work Email")]
    public string WorkEmail { get; set; }
    
    [Display(Name = "National Identity Card Number")]
    public string NIC { get; set; }
    
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    
    public string Position { get; set; }
    public Gender Gender { get; set; }

    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    public string? WorkPhone { get; set; }
}

public class OrganizationViewModel
{
    public int? CustomerId { get; set; }

    [Required]
    [Display(Name = "Organization Registration Number")]
    public string RegNo { get; set; }
    
    [Required]
    [Display(Name = "Organization Name")]
    public string Name { get; set; }
    
    [Display(Name = "Company Email")]
    public string CompanyEmail { get; set; }
    
    [Display(Name = "Organization Type")]
    public string Type { get; set; }
    
    [Display(Name = "Mobile Number")]
    public string? HomeNumber { get; set; }
    
    [Display(Name = "Organization Address")]
    public string Address { get; set; }
    public List<OrganizationIndividualViewModel>? Owners { get; set; }
}

public class OrganizationSearchViewModel
{
    public string RegNo { get; set; }
    public bool? Found { get; set; }
    public OrganizationViewModel? Result { get; set; }
}

public class CreateOrganizationViewModel : OrganizationViewModel
{
    public OrganizationIndividualViewModel Owner { get; set; }
}

public class TransferViewModel
{
    public string SenderAccountId { get; set; }
    public string RecipientAccountId { get; set; }
    public double Amount { get; set; }
    public string Reference { get; set; }

    public SelectList SenderAccounts { get; set; }
}

public class AccountViewModel
{
    public string AccountNumber { get; set; }

}