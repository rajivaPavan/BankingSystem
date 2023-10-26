namespace BankingSystem.ViewModels;

public class CustomerSearchViewModel
{
    public string? ValidateNic { get; set; }
    public string? ValidateBusinessRegNo { get; set; }
    public string? SearchNic { get; set; }
    public string? SearchBusinessRegNo { get; set; }
    public bool Search { get; set; }
}