using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

/// <summary>
/// Handles all individual customer related operations.
/// </summary>
public class IndividualsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IIndividualRepository _individualRepository;

    public IndividualsController(AppDbContext context,
        IIndividualRepository individualRepository)
    {
        _context = context;
        _individualRepository = individualRepository;
    }
    
    [HttpGet]
    public IActionResult AddNewIndividual()
    {
        var nic = TempData["nic"] as string;
        bool checkForChild = TempData["checkForChild"] as bool? ?? false;
        
        if (nic == null)
        {
            return RedirectToAction("ManageIndividuals", "Customers");
        }
        var model = new IndividualViewModel()
        {
            NIC = nic
        };
        TempData["nic"] = nic;
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddNewIndividual(IndividualViewModel model)
    {
        var conn = _context.GetConnection();
        int customerId = -1;
        if(model.NIC == null)
            model.NIC = TempData["nic"] as string;

        try
        {
            await conn.OpenAsync();
            customerId = await _individualRepository.AddIndividual(model);
        }
        finally
        {
            await conn.CloseAsync();
        }

        if (customerId != -1)
        {
            TempData["customerId"] = customerId;
            TempData["nic"] = model.NIC;
            return RedirectToAction("AddNewBankAccount", 
                "BankAccount");
        }
            
        
        return View(model);
    }
}