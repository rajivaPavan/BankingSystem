﻿using BankingSystem.DbOperations;
using BankingSystem.Services;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class AccountController : Controller
{
    private readonly IAuthenticationService _authService;

    public AccountController(IAuthenticationService authService)
    {
        this._authService = authService;
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
    {
        var user = await _authService.Login(model.Username, model.Password);

        if (user == false)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        return RedirectToAction("Index", "Home");

    }
    
    public async Task<IActionResult> Logout()
    {
        await _authService.Logout();
        return RedirectToAction("Index", "Home");
    }
}