using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class BankController : Controller
{
    private readonly AppDbContext _context;
    private readonly IIndividualRepository _individualRepository;

    public BankController(AppDbContext context,
        IIndividualRepository individualRepository)
    {
        _context = context;
        _individualRepository = individualRepository;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    /// <summary>
    /// Returns a List all individual customers in the database and
    /// provide a link in the view to create a new individual customer.
    /// </summary>
    [Route("[controller]/customers/individuals")]
    public async Task<IActionResult> Individuals()
    {
        await _context.GetConnection().OpenAsync();
        var all = await _individualRepository.GetAllAsync();
        await _context.GetConnection().CloseAsync();
        
        IndividualsViewModel model = new()
        {
            Individuals = all
        };
        return View(model);
    }
    
    /// <summary>
    /// Returns a View to create a new individual customer by filling out the form in this view.
    /// </summary>
    [HttpGet]
    [Route("[controller]/customers/individuals/{nic}")]
    public IActionResult Individual(string nic)
    {
        // validate nic
        // check if nic already exists in the database
        // if nic already exists, redirect to view individual customer page
        // if nic does not exist, redirect to create individual customer page
        return View("CreateIndividual", nic);
    }
    
    [HttpPost]
    public IActionResult CreateIndividual()
    {
        
        return View();
    }
    
}