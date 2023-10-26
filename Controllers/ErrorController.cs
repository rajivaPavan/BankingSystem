using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class ErrorController : Controller
{
    [Route("/error")]
    public IActionResult Error()
    {
        // get the error status code
        var statusCode = HttpContext.Response.StatusCode;
        // get the error message
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}