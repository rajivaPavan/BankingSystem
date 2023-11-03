namespace BankingSystem.Models;

// Account Model
public class Account
{
    public string AccountNumber { get; set; }
    public double Balance { get; set; }
    public AccountType AccountType { get; set; }
}

// Transaction Model
public class Transaction
{
    public string Id { get; set; }

    public string AccountNumber { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public string Recipient { get; set; }
    public double Amount { get; set; }
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