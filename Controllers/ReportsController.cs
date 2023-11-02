using BankingSystem.Services;
using BankingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers;

public class ReportsController : Controller
{
    private readonly IReportsService _reportsService;

    public ReportsController(IReportsService reportsService)
    {
        _reportsService = reportsService;
    }
    
    [HttpGet]
    public async Task<IActionResult> BranchReport()
    {
        var branchReports = await _reportsService.GetBranchReports();

        var branchReportViewModelList = branchReports.Select(report => new BranchReportViewModel
        {
            Incomes = report.Incomes.Select(income => new Income
            {
                AccountNumber = income.AccountNumber,
                BranchId = income.BranchId,
                OpeningDate = income.OpeningDate,
                Amount = income.Amount,
                TransactionType = TransactionType.Income
            }).ToList(),
            Outcomes = report.Outcomes.Select(outcome => new Outcome
            {
                AccountNumber = outcome.AccountNumber,
                BranchId = outcome.BranchId,
                OpeningDate = outcome.OpeningDate,
                Amount = outcome.Amount,
                TransactionType = TransactionType.Outgo
            }).ToList()
        }).ToList();

        return View("BranchReports", branchReportViewModelList);
    }
    
    [HttpGet]
    public async Task<IActionResult> LoanReport()
    {
        var loanReports = await _reportsService.GetLoanReports();

        var loanReportViewModelList = loanReports.Select(report => new LoanReportViewModel
        {
            loan = report.loan.Select(loan => new LoanInstallments
            {
                Customer_id = loan.Customer_id,
                NIC = loan.NIC,
                Name = loan.Name,
                Loan_id = loan.Loan_id,
                BranchId = loan.BranchId,
                Last_unpaid_date = loan.Last_unpaid_date,
                No_of_missing_installments = loan.No_of_missing_installments,
                Interest = loan.Interest,
                Loan_amount = loan.Loan_amount,
                Missing_amount = loan.Missing_amount,
            }).ToList(),
        }).ToList();

        return View("LoanReports", loanReportViewModelList);
    }
}