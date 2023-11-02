using BankingSystem.Constants;
using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BankingSystem.Controllers;

public class TransactionsController: Controller
{
    private readonly AppDbContext _context;
    private readonly ITransactionsRepository _transactionsRepository;



    public TransactionsController(AppDbContext context, ITransactionsRepository transactionsRepository)
    {
        _context = context;
        _transactionsRepository = transactionsRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Withdrawal()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Withdrawal(TransactionsViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var conn = _context.GetConnection();
        try
        {
            await conn.OpenAsync();
            await _transactionsRepository.AddTransaction(model, TransactionType.Withdrawal);
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", e.Message);
            await conn.CloseAsync();
            return View(model);
        }
        finally
        {
            await conn.CloseAsync();
        }
        
        return RedirectToAction("Withdrawal");
    }

    [HttpGet]
    public async Task<IActionResult> Deposit()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Deposit(TransactionsViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        
        var conn = _context.GetConnection();
        try
        {
            await conn.OpenAsync();
            await _transactionsRepository.AddTransaction(model, TransactionType.Deposit);
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", e.Message);
            await conn.CloseAsync();
            return View(model);
        }
        finally
        {
            await conn.CloseAsync();
        }
        
        return RedirectToAction("Deposit");
    } 
}