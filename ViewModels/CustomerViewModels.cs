using BankingSystem.Models;

namespace BankingSystem.ViewModels;

public class CustomerSearchViewModel
{
    public string? ValidateNic { get; set; }
    public string? ValidateBusinessRegNo { get; set; }
    public string? SearchNic { get; set; }
    public string? SearchBusinessRegNo { get; set; }
    public bool Search { get; set; }
    public bool? Found { get; set; }
    public int? IndividualId { get; set; }
}

public class IndividualViewModel : Individual
{
}