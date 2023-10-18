using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.ApiControllers;

[ApiController]
public class BankApiController : Controller
{
    public BankApiController()
    {
        
    }

    [HttpGet]
    [Route("api/bank/customers/individuals/{nic}")]
    public async Task<IActionResult> GetCustomer(string nic)
    {
        // Query database for customer with that nic
        return Json(new { success = true, nic });
    }
    
    
}