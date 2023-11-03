using System.Security.Claims;
using BankingSystem.Constants;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using BankingSystem.Services;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace BankingSystAem.Controllers;

public class EmployeesController : Controller
{
    private readonly Emp_UserService _employeeService;
    private readonly IEmployeeRepository _employeeRepository;


    [HttpGet]
    public async Task<IActionResult> EmployeeSelfRegister()
    {
        return View("EmployeeSelfRegister");
    }

    [HttpPost]
    public async Task<IActionResult> EmployeeSelfRegister(SelfRegistrationEmployeeViewModel model)
    {

        // store nic, bank account number in temp data
        TempData["nic"] = model.NIC;     

        // validate individual using nic and bank account number
        var hasUserAccount = await _employeeService.EmployeeHasUserAccount(model.NIC);

        if (hasUserAccount == true)
        {
            ModelState.AddModelError("", "This Employee already has an account.");
            return View(model);
        }

        // generate and save otp in temp data
        var otp = new Random().Next(100000, 999999).ToString();
        TempData["otp"] = otp;

        return RedirectToAction("EmployeeFinalizeSelfRegister");
    }

    [HttpGet]
    public async Task<IActionResult> EmployeeFinalizeSelfRegister()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> EmployeeFinalizeSelfRegister(FinalizeSelfRegisterEmployeeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Invalid");
            return View(model);
        }

        var nic = TempData["nic"] as string;
        var bankAccountNumber = TempData["bankAccountNumber"] as string;
        var username = model.Username;
        var password = model.Password;
        var otp = model.OTP;

        // validate otp
        var tempOtp = TempData["otp"] as string;
        if (tempOtp != otp || tempOtp == null)
        {
            ModelState.AddModelError("", "Invalid OTP. Please try again.");
            return View(model);
        }

        var user = new User()
        {
            UserName = username,
            UserType = UserType.Employee
        };

        // register user
        await _employeeService.RegisterUser(user, password);

        return RedirectToAction("Login");
    }

    [Authorize(Policy = Policies.EmployeePolicy)]
    [HttpGet]
    public IActionResult CreateEmployee()
    {
        var model = new EmployeeViewModel();
        return View("EmployeeForm");
    }

    [Authorize(Policy = Policies.EmployeePolicy)]
    [HttpPost]
    public IActionResult CreateEmployee(EmployeeViewModel model)
    {
        var branchId = int.Parse(HttpContext.User.FindFirst("BranchId")!.Value);
        var employee = new BankingSystem.ViewModels.Employee
        {
            BranchId = branchId,         /* Fetch the BranchId for the current user */
            FirstName = model.FirstName,
            LastName = model.LastName,
            DateOfBirth = model.DateOfBirth,
            Gender = model.Gender,
            NIC = model.NIC,
            Email = model.Email,
            Phone = "0775405059"
        };

        _employeeRepository.AddEmployee(employee);

        return RedirectToAction("Index"); // Redirect to the desired action after inserting the employee.

    }
}