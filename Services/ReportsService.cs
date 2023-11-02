using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.ViewModels;

namespace BankingSystem.Services;

public interface IReportsService
{
    public Task<IEnumerable<BranchReportViewModel>> GetBranchReports();
    Task<IEnumerable<LoanReportViewModel>> GetLoanReports();   
}

public class ReportsService : IReportsService
{
    private readonly AppDbContext _context;
    private readonly IReportsRepository _reportsRepository;

    public ReportsService(AppDbContext context, IReportsRepository reportsRepository)
    {
        _context = context;
        _reportsRepository = reportsRepository;
    }
    
    public async Task<IEnumerable<BranchReportViewModel>> GetBranchReports()
    {
        var conn = _context.GetConnection();
        IEnumerable<BranchReportViewModel> branchReports;
        try
        {
            await conn.OpenAsync();
            branchReports = await _reportsRepository.GetBranchReports();

        }
        finally
        {
            await conn.CloseAsync();
        }

        return branchReports;
    }

    public async Task<IEnumerable<LoanReportViewModel>> GetLoanReports()
    {
        var conn = _context.GetConnection();
        IEnumerable<LoanReportViewModel> loanReports;
        try
        {
            await conn.OpenAsync();
            loanReports = await _reportsRepository.GetLoanReports();

        }
        finally
        {
            await conn.CloseAsync();
        }

        return loanReports;
    }
}