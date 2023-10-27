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
    public IActionResult AddNewIndividual(string? nic)
    {
        if (nic == null)
        {
            return RedirectToAction("Index", "Customers");
        }
        var model = new IndividualViewModel()
        {
            NIC = nic
        };
        return View("AddNewIndividual", model);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddNewIndividual(IndividualViewModel model)
    {
        var conn = _context.GetConnection();
        int customerId = -1;
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
            return RedirectToAction("AddNewBankAccount", 
                "BankAccount", 
                new {customerId, nic = model.NIC});
        
        return View(model);
    }
}