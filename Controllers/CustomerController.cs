using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class CustomerController : Controller
{
    public CustomerController()
    {
        
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Loan()
    {
        // database query
        var loans = new List<LoanViewModel>
        {
            new LoanViewModel
            {
                Id = "1",
                Name = "Home Loan",
                Description = "Home Loan Description"
            },
            new LoanViewModel
            {
                Id = "2",
                Name = "Car Loan",
                Description = "Car Loan Description"
            },
            new LoanViewModel
            {
                Id = "3",
                Name = "Personal Loan",
                Description = "Personal Loan Description"
            }
        };
        var allLoans = new AllLoansViewModel
        {
            Loans = loans
        };
        return View(allLoans);
    }
}