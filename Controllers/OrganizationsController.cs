using BankingSystem.DbOperations;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class OrganizationsController : Controller
{
    private readonly IOrganizationRepository _organizationRepository;

    public OrganizationsController(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository;
    }
    
    [HttpGet]
    public IActionResult ViewOrganization()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AddNewOrganization()
    {
        var regNo = TempData["regNo"] as string;
        if (regNo == null)
        {
            return RedirectToAction("ManageOrganizations", "Customers");
        }
        
        var model = new CreateOrganizationViewModel()
        {
            RegNo = regNo
        };
        
        TempData["regNo"] = regNo;
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddNewOrganization(CreateOrganizationViewModel model)
    {
        int customerId = -1;
        if(model.RegNo == null)
            model.RegNo = TempData["regNo"] as string;

        try
        {
            customerId = await _organizationRepository.AddOrganization(model);
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return RedirectToAction("ManageOrganizations", "Customers");
        }
        
        if (customerId != -1)
        {
            TempData["customerId"] = customerId;
            TempData["regNo"] = model.RegNo;
            return RedirectToAction("AddNewOwner");
        }
        
        return View(model);
    }

    [HttpGet]
    public IActionResult AddNewOwner()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddNewOwner(OrganizationIndividualViewModel model)
    {
        
        return View(model);
    }
    
    
}