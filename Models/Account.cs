namespace BankingSystem.Models;

// Account Model
public class Account
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public AccountType AccountType { get; set; }
}

// Transaction Model
public class Transaction
{
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public string Recipient { get; set; }
    public decimal Amount { get; set; }
}

// Enums
public enum AccountType
{
    Savings,
    Current
}

public enum TransactionType
{
    Deposit,
    Withdrawal,
    Transfer,
    Payment
}