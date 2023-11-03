namespace BankingSystem.Models
{
    public class LCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public double ExistingFdAmount { get; set; }
        public double SavingsAccount  { get; set; }
    }

    public class LoanProduct
    {
        public int LoanId { get; set; }
        public string Name { get; set; }
        public decimal InterestRate { get; set; }
    }

    public class LoanApplication
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public LCustomer Customer { get; set; }
        public int LoanPlanId { get; set; }
        public LoanProduct LoanPlan { get; set; }
        public decimal Amount { get; set; }
        public DateTime ApplicationDate { get; set; }
        
    }

    public class LoanRepayment
    {
        public int Id { get; set; }
        public int LoanApplicationId { get; set; }
        public LoanApplication LoanApplication { get; set; }
        public decimal Amount { get; set; }
        public DateTime RepaymentDate { get; set; }
        
    }

    public class LoanApplicationModel
    {
        public string CustomerId { get; set; }
        public decimal RequestedAmount { get; set; }
    }

    public class CustomerLoanViewModel
    {
        public Customer Customer { get; set; }
        public List<LoanProduct> Loans { get; set; }
    }
}

