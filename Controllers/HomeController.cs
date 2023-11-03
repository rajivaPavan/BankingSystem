using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly IBankRepository _repository;
    
    public HomeController(AppDbContext context, IBankRepository repository)
    {
        _context = context;
        _repository = repository;
    }
    
    [Route("/dashboard")]
    public async Task<IActionResult> Index()
    {
        // return view based on user role
        if (User.IsInRole(UserType.Customer.ToString()))
        {
            var customerId = int.Parse(HttpContext.User.FindFirst("CustomerId")!.Value);
            var conn = _context.GetConnection();
            List<Account> accounts = new();
            try
            {
                await conn.OpenAsync();
                accounts = await _repository.GetAccounts(customerId);
            }
            finally
            {
                await conn.CloseAsync();
            }
            
            return View("CustomerDashboard", accounts);
        }

        return View("BankDashboard");
    }
}