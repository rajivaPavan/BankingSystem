using BankingSystem.DBContext;
using BankingSystem.DbOperations;
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
    private readonly IOrganizationRepository _organizationRepository;

    public CustomersController(AppDbContext context, 
        IIndividualRepository individualRepository,
        IOrganizationRepository organizationRepository)
    {
        _context = context;
        _individualRepository = individualRepository;
        _organizationRepository = organizationRepository;
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

    [HttpGet]
    public IActionResult ManageOrganizations()
    {
        OrganizationSearchViewModel model = new OrganizationSearchViewModel();
        model.Found = null;
        model.Result = null;
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> ManageOrganizations(OrganizationSearchViewModel model)
    {
        
        // Implement the logic to search for organization customers and populate the model.
        var res = await _organizationRepository.GetOrganization(model.RegNo);
        
        if (res is not null) // Replace with your actual logic
        {
            var individuals =
                await _organizationRepository.GetOrganizationIndividuals(model.RegNo);
            res.Owners = individuals;
            model.Found = true;
            model.Result = res;
        }
        else
        {
            model.Found = false;
            TempData["regNo"] = model.RegNo;
        }
        
        return View(model);
    }
}