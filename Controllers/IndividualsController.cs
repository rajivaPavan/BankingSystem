using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

/// <summary>
/// Handles all individual customer related operations.
/// </summary>
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
    
    [HttpGet]
    public IActionResult AddNewIndividual(string? nic)
    {
        if (nic == null)
        {
            return RedirectToAction("Index", "Customers");
        }
        return View("AddNewIndividual", nic);
    }
}