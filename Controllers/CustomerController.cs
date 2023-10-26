using BankingSystem.Constants;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

[Authorize(policy: Policies.CustomerPolicy)]
public class CustomerController : Controller
{
    public CustomerController()
    {
        
    }
}