using BankingSystem.DbOperations;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class TransactionController : Controller
{
    private readonly TransactionRepository repository;

    public TransactionController()
    {
        repository = new TransactionRepository();
    }

    public IActionResult SearchTransactions(string accountNumber, DateTime startDate, DateTime endDate)
    {
        List<Transaction> transactions = repository.GetAccountTransactions(accountNumber);

        // Filter transactions within the specified date range
        transactions = transactions.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).ToList();

        // Pass the filtered transactions to the view
        return View("TransactionHistory", transactions);
    }

    public IActionResult FilterTransactions(string accountNumber, string transactionType)
    {
        List<Transaction> transactions = repository.GetAccountTransactions(accountNumber);

        // Filter transactions based on the transaction type
        transactions = transactions.Where(t => t.TransactionType == transactionType).ToList();

        // Pass the filtered transactions to the view
        return View("TransactionHistory", transactions);
    }
}