using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class IndividualsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IIndividualRepository _individualRepository;

    public IndividualsController(AppDbContext context,
        IIndividualRepository individualRepository)
    {
        _context = context;
        _individualRepository = individualRepository;
    }
    
    /// <summary>
    /// Returns a List all individual customers in the database and
    /// provide a link in the view to create a new individual customer.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
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

    [HttpPost]
    public async Task<IActionResult> Index(string nic)
    {
        // validate if customer with nic exists
        await _context.GetConnection().OpenAsync();
        var individual = await _individualRepository.GetByNicAsync(nic);
        await _context.GetConnection().CloseAsync();

        if (individual != null)
        {
            return RedirectToAction("Index");
        }
        
        return RedirectToAction("AddNewIndividual", new {nic});
    }    
    
    [HttpGet]
    public IActionResult AddNewIndividual(string? nic)
    {
        if (nic == null)
        {
            return RedirectToAction("Index");
        }
        return View("AddNewIndividual", nic);
    }
}