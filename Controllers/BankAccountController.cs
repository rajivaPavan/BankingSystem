using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Services;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlConnector;

namespace BankingSystem.Controllers;

public class BankAccountController : Controller
{
    private readonly AppDbContext _context;
    private readonly IBankAccountService _bankAccountService;

    public BankAccountController(AppDbContext context,
        IBankAccountService bankAccountService)
    {
        _context = context;
        _bankAccountService = bankAccountService;
    }

    [HttpGet]
    public async Task<IActionResult> AddNewBankAccount(int customerId, string nic)
    {
        var savingsPlans
            = await _bankAccountService.GetSavingsPlans();

        var model = new AddBankAccountViewModel()
        {
            CustomerId = customerId,
            NicOrRegNo = nic,
            SavingsPlans = new SelectList(savingsPlans, "SavingsPlanId", "Name"),
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddSavingsAccount(AddBankAccountViewModel model)
    {
        try
        {
            await _bankAccountService.AddSavingsAccount(model);
        }
        catch (MySqlException e)
        {
            ModelState.AddModelError("", "Something went wrong. Please try again later.");
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", e.Message);
        }

        return RedirectToAction("Index", "Customers");
    }

    [HttpPost]
    public IActionResult AddCurrentAccount(AddBankAccountViewModel model)
    {
        return View("AddNewBankAccount", model);
    }


    [HttpGet]
    public async Task<IActionResult> BranchReport()
    {
        var branchReports = await _bankAccountService.GetBranchReports();

        var branchReportViewModelList = branchReports.Select(report => new BranchReportViewModel
        {
            Incomes = report.Incomes.Select(income => new Income
            {
                AccountNumber = income.AccountNumber,
                BranchId = income.BranchId,
                OpeningDate = income.OpeningDate,
                Amount = income.Amount,
                TransactionType = TransactionType.Income
            }).ToList(),
            Outcomes = report.Outcomes.Select(outcome => new Outcome
            {
                AccountNumber = outcome.AccountNumber,
                BranchId = outcome.BranchId,
                OpeningDate = outcome.OpeningDate,
                Amount = outcome.Amount,
                TransactionType = TransactionType.Outgo
            }).ToList()
        }).ToList();

        return View("BranchReports", branchReportViewModelList);
    }

    
}