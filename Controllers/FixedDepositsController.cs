using BankingSystem.DbOperations;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MySqlConnector;

namespace BankingSystem.Controllers;

public class FixedDepositsController : Controller
{
    private readonly IFixedDepositsRepository _fixedDepositsRepository;

    public FixedDepositsController(IFixedDepositsRepository fixedDepositsRepository)
    {
        _fixedDepositsRepository = fixedDepositsRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> AddFixedDeposit()
    {
        var fdPlans = await _fixedDepositsRepository.GetFdPlansAsync();
        var addFixedDepositViewModel = new AddFixedDepositViewModel
        {
            FdPlans = fdPlans,
        };
        return View(addFixedDepositViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddFixedDeposit(AddFixedDepositViewModel addFixedDepositViewModel)
    {
        if (addFixedDepositViewModel.FdPlanId == 0)
        {
            ModelState.AddModelError("", "Please select a Fixed Deposit Plan.");
            var fdPlans = await _fixedDepositsRepository.GetFdPlansAsync();
            addFixedDepositViewModel.FdPlans = fdPlans;
            return View("AddFixedDeposit", addFixedDepositViewModel);
        }
        if (!ModelState.IsValid)
        {
            var fdPlans = await _fixedDepositsRepository.GetFdPlansAsync();
            addFixedDepositViewModel.FdPlans = fdPlans;
            // print all model validation errors
            return View(addFixedDepositViewModel);
        }

        try
        {
            await _fixedDepositsRepository.AddFixedDepositAsync(addFixedDepositViewModel);
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", e.Message);
            
            var fdPlans = await _fixedDepositsRepository.GetFdPlansAsync();
            addFixedDepositViewModel.FdPlans = fdPlans;
            return View(addFixedDepositViewModel);
        }
        return Json(new {success=true});
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> ViewFixedDeposits()
    {
        var fixedDeposits = await _fixedDepositsRepository.GetFixedDeposits();
        ViewFixedDepositsViewModel model = new()
        {
            FixedDeposits = fixedDeposits
        };
        return View(model);
    }
}