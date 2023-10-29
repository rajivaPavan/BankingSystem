using System.Runtime.InteropServices;
using BankingSystem.DBContext;
using BankingSystem.Models;
using BankingSystem.ViewModels;
using MySqlConnector;

namespace BankingSystem.DbOperations;

public interface IIndividualRepository : IRepository<Individual>
{
    Task<IndividualViewModel> GetIndividualInfoForEmployee(string nic);
    Task<List<IndividualViewModel>> GetChildIndividualsByNicAsync(string nic);
    Task<int> AddIndividual(IndividualViewModel model);
}

public class IndividualRepository : Repository, IIndividualRepository
{
    private readonly AppDbContext _context;

    public IndividualRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Individual> ReadAsync(MySqlDataReader reader)
    {
        if (await reader.ReadAsync() == false) return null;
        var individual = new Individual()
        {
            IndividualId = reader.GetInt32("individual_id"),
            NIC = reader.GetString("NIC"),
            FirstName = reader.GetString("first_name"),
            LastName = reader.GetString("last_name"),
            DateOfBirth = reader.GetDateTime("date_of_birth"),
            CustomerId = reader.GetInt32("customer_id"),
            Gender = reader.GetBoolean("gender") ? Gender.Female : Gender.Male,
            MobileNumber = reader.GetString("mobile_number"),
            Address = reader.GetString("address"),
        };
        if (!reader.IsDBNull(reader.GetOrdinal("user_id")))
            individual.UserId = reader.GetInt32(reader.GetOrdinal("user_id"));
        if (!reader.IsDBNull(reader.GetOrdinal("home_number")))
            individual.HomeNumber = reader.GetString(reader.GetOrdinal("home_number"));
        if (!reader.IsDBNull(reader.GetOrdinal("email")))
            individual.Email = reader.GetString(reader.GetOrdinal("email"));

        return individual;
    }

    public async Task<IEnumerable<Individual>> GetAllAsync()
    {
        var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM individual";
        using var reader = await cmd.ExecuteReaderAsync();
        var individuals = new List<Individual>();
        while (true)
        {
            var res = await ReadAsync(reader);
            if (res == null) break;
            individuals.Add(res);
        }

        return individuals;
    }

    public Task<Individual> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(Individual entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Individual entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IndividualViewModel> GetIndividualInfoForEmployee(string nic)
    {
        var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM minimal_individual_bank_account_view WHERE NIC = @nic";
        cmd.Parameters.AddWithValue("@nic", nic);
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync() == false) return null;
        var res = GetIndividualViewModel(reader);
        return res;
    }

    private static IndividualViewModel GetIndividualViewModel(MySqlDataReader reader)
    {
        var individualViewModel = new IndividualViewModel();
        individualViewModel.FirstName = reader.GetString("first_name");
        individualViewModel.LastName = reader.GetString("last_name");
        individualViewModel.CustomerId = reader.GetInt32("customer_id");
        // individualViewModel.BankAccountNumber = reader.GetString("account_no");
        // handle null
        if (!reader.IsDBNull(reader.GetOrdinal("account_no")))
        {
            individualViewModel.BankAccountNumber = reader.GetString(reader.GetOrdinal("account_no"));
            individualViewModel.BankAccountType = (BankAccountType) reader.GetInt16(reader.GetOrdinal("account_type"));
            individualViewModel.BankAccountBalance = reader.GetDouble(reader.GetOrdinal("balance"));
        }

        return individualViewModel;
    }

    public async Task<List<IndividualViewModel>> GetChildIndividualsByNicAsync(string nic)
    {
        var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM minimal_child_bank_account_view " +
                          "WHERE guardian_NIC = @nic";
        cmd.Parameters.AddWithValue("@nic", nic);
        using var reader = await cmd.ExecuteReaderAsync();
        var individuals = new List<IndividualViewModel>();
        while (await reader.ReadAsync())
        {
            var res = GetIndividualViewModel(reader);
            individuals.Add(res);
        }

        return individuals;
    }

    public async Task<int> AddIndividual(IndividualViewModel model)
    {
        var conn = _context.GetConnection();
        using var cmd = conn.CreateCommand();
        // call the stored procedure to add a new individual
        cmd.CommandText = "CALL add_new_individual(@nic, @first_name, @last_name," +
                          " @date_of_birth, @customerId, @email, @gender, @mobile_number, @home_number, @address)";
        cmd.Parameters.AddWithValue("@nic", model.NIC);
        cmd.Parameters.AddWithValue("@first_name", model.FirstName);
        cmd.Parameters.AddWithValue("@last_name", model.LastName);
        cmd.Parameters.AddWithValue("@date_of_birth", model.DateOfBirth);
        cmd.Parameters.AddWithValue("@customerId", null);
        cmd.Parameters.AddWithValue("@gender", model.Gender != Gender.Male);
        cmd.Parameters.AddWithValue("@mobile_number", model.MobileNumber);
        cmd.Parameters.AddWithValue("@home_number", model.HomeNumber);
        cmd.Parameters.AddWithValue("@address", model.Address);
        cmd.Parameters.AddWithValue("@email", model.Email);

        await cmd.ExecuteNonQueryAsync();

        // now get the customer id of the newly added customer
        cmd.CommandText =
            "Select customer_id from individual where nic = @search_nic ORDER BY customer_id DESC LIMIT 1";
        cmd.Parameters.AddWithValue("@search_nic", model.NIC);
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync() == false) return -1;
        return reader.GetInt32("customer_id");
    }
}