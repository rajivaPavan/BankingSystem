using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Services;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
    public IActionResult AddSavingsAccount(AddBankAccountViewModel model)
    {
        _bankAccountService.AddSavingsAccount(model);
        
            
        return Json(new {success = true});
    }
    
    [HttpPost]
    public IActionResult AddCurrentAccount(AddBankAccountViewModel model)
    {
        return View("AddNewBankAccount", model);
    }
}