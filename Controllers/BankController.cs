using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class BankController : Controller
{
    /// <summary>
    /// Dashboard for the bank employees and managers.
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }
}