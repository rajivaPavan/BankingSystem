using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class CustomerController : Controller
{
    public CustomerController()
    {
        
    }
    
    /// <summary>
    /// Customer Dashboard
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("/dashboard")]
    public IActionResult Index()
    {
        return View();
    }
}