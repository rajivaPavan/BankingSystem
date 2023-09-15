using BankingSystem.DbOperations;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class AccountController : Controller
{
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountRepository _accountRepository)
    {
        this._accountRepository = _accountRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // Use the injected IAccountRepository to check if the user exists
        var user = _accountRepository.Login(model.Username, model.Password);

        return RedirectToAction("Index", "Home");
    }
}