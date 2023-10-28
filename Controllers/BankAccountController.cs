using BankingSystem.DbOperations;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class BankAccountController : Controller
{
    private readonly BankRepository repository;

    public BankAccountController(BankRepository repository)
    {
        this.repository = repository;
    }

    public IActionResult AccountSummary(string customerId)
    {
        List<Account> accounts = repository.GetAccounts(customerId);
        return View(accounts);
    }

    
}