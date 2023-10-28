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
    private readonly IPasswordService _passwordService;

    public AccountController(IAuthenticationService authService, IUserService userService,
    IPasswordService passwordService)
    {
        _authService = authService;
        _userService = userService;
        _passwordService = passwordService;
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
        var hash = _passwordService.HashPassword(model.Password);
        var success = await _authService.Login(model.Username, hash);

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
        // validate individual using nic and bank account number
        var individualId = await _userService.IndividualHasUserAccount(model.NIC, model.BankAccountNumber);
        
        if (individualId == -1)
        {
            ModelState.AddModelError("", "User already has an account.");
            return View(model);
        }
        
        // generate and save otp in temp data
        var otp = new Random().Next(100000, 999999).ToString();
        TempData["otp"] = otp;
        TempData["otpExpiry"] = DateTime.Now.AddMinutes(10).ToString("g");
        TempData["individualId"] = individualId;
        
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
        
        var individualId = (TempData["individualId"] as int?);
        var otp = model.OTP;
        if (individualId == null)
        {
            ModelState.AddModelError("","Error Occurred. Try again");
            return RedirectToAction("CustomerSelfRegister");
        }
        
        var username = model.Username;
        var password = model.Password;
        
        // validate otp
        var tempOtp = TempData["otp"] as string;
        // check if tempOtp expired or not
        var tempOtpExpiry = DateTime.Parse(TempData["otpExpiry"] as string ?? string.Empty);
        if (tempOtpExpiry < DateTime.Now)
        {
            ModelState.AddModelError("", "OTP has expired");
            return RedirectToAction("CustomerSelfRegister");
        }

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
        password = _passwordService.HashPassword(password);
        // register user
        await _userService.RegisterIndividualUser(user, password, (int)individualId);
        
        return RedirectToAction("Login");
    }

    [HttpGet]
    [Route("/otpdemo")]
    public IActionResult OtpDemo()
    {
        var otp = TempData["otp"] as string;
        TempData["otp"] = otp;
        return Json(new {otp});
    }
    
    
}