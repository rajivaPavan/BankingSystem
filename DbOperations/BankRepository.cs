using BankingSystem.DBContext;
using BankingSystem.Models;
using MySqlConnector;

namespace BankingSystem.DbOperations;

public class BankRepository
{
    private readonly string connectionString;

    public BankRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public List<Account> GetAccounts(string customerId)
    {
        // Database query to fetch account information based on customer ID
        // Populating list of Account objects
        public List<Account> GetAccounts(string customerId)
        {
            List<Account> accounts = new List<Account>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT AccountNumber, Balance, AccountType FROM BankAccount WHERE CustomerId = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Account account = new Account()
                            {
                                AccountNumber = reader.GetString(0),
                                Balance = reader.GetDecimal(1),
                                AccountType = Enum.Parse<AccountType>(reader.GetString(2))
                            };

                            accounts.Add(account);
                        }
                    }
                }
            }

            return accounts;
        }

    }
}