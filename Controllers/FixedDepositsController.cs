using BankingSystem.Constants;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
    
    [Authorize(Policies.EmployeePolicy)]
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

    [Authorize(Policies.EmployeePolicy)]
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

    [Authorize(Policies.EmployeePolicy)]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> ViewFixedDeposits()
    {
        List<FixedDepositViewModel>? fixedDeposits = new();
        if(User.IsInRole(UserType.Employee.ToString()) || User.IsInRole(UserType.Manager.ToString()))
            fixedDeposits = await _fixedDepositsRepository.GetFixedDeposits();
        else if (User.IsInRole(UserType.Customer.ToString()))
        {
            var customerId = int.Parse(HttpContext.User.FindFirst("CustomerId")!.Value);
            fixedDeposits = await _fixedDepositsRepository.GetFixedDeposits(customerId);
        }
            
        ViewFixedDepositsViewModel model = new()
        {
            FixedDeposits = fixedDeposits
        };
        return View(model);
    }
    
}