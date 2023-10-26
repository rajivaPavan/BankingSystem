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
        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> CustomerFinalizeSelfRegister()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CustomerFinalizeSelfRegister(FinalizeSelfRegisterViewModel model)
    {
        return View(model);
    }
    
    
}