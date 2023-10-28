using System.Security.Claims;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using BankingSystem.Services;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class AccountController : Controller
{
    private readonly IAuthenticationService _authService;
    private readonly IUserService _userService;

    public AccountController(IAuthenticationService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        if (!HttpContext!.User.Identity!.IsAuthenticated)
            return View();
        
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
    {
        var success = await _authService.Login(model.Username, model.Password);

        if (success == false)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        // redirect user based on Roles Claim
        return RedirectToAction("Index", "Home");

    }
    
    public async Task<IActionResult> Logout()
    {
        if (HttpContext!.User.Identity!.IsAuthenticated)
        {
            await _authService.Logout();
            return RedirectToAction("Login");
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> CustomerSelfRegister()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CustomerSelfRegister(SelfRegistrationViewModel model)
    {
        
        // store nic, bank account number in temp data
        TempData["nic"] = model.NIC;
        TempData["bankAccountNumber"] = model.BankAccountNumber;
        
        // validate individual using nic and bank account number
        var hasUserAccount = await _userService.IndividualHasUserAccount(model.NIC, model.BankAccountNumber);
        
        if (hasUserAccount == true)
        {
            ModelState.AddModelError("", "User already has an account.");
            return View(model);
        }
        
        // generate and save otp in temp data
        var otp = new Random().Next(100000, 999999).ToString();
        TempData["otp"] = otp;
        
        return RedirectToAction("CustomerFinalizeSelfRegister");
    }
    
    [HttpGet]
    public async Task<IActionResult> CustomerFinalizeSelfRegister()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CustomerFinalizeSelfRegister(FinalizeSelfRegisterViewModel model)
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
            UserType = UserType.Customer
        };
        
        // register user
        await _userService.RegisterUser(user, password);
        
        return RedirectToAction("Login");
    }
    
    
}