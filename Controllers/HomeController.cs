using BankingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

[Authorize]
public class HomeController : Controller
{
    [Route("/dashboard")]
    public IActionResult Index()
    {
        // return view based on user role
        if (User.IsInRole(UserType.Customer.ToString()))
            return RedirectToAction("AccountSummary", "CustomerHome");

        return View("BankDashboard");
    }
}