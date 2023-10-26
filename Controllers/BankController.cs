using System.Security.Claims;
using BankingSystem.Constants;
using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.Models;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

[Authorize(policy:Policies.NonCustomerPolicy)]
public class BankController : Controller
{
}