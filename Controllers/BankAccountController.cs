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
    public async Task<IActionResult> AddSavingsAccount(int customerId)
    {
        var nic = TempData["nic"] as string;
        
        if(customerId == -1)
            return RedirectToAction("ManageIndividuals", "Customers");
        
        var savingsPlans 
            = await _bankAccountService.GetSavingsPlansForCustomer(customerId);
        
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
            return View(model);
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", e.Message);
            return View(model);
        }

        return RedirectToAction("ManageIndividuals", "Customers");
    }
    
    [HttpPost]
    public IActionResult AddCurrentAccount(AddBankAccountViewModel model)
    {
        return View(model);
    }
}