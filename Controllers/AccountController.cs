using BankingSystem.DbOperations;
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
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // Use the injected IUserRepository to check if the user exists
        var user = _authService.Login(model.Username, model.Password);

        return RedirectToAction("Index", "Home");
    }
}