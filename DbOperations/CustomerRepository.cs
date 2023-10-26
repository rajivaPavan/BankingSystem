using BankingSystem.DBContext;
using BankingSystem.Models;
using MySqlConnector;

namespace BankingSystem.DbOperations;

public interface IIndividualRepository : IRepository<Individual>
{
    Task<Individual> GetByNicAsync(string nic);
}

public class IndividualRepository :  Repository, IIndividualRepository
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
            NIC = reader.GetString("nic"),
            FirstName = reader.GetString("first_name"),
            LastName = reader.GetString("last_name"),
            DateOfBirth = reader.GetDateTime("date_of_birth"),
            CustomerId = reader.GetInt32("customer_id"),
            UserId = reader.GetInt32("user_id"),
            Gender = (Gender) reader.GetInt16("Gender"),
            MobileNumber = reader.GetString("mobile_number"),
            HomeNumber = reader.GetString("home_number"),
            Address = reader.GetString("address"),
            Email = reader.GetString("email")
        };
        return individual;
    }
    
    public async Task<IEnumerable<Individual>> GetAllAsync()
    {
        var conn =  _context.GetConnection();
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

    public async Task<Individual> GetByNicAsync(string nic)
    {
        var conn =  _context.GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM individual WHERE nic = @nic";
        cmd.Parameters.AddWithValue("@nic", nic);
        using var reader = await cmd.ExecuteReaderAsync();
        return await ReadAsync(reader);
    }
}