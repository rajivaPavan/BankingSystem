using System.Text;
using BankingSystem.DBContext;
using BankingSystem.DbOperations;
using BankingSystem.ViewModels;

namespace BankingSystem.Services;

public interface IBankAccountService
{
    public Task AddSavingsAccount(AddBankAccountViewModel model);
    public Task<IEnumerable<SavingsPlanViewModel>> GetSavingsPlans();

}

public class BankAccountService : IBankAccountService
{
    private readonly AppDbContext _context;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BankAccountService(AppDbContext context, IBankAccountRepository bankAccountRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _bankAccountRepository = bankAccountRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AddSavingsAccount(AddBankAccountViewModel model)
    {
        var conn = _context.GetConnection();
        try
        {
            await conn.OpenAsync();
            var branchId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst("BranchId")!.Value);
            // check if customer already has a savings account in the same branch
            var hasSavingsAccount = await _bankAccountRepository
                .HasAccountInBranch(model.CustomerId, branchId, BankAccountType.Savings);

            if (hasSavingsAccount)
                throw new Exception("Customer already has a savings account in this branch");

            var accNo = GenerateBankAccountNumber();
            await _bankAccountRepository.AddSavingsAccount(model, accNo, branchId);
        }
        finally
        {
            await conn.CloseAsync();
        }
    }

    public async Task<IEnumerable<SavingsPlanViewModel>> GetSavingsPlans()
    {
        var conn = _context.GetConnection();
        IEnumerable<SavingsPlanViewModel> savingsPlans;
        try
        {
            await conn.OpenAsync();
            savingsPlans = await _bankAccountRepository.GetSavingsPlans();
        }
        finally
        {
            await conn.CloseAsync();
        }

        return savingsPlans;
    }

    /// <summary>
    /// Returns the next 16 character length numeric string based on 
    /// </summary>
    private static string GenerateBankAccountNumber(int length = 16)
    {
        const string validChars = "0123456789";
        var random = new Random();
        var accountNumber = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            accountNumber.Append(validChars[random.Next(0, validChars.Length)]);
        }

        return accountNumber.ToString();

    }
}