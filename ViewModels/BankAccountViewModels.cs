using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Net.Mime.MediaTypeNames;
using System.Numerics;

using System;
using System.Collections.Generic;
using BankingSystem.ViewModels;


namespace BankingSystem.ViewModels;

public class AddBankAccountViewModel
{
    /// <summary>
    /// Customer Id of the customer to whom the bank account is to be added.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Customer's NIC/RegNo, used to identify the customer.
    /// </summary>
    public string NicOrRegNo { get; set; }

    /// <summary>
    /// List of all savings plans available. To be selected by the customer.
    /// </summary>
    public SelectList SavingsPlans { get; set; }

    /// <summary>
    /// Selected savings plan id.
    /// </summary>
    public int SavingsPlanId { get; set; }

    /// <summary>
    /// Whether the customer wants a debit card or not.
    /// </summary>
    public bool WantDebitCard { get; set; }

    public double InitialDeposit { get; set; }
}

public class SavingsPlanViewModel
{
    public int SavingsPlanId { get; set; }
    public string Name { get; set; }
    public double InterestRate { get; set; }
    public double MinimumBalance { get; set; }
    public int MaxWithdrawals { get; set; }
}

public class BranchReportViewModel
{
    public List<Income> Incomes { get; set; }
    public List<Outcome> Outcomes { get; set; }

}

public class LoanReportViewModel
{
    public List<Income> Incomes { get; set; }
    public List<Outcome> Outcomes { get; set; }

}

public class Income
{
    public string AccountNumber { get; set; }
    public int BranchId { get; set; }
    public DateTime OpeningDate { get; set; }
    public double Amount { get; set; }
    public TransactionType TransactionType { get; set; }
}

public class Outcome
{
    public string AccountNumber { get; set; }
    public int BranchId { get; set; }
    public DateTime OpeningDate { get; set; }
    public double Amount { get; set; }
    public TransactionType TransactionType { get; set; }
}

public enum TransactionType
{
    Income,
    Outgo
}







