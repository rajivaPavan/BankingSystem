using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class TransactionController : Controller
{
    private readonly ITransactionRepository repository;

    public TransactionController(ITransactionRepository transactionRepository)
    {
        this.repository = transactionRepository;
    }

    public IActionResult SearchTransactions(string accountNumber, DateTime startDate, DateTime endDate)
    {
        List<Transaction> transactions = repository.GetAccountTransactions(accountNumber);

        // Filter transactions within the specified date range
        transactions = transactions.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).ToList();

        // Pass the filtered transactions to the view
        return View("AccountTransactions", transactions);
    }

    public IActionResult FilterTransactions(string accountNumber, string transactionType)
    {
        List<Transaction> transactions = repository.GetAccountTransactions(accountNumber);

        // Filter transactions based on the transaction type
        transactions = transactions.Where(t => t.TransactionType == transactionType).ToList();

        // Pass the filtered transactions to the view
        return View("AccountTransactions", transactions);
    }
}