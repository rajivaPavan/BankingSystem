using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankingSystem.Controllers;

public class BankAccountController : Controller
{
    private readonly AppDbContext _context;
    private readonly IBankAccountRepository _bankAccountRepository;

    public BankAccountController(AppDbContext context, IBankAccountRepository bankAccountRepository)
    {
        _context = context;
        _bankAccountRepository = bankAccountRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> AddNewBankAccount(int customerId, string nic)
    {
        var conn = _context.GetConnection();
        IEnumerable<SavingsPlanViewModel> savingsPlans;
        try
        {
            await conn.OpenAsync();
            savingsPlans = await _bankAccountRepository.GetSavingsPlans();
        }
        finally
        {
            await conn.CloseAsync();
        }
        
        var model = new AddBankAccountViewModel()
        {
            CustomerId = customerId,
            NicOrRegNo = nic,
            SavingsPlans = new SelectList(savingsPlans, "SavingsPlanId", "Name"),
        };
        
        return View(model);
    }
    
    [HttpPost]
    public IActionResult AddSavingsAccount(AddBankAccountViewModel model)
    {
        return View("AddNewBankAccount", model);
    }
    
    [HttpPost]
    public IActionResult AddCurrentAccount(AddBankAccountViewModel model)
    {
        return View("AddNewBankAccount", model);
    }
}