using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

/// <summary>
/// Handles all customer(individual and organization) related common operations.
/// </summary>
public class CustomersController : Controller
{
    private readonly AppDbContext _context;
    private readonly IIndividualRepository _individualRepository;

    public CustomersController(AppDbContext context, IIndividualRepository individualRepository)
    {
        _context = context;
        _individualRepository = individualRepository;
    }
    
    public IActionResult Index()
    {
        CustomerSearchViewModel model = new();
        model.Found = false;
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> SearchIndividual(CustomerSearchViewModel model)
    {
        // validate if customer with nic exists
        Individual? individual;
        try
        {
            await _context.GetConnection().OpenAsync();
            individual = await _individualRepository
                .GetByNicAsync(model.Search ? model.SearchNic! : model.ValidateNic!);
        }
        finally
        {
            await _context.GetConnection().CloseAsync();
        }
        
        if (model.Search)
        {
            // check if nic is valid
            if(model.SearchNic!.Length != 12)
            {
                ModelState.AddModelError("SearchNic", "Invalid NIC.");
                return View("Index", model);
            }
            
            if(individual == null)
            {
                ModelState.AddModelError("SearchNic", "Customer with this NIC does not exist.");
                return View("Index", model);
            }

            model.Found = true;
            model.IndividualId = individual.IndividualId;
            return View("Index", model);
        }
        
        // check if nic is valid
        if(model.ValidateNic.Length != 12)
        {
            ModelState.AddModelError("ValidateNic", "Invalid NIC.");
            return View("Index", model);
        }
        
        if(individual != null)
        {
            ModelState.AddModelError("ValidateNic", "Customer with this NIC already exists.");
            return View("Index", model);
        }
        
        
            
        return RedirectToAction("AddNewIndividual", "Individuals", new {nic=model.ValidateNic});
    }
    
    [HttpPost]
    public async Task<IActionResult> SearchOrganization(string nic)
    {
        // validate if customer with nic exists
        await _context.GetConnection().OpenAsync();
        var individual = await _individualRepository.GetByNicAsync(nic);
        await _context.GetConnection().CloseAsync();
        
        
        if (individual != null)
        {
            ModelState.AddModelError("nic", "Customer with this NIC already exists.");
            return View("Index");
        }
                
        return RedirectToAction("AddNewIndividual", "Individuals", new {nic});
    }

    public async Task<IActionResult> ViewIndividual(int individualId)
    {
        // validate if customer with nic exists
        Individual? individual;
        try
        {
            await _context.GetConnection().OpenAsync();
            individual = await _individualRepository.GetByIdAsync(individualId);
        }
        finally
        {
            await _context.GetConnection().CloseAsync();
        }
        
        if (individual == null)
        {
            return View("Index");
        }
        
        return View(individual);
    }
}