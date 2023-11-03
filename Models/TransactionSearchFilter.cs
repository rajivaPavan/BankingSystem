namespace BankingSystem.Models;

public class TransactionSearchFilter
{
    public string Keyword { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string TransactionType { get; set; }
}