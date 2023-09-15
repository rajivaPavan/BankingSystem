using BankingSystem.DBContext;

namespace BankingSystem.DbOperations;

public interface IAccountRepository
{
    bool Login(string username, string password);
}

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _dbContext;

    public AccountRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public bool Login(string username, string password)
    {
        // make sql query to check if the user exists
        return false;
    }
}
