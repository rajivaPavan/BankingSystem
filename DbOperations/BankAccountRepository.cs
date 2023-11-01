using BankingSystem.DBContext;
using BankingSystem.ViewModels;

namespace BankingSystem.DbOperations;

public interface IBankAccountRepository
{
    Task<IEnumerable<SavingsPlanViewModel>> GetSavingsPlans();
    Task AddSavingsAccount(AddBankAccountViewModel model, string accNo, int branchId);
    Task<bool> HasAccountInBranch(int customerId, int branchId, BankAccountType type);
    Task<IEnumerable<BranchReportViewModel>> GetBranchReports();
    Task<IEnumerable<LoanReportViewModel>> GetLoanReports();
}

public class BankAccountRepository : IBankAccountRepository
{
    private readonly AppDbContext _context;

    public BankAccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SavingsPlanViewModel>> GetSavingsPlans()
    {
        var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM savings_plan";
        using var reader = await cmd.ExecuteReaderAsync();
        var savingsPlans = new List<SavingsPlanViewModel>();
        while (reader.Read())
        {
            var savingsPlan = new SavingsPlanViewModel()
            {
                SavingsPlanId = reader.GetInt32("savings_plan_id"),
                Name = reader.GetString("name"),
                InterestRate = reader.GetDouble("interest"),
                MinimumBalance = reader.GetInt16("minimum"),
                MaxWithdrawals = reader.GetInt32("max_withdrawals")
            };

            savingsPlans.Add(savingsPlan);
        }

        return savingsPlans;
    }

    public async Task<IEnumerable<BranchReportViewModel>> GetBranchReports()
    {
        var conn = _context.GetConnection();
        var reports = new List<BranchReportViewModel>();

        using (var cmdIncome = conn.CreateCommand())
        {
            cmdIncome.CommandText = "SELECT * FROM income_report_view_for_employees";
            using (var reader = await cmdIncome.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    var income = new Income
                    {
                        AccountNumber = reader.GetString("account_number"),
                        BranchId = reader.GetInt32("branch_id"),
                        OpeningDate = reader.GetDateTime("opening_date"),
                        Amount = reader.GetDouble("amount"),
                        TransactionType = TransactionType.Income
                    };

                    // Create a new report for each income
                    var report = new BranchReportViewModel
                    {
                        Incomes = new List<Income> { income }, // Add the income to the Incomes list
                        Outcomes = new List<Outcome>()         // Initialize the Outcomes list
                    };

                    reports.Add(report);
                }
            }
        }

        using (var cmdOutgo = conn.CreateCommand())
        {
            cmdOutgo.CommandText = "SELECT * FROM outgo_report_view_for_employees";
            using (var outgoReader = await cmdOutgo.ExecuteReaderAsync())
            {
                while (outgoReader.Read())
                {
                    var outcome = new Outcome
                    {
                        AccountNumber = outgoReader.GetString("account_no"),
                        BranchId = outgoReader.GetInt32("branch_id"),
                        OpeningDate = outgoReader.GetDateTime("time_stamp"),
                        Amount = outgoReader.GetDouble("amount"),
                        TransactionType = TransactionType.Outgo
                    };

                    // Find the relevant report in the list to add the outcome
                    var report = reports.Find(r => r.Incomes.Any(i => i.AccountNumber == outcome.AccountNumber));

                    if (report != null)
                    {
                        report.Outcomes.Add(outcome); // Add the outcome to the respective report
                    }
                    else
                    {
                        // If no report exists for the outcome's account number, create a new one
                        var newReport = new BranchReportViewModel
                        {
                            Incomes = new List<Income>(), // Initialize the Incomes list
                            Outcomes = new List<Outcome> { outcome } // Add the outcome to the Outcomes list
                        };
                        reports.Add(newReport); // Add the new report to the list
                    }
                }
            }
        }

        return reports;
    }


    public async Task<IEnumerable<LoanReportViewModel>> GetLoanReports()
    {
        var conn = _context.GetConnection();
        var reports = new List<LoanReportViewModel>();

        using (var cmdLoanInstallment = conn.CreateCommand())
        {
            cmdLoanInstallment.CommandText = "SELECT * FROM loan_installment_report";
            using (var reader = await cmdLoanInstallment.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    var loanInstallment = new LoanInstallments
                    {
                        Customer_id = reader.GetInt32("customer_id"),
                        NIC = reader.GetString("NIC"),
                        Name = reader.GetString("Name"),
                        Loan_id = reader.GetInt32("loan_id"),
                        BranchId = reader.GetInt32("branch_id"),
                        Last_unpaid_date = reader.GetDateTime("last_unpaid_date"),
                        No_of_missing_installments = reader.GetInt32("no_of_missing_installments"),
                        Interest = reader.GetDouble("interest"),
                        Loan_amount = reader.GetDouble("total_amount"),
                        // Assuming missing_amount is fetched from the database
                        Missing_amount = reader.GetDouble("missing_amount")
                    };

                    var report = reports.Find(r => r.loan.Any(l => l.Loan_id == loanInstallment.Loan_id));

                    if (report != null)
                    {
                        report.loan.Add(loanInstallment);
                    }
                    else
                    {
                        var newReport = new LoanReportViewModel
                        {
                            loan = new List<LoanInstallments> { loanInstallment }
                        };
                        reports.Add(newReport);
                    }
                }
            }
        }

        return reports;
    }



    public async Task AddSavingsAccount(AddBankAccountViewModel model, string accNo, int branchId)
    {
        var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "CALL add_savings_account(@acc_no, @customer_id, @branch_id, @initial_deposit, @open_date, @savings_plan_id)";
        cmd.Parameters.AddWithValue("@acc_no", accNo);
        cmd.Parameters.AddWithValue("@customer_id", model.CustomerId);
        cmd.Parameters.AddWithValue("@branch_id", branchId);
        cmd.Parameters.AddWithValue("@initial_deposit", model.InitialDeposit);
        cmd.Parameters.AddWithValue("@open_date", DateTime.Now.Date);
        cmd.Parameters.AddWithValue("@savings_plan_id", model.SavingsPlanId);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<bool> HasAccountInBranch(int customerId, int branchId, BankAccountType type)
    {
        var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT EXISTS(SELECT * FROM bank_account " +
                          "WHERE customer_id = @customer_id " +
                          "AND branch_id = @branch_id " +
                          "AND account_type = @t)";
        cmd.Parameters.AddWithValue("@customer_id", customerId);
        cmd.Parameters.AddWithValue("@branch_id", branchId);
        cmd.Parameters.AddWithValue("@t", (sbyte)(type));
        var result = await cmd.ExecuteScalarAsync();
        return (long)result == 1;
    }
}

public enum BankAccountType
{
    Current = 0,
    Savings = 1
}