using BankingSystem.DbOperations;
using BankingSystem.Models;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class CustomerHomeController : Controller
{
    private readonly IBankRepository repository;

    public CustomerHomeController(IBankRepository repository)
    {
        this.repository = repository;
    }

    public IActionResult AccountSummary(string customerId)
    {
        List<Account> accounts = repository.GetAccounts(customerId);
        return View(accounts);
    }
}