using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlConnector;

namespace BankingSystem.Controllers;

public class TransactionController : Controller
{
    private readonly AppDbContext _context;
    private readonly IIndividualRepository _individualRepository;
    public TransactionController(AppDbContext context, IIndividualRepository individualRepository)
    {
        _context = context;
        _individualRepository = individualRepository;
    }

    public IActionResult Index()
    {
        TransferViewModel model = new();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Transfer(int recipientAccountId, double amount, int senderId,string Reference)
    {
        var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT account_number FROM bank_accounts WHERE customer_id = @customer_id;";
        cmd.Parameters.AddWithValue("@customer_id", senderId);
        using var reader = await cmd.ExecuteReaderAsync();
        var Accounts = new List<AccountViewModel>();
        while (reader.Read())
        {
            var Account = new AccountViewModel()
            {
                Accounts_id = reader.GetString("account_no"),
            };

            Accounts.Add(Account);
        }

        var model = new TransferViewModel();
        {
            model.RecipientAccountId = recipientAccountId;
            model.Amount = amount;
            model.SenderAccountId = senderId;
            model.SenderAccounts = new SelectList(Accounts, "Id", "AccountNumber");
            model.Reference= Reference;
        }
        return View(model);
    }

    [HttpPost]
    public async Task Addtransfer(TransferViewModel model)
    {
        var conn = _context.GetConnection();
        try
        {
            await conn.OpenAsync();
            await _individualRepository.Addtransfer(model);
        }
        finally
        {
            await conn.CloseAsync();
        }
    }

}

