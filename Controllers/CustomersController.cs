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
    
    public IActionResult ManageIndividuals()
    {
        IndividualSearchViewModel model = new();
        model.Found = false;
        model.Result = null;
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> ManageIndividuals(IndividualSearchViewModel model)
    {
        // validate if customer with nic exists
        List<IndividualViewModel> individuals = new();
        try
        {
            await _context.GetConnection().OpenAsync();
            if (!model.CheckForChild)
            {
                var individual = await _individualRepository
                    .GetIndividualInfoForEmployee(model.Nic);
                if (individual != null)
                {
                    if (individual.BankAccountNumber == null)
                    {
                        TempData["customerId"] = individual.CustomerId;
                        TempData["nic"] = model.Nic;
                    }
                    
                    individuals.Add(individual);
                }
            }
            else
            {
                var res = await _individualRepository
                    .GetChildIndividualsByNicAsync(model.Nic);
                individuals.AddRange(res);
            }

        }
        finally
        {
            await _context.GetConnection().CloseAsync();
        }
        
        if (individuals.Count == 0)
        {
            ModelState.AddModelError("NotFound", "Customer with this NIC does not exist.");
            model.Found = false;
            model.Result = individuals;
            TempData["nic"] = model.Nic;
            TempData["checkForChild"] = model.CheckForChild;
            return View("ManageIndividuals", model);
        }
        
        model.Found = true;
        model.Result = individuals;
        
        return View("ManageIndividuals", model);
    }

    public IActionResult ManageOrganizations()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> SearchOrganization(string nic)
    {
        // validate if customer with nic exists
        await _context.GetConnection().OpenAsync();
        var individual = await _individualRepository.GetIndividualInfoForEmployee(nic);
        await _context.GetConnection().CloseAsync();
        
        
        if (individual != null)
        {
            ModelState.AddModelError("nic", "Customer with this NIC already exists.");
            return View("ManageIndividuals");
        }
                
        return RedirectToAction("AddNewIndividual", "Individuals", new {nic});
    }
}