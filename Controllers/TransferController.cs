using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlConnector;

namespace BankingSystem.Controllers;

public class TransferController : Controller
{
    private readonly AppDbContext _context;
    private readonly ITransferRepository _transfersRepository;
    public TransferController(AppDbContext context, ITransferRepository transfersRepository)
    {
        _context = context;
        _transfersRepository = transfersRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int recipientAccountId, double amount)
    {
        var customerId = HttpContext.User.FindFirst("CustomerId")!.Value;
        var conn = _context.GetConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT account_no FROM bank_account 
            WHERE customer_id = @customer_id and account_type = 1;";
        cmd.Parameters.AddWithValue("@customer_id", customerId);
        using var reader = await cmd.ExecuteReaderAsync();
        var accounts = new List<AccountViewModel>();
        while (reader.Read())
        {
            var account = new AccountViewModel()
            {
                AccountNumber = reader.GetString("account_no"),
            };

            accounts.Add(account);
        }

        await conn.CloseAsync();
        var model = new TransferViewModel();
        {
            model.SenderAccounts = new SelectList(accounts, "AccountNumber", "AccountNumber");
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(TransferViewModel model)
    {
        var conn = _context.GetConnection();
        try
        {
            await conn.OpenAsync();
            await _transfersRepository.AddTransfer(model);
            ViewBag.Message = "Transfer successful";
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", "Transfer failed");
        }
        finally
        {
            await conn.CloseAsync();
        }
        
        return View(model);
    }

}

