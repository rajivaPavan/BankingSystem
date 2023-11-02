using BankingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using BankingSystem.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BankingSystem.Controllers
{
    public class LoanProductsController : Controller
    {
        private readonly AppDbContext _context;
        public LoanProductsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // SQL query to fetch loan products from the database
            string sqlQuery = "SELECT * FROM LoanProduct";

            // Execute the SQL query using the DbContext.Database object
            var loanProducts = _context.LoanProduct.FromSqlRaw(sqlQuery).ToList();

            return View(loanProducts);
        }


    }

    
   

    public class LoanRepaymentsController : Controller
    {
        public IActionResult Create(int loanApplicationId)
        {
            // Logic to fetch loan application details based on the id
            var loanApplication = _context.LoanApplications.Find(loanApplicationId);

            ViewBag.LoanApplication = loanApplication;

            return View();
        }

        [HttpPost]
        public IActionResult Create(LoanRepayment loanRepayment)
        {
            if (ModelState.IsValid)
            {
                // Logic to save the loan repayment to the database
                _context.LoanRepayments.Add(loanRepayment);
                _context.SaveChanges();

                return RedirectToAction("Index", "LoanRepayments");
            }

            // If model state is not valid, redisplay the form with validation errors
            var loanApplication = _context.LoanApplications.Find(loanRepayment.LoanApplicationId);

            ViewBag.LoanApplication = loanApplication;

            return View(loanRepayment);
        }

    }

    [Authorize]
    public class CustomerLoanController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<Customer> _userManager;

        public CustomerLoanController(AppDbContext dbContext, UserManager<Customer> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Get the currently logged-in customer
            var customer = await _userManager.GetUserAsync(User);

            if (customer == null)
            {
                return NotFound();
            }

            // Retrieve the loan details specific to the customer
            var loans = _dbContext.Loans.Where(l => l.CustomerId == customer.Id).ToList();

            // Pass the loan details to the view
            var model = new CustomerLoanViewModel
            {
                Customer = customer,
                Loans = loans
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyLoan(LoanApplicationModel model)
        {
            // Retrieve the currently logged-in customer
            var customer = await _userManager.GetUserAsync(User);
            // Retrieve the FD details
            var existingFdAmount = customer.ExistingFdAmount;
            var savingsAccount = customer.SavingsAccount;

            // Check if the requested loan amount is within the allowable limits
            var maxLoanAmount = Math.Min(existingFdAmount * 0.6m, 500000m);
            if (model.RequestedAmount <= 0 || model.RequestedAmount > maxLoanAmount)
            {
                ModelState.AddModelError(string.Empty, "Invalid loan amount");
                return View("Index");
            }

            // Create a loan record and calculate the loan installment
            var loan = new Loan
            {
                CustomerId = customer.Id,
                Amount = model.RequestedAmount,
                LoanType = LoanType.Online
            };

            // Add the loan to the database
            _dbContext.Loans.Add(loan);
            await _dbContext.SaveChangesAsync();

            // Deposit the loan amount to the savings account bound to the FD
            savingsAccount.Balance += model.RequestedAmount;
            _dbContext.SaveChanges();

            // Redirect back to the customer index page with a success message
            TempData["LoanApplicationSuccess"] = "Loan application submitted successfully";
            return RedirectToAction("Index");
        }

    }
}
