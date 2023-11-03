using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class CustomerHomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly IBankRepository repository;

    public CustomerHomeController(AppDbContext context, IBankRepository repository)
    {
        _context = context;
        this.repository = repository;
    }
    
    public async Task<IActionResult> AccountSummary()
    {
        var customerId = int.Parse(HttpContext.User.FindFirst("CustomerId")!.Value);
        var conn = _context.GetConnection();
        List<Account> accounts = new();
        try
        {
            await conn.OpenAsync();
            accounts = await repository.GetAccounts(customerId);
        }
        finally
        {
            await conn.CloseAsync();
        }
        return View(accounts);
    }
}